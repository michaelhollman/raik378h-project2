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

            var oneItemDict = new Dictionary<int, int>();
            var twoItemDict = new Dictionary<Tuple<int, int>, int>();
            var threeItemDict = new Dictionary<Tuple<int, int, int>, int>();
            var threeItemBaskets = new Dictionary<Tuple<int, int, int>, List<Basket>>();

            var sentiments = ReadSentimentFile();

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
                                threeItemBaskets[allThree].Add(basket);
                            }
                            else // == 0
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
                                    threeItemBaskets.Add(allThree, new List<Basket>() { basket });
                                }
                            }
                        }
                    }
                }
            }

            //threeItemDict = threeItemDict.Where(i => i.Value >= threshold).ToDictionary(i => i.Key, i => i.Value);
            //threeItemBaskets = threeItemBaskets.Where(i => threeItemDict.ContainsKey(i.Key)).ToDictionary(i => i.Key, i => i.Value);

            threeItemDict = threeItemDict.Where(i => i.Value >= threshold).Select(i =>
            {
                var sorted = new SortedSet<int>(new int[] { i.Key.Item1, i.Key.Item2, i.Key.Item3 }).ToArray();
                return new KeyValuePair<Tuple<int, int, int>, int>(new Tuple<int, int, int>(sorted[0], sorted[1], sorted[2]), i.Value);
            }).OrderBy(i => i.Key.Item1).ThenBy(i => i.Key.Item2).ThenBy(i => i.Key.Item3).ToDictionary(i => i.Key, i => i.Value);
            threeItemBaskets = threeItemBaskets.Select(i =>
            {
                var sorted = new SortedSet<int>(new int[] { i.Key.Item1, i.Key.Item2, i.Key.Item3 }).ToArray();
                return new KeyValuePair<Tuple<int, int, int>, List<Basket>>(new Tuple<int, int, int>(sorted[0], sorted[1], sorted[2]), i.Value);
            }).Where(i => threeItemDict.ContainsKey(i.Key)).OrderBy(i => i.Key.Item1).ThenBy(i => i.Key.Item2).ThenBy(i => i.Key.Item3).ToDictionary(i => i.Key, i => i.Value);


            var lines = threeItemDict.Select(d =>
                {
                    var daysToSentiments = new Dictionary<string, int>();
                    threeItemBaskets[d.Key].ForEach(b =>
                        {
                            if (!daysToSentiments.ContainsKey(b.Weekday))
                                daysToSentiments.Add(b.Weekday, 0);

                            b.Items.Where(i => d.Key.Item1 == i.ItemId || d.Key.Item2 == i.ItemId || d.Key.Item3 == i.ItemId).ToList().ForEach(i =>
                            {
                                daysToSentiments[b.Weekday] = daysToSentiments[b.Weekday] + i.Review.Split(' ').Sum(w => sentiments.ContainsKey(w) ? sentiments[w] : 0);
                            });
                        });
                    daysToSentiments = daysToSentiments.OrderBy(kvp => DayOfWeekToInt(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    var detais = new StringBuilder();
                    daysToSentiments.ToList().ForEach(ds => detais.AppendFormat(" {0} {1}", ds.Key, ds.Value));

                    return string.Format("({0}, {1}, {2}) {3} {4}", d.Key.Item1, d.Key.Item2, d.Key.Item3, d.Value, detais.ToString().Trim());
                });

            watch.Stop();
            var totalRunTime = watch.ElapsedMilliseconds;

            File.WriteAllLines(@"../../output.txt", lines);

            Console.WriteLine("Number of 3 item sets: {0}", threeItemBaskets.Count);
            Console.WriteLine("File read time: {0}", fileReadTime);
            Console.WriteLine("Data analysis runtime: {0}", totalRunTime - fileReadTime);
            Console.WriteLine("Total Execution time: {0} milliseconds", totalRunTime);


            // wait hack
            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        static Dictionary<string, int> ReadSentimentFile()
        {
            var dict = new Dictionary<string, int>();
            var reader = new StreamReader(File.OpenRead(@"../../sentiment.csv"));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var values = line.Split('\t');
                    dict.Add(values[0], int.Parse(values[1]));
                }
            }
            reader.Close();
            return dict;
        }


        static readonly List<string> daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        static int DayOfWeekToInt(string day)
        {
            return daysOfWeek.IndexOf(day);
        }

        static int RuhRoh(string s)
        {
            Console.WriteLine("Ruhroh: " + s);
            return 0;
        }
    }
}
