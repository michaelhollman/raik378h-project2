using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raik378h_project2
{
    class Basket
    {
        public int customer_id;
        public string state;
        public string weekday;
        public int item_num;
        public List<Item> items;

        public void print()
        {
            Console.WriteLine("Customer Id: {0}", customer_id);
            Console.WriteLine("State: {0}", state);
            Console.WriteLine("Date: {0}", weekday);
            foreach (Item item in items)
            {
                item.print();
            }
        }
    }
}
