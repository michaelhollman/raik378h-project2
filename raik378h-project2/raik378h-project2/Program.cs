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
            /*
             * 1) LET C1 = all items that appear in the file D; 
                2) FOR i : = 1 TO q DO BEGIN 
                3) Fi := those sets in Ci that occur as least s times in D; 
                4) IF i = q BREAK; 
                5) LET Ci+1 = all itemsets S of size i+1 such that every 
                subset of S of size i is in Fi 
                6) END 
             */

            int threshold = 3;
            List<Basket> allBaskets = new List<Basket>();
            Dictionary<HashSet<Item>, int> oneItemCount = new Dictionary<HashSet<Item>,int>();
            Dictionary<HashSet<Item>, int> twoItemCount = new Dictionary<HashSet<Item>, int>();
            Dictionary<HashSet<Item>, int> threeItemCount = new Dictionary<HashSet<Item>, int>();

            foreach (Basket basket in allBaskets)
            {
                foreach(Item item in basket.items) 
                {
                    var itemSet = new HashSet<Item>();
                    itemSet.Add(item);
                    
                    //currentCount will be 0 (default value of int) if key id doesn't exist
                    int currentCount;
                    oneItemCount.TryGetValue(itemSet, out currentCount);
                    oneItemCount[itemSet] = currentCount + 1;
                }
            }
            
            // get the item sets from the dictionary where count >= threshold
            oneItemCount = oneItemCount.Where(i => i.Value >= threshold)
                    .ToDictionary(i => i.Key, i => i.Value);

            // then go through each basket and if the basket has 2 items from the list of super special high frequency items, iterate count in new itemCount dict.
            foreach (Basket basket in allBaskets)
            {
                for (int i = 0; i < basket.items.Count - 1; i++)
                    for (int j = i+1; j < basket.items.Count; j++ )
                    {
                        var itemI = new HashSet<Item>();
                        itemI.Add(basket.items[i]);
                        var itemJ = new HashSet<Item>();
                        itemJ.Add(basket.items[j]);
                        if (oneItemCount.ContainsKey(itemI) && oneItemCount.ContainsKey(itemJ)) {
                            var doubleItemSet = new HashSet<Item>();
                            doubleItemSet.Add(basket.items[i]);
                            doubleItemSet.Add(basket.items[j]);

                            int currentCount;
                            twoItemCount.TryGetValue(doubleItemSet, out currentCount);
                            twoItemCount[doubleItemSet] = currentCount + 1;
                        }
                    }
            }

            // get the item sets from the dictionary where count > new threshold value (on second pass, this will be 2, then 3, etc.)
            twoItemCount = twoItemCount.Where(i => i.Value >= threshold)
                    .ToDictionary(i => i.Key, i => i.Value);

            foreach (Basket basket in allBaskets)
            {
                for (int i = 0; i < basket.items.Count - 2; i++)
                {
                    for (int j = i+1; j < basket.items.Count - 1; j++)
                    {
                        for (int k = j + 1; k < basket.items.Count; k++)
                        {
                            var allThree = new HashSet<Item>();
                            allThree.Add(basket.items[i]);
                            allThree.Add(basket.items[j]);
                            allThree.Add(basket.items[k]);

                            /*if (threeItemCount.ContainsKey(allThree)){
                                threeItemCount[allThree] 
                            }*/
                            int currentCount;
                            threeItemCount.TryGetValue(allThree, out currentCount);
                            if (currentCount > 0) {
                                threeItemCount[allThree] = currentCount++;
                            }
                            else {
                            // make jk, ik, and ij
                                var jk = new HashSet<Item>();
                                jk.Add(basket.items[j]);
                                jk.Add(basket.items[k]);

                                var ik = new HashSet<Item>();
                                ik.Add(basket.items[i]);
                                ik.Add(basket.items[k]);

                                var ij = new HashSet<Item>();
                                ij.Add(basket.items[i]);
                                ij.Add(basket.items[j]);
                                
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
                threeItemCount = threeItemCount.Where(i => i.Value >= threshold)
                     .ToDictionary(i => i.Key, i => i.Value);                        

            }

            Console.Read();
        
        }
    }
}
