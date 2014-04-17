using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raik378h_project2
{
    class Program
    {
        static void Main(string[] args)
        {
            //for (var i = 0; i < 2999; i++)
            //{
            //    var b = Basket.ReadFromFile(i);
            //    b.Print();
            //}


            /*
             * 1) LET C1 = all Items that appear in the file D; 
                2) FOR i : = 1 TO q DO BEGIN 
                3) Fi := those sets in Ci that occur as least s times in D; 
                4) IF i = q BREAK; 
                5) LET Ci+1 = all itemsets S of size i+1 such that every 
                subset of S of size i is in Fi 
                6) END 
             */


            int threshold = 3;
            List<Basket> allBaskets = new List<Basket>();

            for (var i = 0; i < 3000; i++)
            {
                allBaskets.Add(Basket.ReadFromFile(i));
            }

            Dictionary<int, int> oneItemCount = new Dictionary<int, int>();
            Dictionary<HashSet<int>, int> twoItemCount = new Dictionary<HashSet<int>, int>();
            Dictionary<HashSet<int>, int> threeItemCount = new Dictionary<HashSet<int>, int>();

            foreach (Basket basket in allBaskets)
            {
                foreach (Item item in basket.Items)
                {
                    //var itemSet = new HashSet<Item>();
                    //itemSet.Add(item);

                    //currentCount will be 0 (default value of int) if key id doesn't exist
                    int currentCount;
                    oneItemCount.TryGetValue(item.ItemId, out currentCount);
                    oneItemCount[item.ItemId] = currentCount + 1;
                }
            }

            // get the item sets from the dictionary where count > new threshold value (on second pass, this will be 2, then 3, etc.)
            twoItemCount = twoItemCount.Where(i => i.Value >= threshold)
                    .ToDictionary(i => i.Key, i => i.Value);

            foreach (Basket basket in allBaskets)
            {
                for (int i = 0; i < basket.Items.Count - 2; i++)
                {
                    for (int j = i + 1; j < basket.Items.Count - 1; j++)
                    {
                        for (int k = j + 1; k < basket.Items.Count; k++)
                        {
                            var allThree = new HashSet<int>();
                            allThree.Add(basket.Items[i].ItemId);
                            allThree.Add(basket.Items[j].ItemId);
                            allThree.Add(basket.Items[k].ItemId);

                            /*if (threeItemCount.ContainsKey(allThree)){
                                threeItemCount[allThree] 
                            }*/
                            int currentCount;
                            threeItemCount.TryGetValue(allThree, out currentCount);
                            if (currentCount > 0)
                            {
                                threeItemCount[allThree] = currentCount++;
                            }
                            else
                            {
                                // make jk, ik, and ij
                                var jk = new HashSet<int>();
                                jk.Add(basket.Items[j].ItemId);
                                jk.Add(basket.Items[k].ItemId);

                                var ik = new HashSet<int>();
                                ik.Add(basket.Items[i].ItemId);
                                ik.Add(basket.Items[k].ItemId);

                                var ij = new HashSet<int>();
                                ij.Add(basket.Items[i].ItemId);
                                ij.Add(basket.Items[j].ItemId);

                                // check if jk, ik, and ij are in 2itemcount
                                if (twoItemCount.ContainsKey(jk) && twoItemCount.ContainsKey(ik) && twoItemCount.ContainsKey(ij))
                                {
                                    //if yes to all 3, add ijk to 3itemcount
                                    threeItemCount[allThree] = currentCount++;
                                }
                            }
                        }
                    }
                }

                threeItemCount = threeItemCount.Where(i => i.Value >= threshold).ToDictionary(i => i.Key, i => i.Value);


            }

            Console.Read();

        }
    }
}
