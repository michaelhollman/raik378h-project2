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
            for (var i = 0; i < 2999; i++)
            {
                var b = Basket.ReadFromFile(i);
                b.Print();
            }


            /*
             * 1) LET C1 = all Items that appear in the file D; 
                2) FOR i : = 1 TO q DO BEGIN 
                3) Fi := those sets in Ci that occur as least s times in D; 
                4) IF i = q BREAK; 
                5) LET Ci+1 = all itemsets S of size i+1 such that every 
                subset of S of size i is in Fi 
                6) END 
             */

            /*
            int threshold = 3;
            List<Basket> allBaskets = new List<Basket>();
            Dictionary<HashSet<Item>, int> itemCount = new Dictionary<HashSet<Item>,int>();
            
            foreach (Basket basket in allBaskets)
            {
                foreach(Item item in basket.Items) 
                {
                    var itemSet = new HashSet<Item>();
                    itemSet.Add(item);
                    
                    //currentCount will be 0 (default value of int) if key id doesn't exist
                    int currentCount;
                    itemCount.TryGetValue(itemSet, out currentCount);
                    itemCount[itemSet] = currentCount + 1;
                }
            }
            
            // get the item sets from the dictionary where count > new threshold value (on second pass, this will be 2, then 3, etc.)
            // then go through each basket and if the basket has 2 Items from the list of super special high frequency Items, iterate count in new itemCount dict.




            List<Item> c1 = new List<Item>();
            */


            Console.Read();

        }
    }
}
