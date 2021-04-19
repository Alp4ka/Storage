using System.Collections.Generic;
using System.Linq;

namespace Storage
{
    public class Storage
    {
        private static int warnProductAmount = 10;
        /// <summary>
        /// Число продуктов, после которого бьем тревогу и включаем в отчет.
        /// </summary>
        public static int WarnProductAmount
        {
            get => warnProductAmount;
            set
            {
                warnProductAmount = value >= 1 ? value : warnProductAmount;
            }
        }
        /// <summary>
        /// Получить продукты, которые находятся в красной книге.
        /// </summary>
        /// <returns></returns>
        public static List<Product> GetProductsInWarn()
        {
            return Products.Where(x => x.InNeed).ToList();
        }
        public static List<string> GetCsvFromList(List<Product> products)
        {
            List<string> result = new List<string>();
            result.Add(SuperSmartCsvManager.ConvertArrayToCsvLine(Program.CsvHeader));
            foreach (Product product in products)
            {
                result.Add(product.GetLine());
            }
            return result;
        }
        /// <summary>
        /// Массив всех продуктов.
        /// </summary>
        public static List<Product> Products { get; set; }
        /// <summary>
        /// Массив категорий.
        /// </summary>
        public static List<Cathegory> Cathegories { get; set; }
        static Storage()
        {
            Products = new List<Product>();
            Cathegories = new List<Cathegory>();
        }
        /// <summary>
        /// Удалить категорию.
        /// </summary>
        /// <param name="cathegory"></param>
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
