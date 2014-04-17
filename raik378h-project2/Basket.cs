using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace raik378h_project2
{
    class Basket
    {
        public int CustomerId { get; set; }
        public string State { get; set; }
        public string Weekday { get; set; }
        public List<Item> Items { get; set; }

        public void Print()
        {
            Console.WriteLine("Customer Id: {0}", CustomerId);
            Console.WriteLine("State: {0}", State);
            Console.WriteLine("Date: {0}", Weekday);
            foreach (Item item in Items)
            {
                item.Print();
            }
        }

        public static Basket ReadFromFile(int fileNum)
        {
            var bytes = File.ReadAllBytes(string.Format("../../data/basket_{0:000000}.dat", fileNum));
            var basket = new Basket
            {
                CustomerId = BitConverter.ToInt32(bytes, 0),
                State = System.Text.Encoding.ASCII.GetString(bytes.Skip(4).TakeWhile(b => b != '\0').ToArray()),
                Weekday = System.Text.Encoding.ASCII.GetString(bytes.Skip(68).TakeWhile(b => b != '\0').ToArray()),
                Items = new List<Item>(),
            };

            var itemNum = BitConverter.ToInt32(bytes, 132);
            for (var i = 0; i < itemNum; i++)
            {
                basket.Items.Add(new Item
                {
                    ItemId = BitConverter.ToInt32(bytes, 136 + 1028 * i),
                    Review = System.Text.Encoding.ASCII.GetString(bytes.Skip(136 + 1028 * i + 4).TakeWhile(b => b != '\0').ToArray()),
                });
            }

            return basket;
        }
    }
}
