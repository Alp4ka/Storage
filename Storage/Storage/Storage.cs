using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storage
{
    public class Storage
    {
        private static int warnProductAmount = 10;
        public static int WarnProductAmount
        {
            get => warnProductAmount;
            set
            {
                warnProductAmount = value >= 1 ? value : warnProductAmount;
            }
        }
        public static List<Product> GetProductsInWarn()
        {
            return Products.Where(x => x.InNeed).ToList();
        }
        public static List<string> GetCsvFromList(List<Product> products)
        {
            List<string> result = new List<string>();
            result.Add(SuperSmartCsvManager.ConvertArrayToCsvLine(Program.CsvHeader));
            foreach(Product product in products)
            {
                result.Add(product.GetLine());
            }
            return result;
        }
        public static List<Product> Products { get; set; }
        public static List<Cathegory> Cathegories { get; set; }
        static Storage()
        {
            Products = new List<Product>();
            Cathegories = new List<Cathegory>();
        }
        public static void RemoveCathegory(Cathegory cathegory)
        {
            var products = cathegory.GetAllProducts();
            Cathegories.Remove(cathegory);
            for (int i = 0; i < products.Count; ++i)
            {
                Products.Remove(products[i]);
            }
        }
    }
}
