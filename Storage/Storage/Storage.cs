using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class Storage : IStorable
    {
        private static int warnProductAmount = 10;
        public static int WarnProductAmount {
            get => warnProductAmount;
            set
            {
                warnProductAmount = value >= 1 ? value : warnProductAmount;
            }
        }
        public static List<Product> Products { get; set; }
        public static List<Cathegory> Cathegories { get; set; }
        static Storage()
        {
            Products = new List<Product>();
            Cathegories = new List<Cathegory>();
        }

    }
}
