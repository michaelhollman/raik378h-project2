using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace raik378h_project2
{
    public class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            var threshold = 3;

            Dictionary<int, int> oneItemDict = new Dictionary<int, int>();
            Dictionary<Tuple<int, int>, int> twoItemDict = new Dictionary<Tuple<int, int>, int>();
            Dictionary<Tuple<int, int, int>, int> threeItemDict = new Dictionary<Tuple<int, int, int>, int>();

            List<Basket> allBaskets = new List<Basket>();
            for (var i = 0; i < 3000; i++)
            {
                allBaskets.Add(Basket.ReadFromFile(i));
            }

            var fileReadTime = watch.ElapsedMilliseconds;

            foreach (Basket basket in allBaskets)
            {
                foreach (Item item in basket.Items)
                {
                    //currentCount will be 0 (default value of int) if key id doesn't exist
                    int currentCount;
                    oneItemDict.TryGetValue(item.ItemId, out currentCount);
                    oneItemDict[item.ItemId] = currentCount + 1;
                }
            }

            oneItemDict = oneItemDict.Where(i => i.Value >= threshold).ToDictionary(i => i.Key, i => i.Value);

            // then go through each basket and if the basket has 2 items from the list of super special high frequency items, iterate count in new itemCount dict.
            foreach (Basket basket in allBaskets)
            {
                for (int i = 0; i < basket.Items.Count - 1; i++)
                {
                    for (int j = i + 1; j < basket.Items.Count; j++)
                    {
                        if (oneItemDict.ContainsKey(basket.Items[i].ItemId) && oneItemDict.ContainsKey(basket.Items[j].ItemId))
                        {
                            var itemTuple = new Tuple<int, int>(basket.Items[i].ItemId, basket.Items[j].ItemId);

                            int currentCount;
                            twoItemDict.TryGetValue(itemTuple, out currentCount);
                            twoItemDict[itemTuple] = currentCount + 1;
                        }
                    }
                }
            }

            // get the item sets from the dictionary where count > new threshold value (on second pass, this will be 2, then 3, etc.)
            twoItemDict = twoItemDict.Where(i => i.Value >= threshold).ToDictionary(i => i.Key, i => i.Value);

            foreach (Basket basket in allBaskets)
            {
                for (int i = 0; i < basket.Items.Count - 2; i++)
                {
                    for (int j = i + 1; j < basket.Items.Count - 1; j++)
                    {
                        for (int k = j + 1; k < basket.Items.Count; k++)
                        {
                            var allThree = new Tuple<int, int, int>(basket.Items[i].ItemId, basket.Items[j].ItemId, basket.Items[k].ItemId);

                            int currentCount;
                            threeItemDict.TryGetValue(allThree, out currentCount);
                            if (currentCount > 0)
                            {
                                threeItemDict[allThree] = currentCount + 1;
                            }
                            else
                            {
                                // make jk, ik, and ij
                                var jk = new Tuple<int, int>(basket.Items[j].ItemId, basket.Items[k].ItemId);
                                var ik = new Tuple<int, int>(basket.Items[i].ItemId, basket.Items[k].ItemId);
                                var ij = new Tuple<int, int>(basket.Items[i].ItemId, basket.Items[j].ItemId);

                                // check if jk, ik, and ij are in 2itemcount
                                if (twoItemDict.ContainsKey(jk) && twoItemDict.ContainsKey(ik) && twoItemDict.ContainsKey(ij))
                                {
                                    //if yes to all 3, add ijk to 3itemcount
                                    threeItemDict[allThree] = currentCount + 1;
                                }
                            }
                        }
                    }
                }
            }

            threeItemDict = threeItemDict.Where(i => i.Value >= threshold).ToDictionary(i => i.Key, i => i.Value);

            watch.Stop();
            var totalRunTime = watch.ElapsedMilliseconds;

            File.WriteAllLines(@"../../output.txt", threeItemDict.Select(d => string.Format("({0}, {1}, {2}) {3}", d.Key.Item1, d.Key.Item2, d.Key.Item3, d.Value)));

            Console.WriteLine("Number of 3 item sets: {0}", threeItemDict.Count);
            Console.WriteLine("File read time: {0}", fileReadTime);
            Console.WriteLine("Data analysis runtime: {0}", totalRunTime - fileReadTime);
            Console.WriteLine("Total Execution time: {0} milliseconds", totalRunTime);

            // wait hack
            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}
