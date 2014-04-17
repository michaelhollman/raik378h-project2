using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raik378h_project2
{
    class Item
    {
        public int ItemId { get; set; }
        public string Review { get; set; }

        public void Print()
        {
            Console.WriteLine("Item: {0}", ItemId);
            Console.WriteLine("Review: {0}", Review);
        }
    }
}
