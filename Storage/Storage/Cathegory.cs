using System.Collections.Generic;

namespace Storage
{
    public class Cathegory
    {
        private const int _recursionDepth = 15;
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public List<Cathegory> Cathegories { get; set; }
        public List<Product> GetAllProducts()
        {
            List<Product> result = new List<Product>();
            RecursiveProducts(ref result, this, _recursionDepth);
            return result;
        }
        public void RecursiveProducts(ref List<Product> result, Cathegory cathegory, int curDepth)
        {
            if (curDepth <= 0)
            {
                return;
            }
            result.AddRange(cathegory.Products);
            foreach (Cathegory c in cathegory.Cathegories)
            {
                RecursiveProducts(ref result, c, curDepth - 1);
            }
        }
        public Cathegory()
        {
            Name = "";
            Products = new List<Product>();
            Cathegories = new List<Cathegory>();
        }
        public Cathegory(string name) : this()
        {
            Name = name;
        }
    }
}
