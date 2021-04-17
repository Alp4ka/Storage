using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class Cathegory : IStorable
    {
        //public IStorable Parent { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public List<Cathegory> Cathegories { get; set; }
        public List<Product> GetAllProducts()
        {
            List<Product> result = new List<Product>();
            RecursiveProducts(result, this);
            return result.ToHashSet().ToList();
        }
        public void RecursiveProducts(List<Product> result, Cathegory cathegory)
        {
            result.AddRange(Products);
            foreach (Cathegory c in Cathegories)
            {
                RecursiveProducts(result, c);
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
