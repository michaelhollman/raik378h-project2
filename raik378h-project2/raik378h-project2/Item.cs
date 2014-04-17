using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raik378h_project2
{
    class Item
    {
        public int item_id;
        public string review;

        public void print()
        {
            Console.WriteLine("Item: {0}", item_id);
            Console.WriteLine("Review: {0}", review);
        }
    }
}
