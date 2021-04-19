using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storage
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public string Article { get; set; }
        public int Amount { get; set; }
        public string Guarantee { get; set; }
        public bool InNeed {
            get
            {
                if (Amount <= Storage.WarnProductAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Product(string name, string description="", string article="", int amount = 0, double price1 = 0, double price2 = 0,  string guarantee="")
        {
            Name = name;
            Description = description;
            Article = article;
            Price1 = price1;
            Price2 = price2;
            Amount = amount;
            Guarantee = guarantee;
        }
        public string GetLine()
        {
            string result = "\"";
            List<string> t = new List<string>();
            t.Add(Name);
            t.Add(Description);
            t.Add(Article);
            t.Add(Price1.ToString());
            t.Add(Price2.ToString());
            t.Add(Amount.ToString());
            t.Add(Guarantee);
            result += String.Join("\",\"", t) + "\"";
            return result;
        }
        public object[] GetArray()
        {
            List<object> t = new List<object>();
            t.Add(Name);
            t.Add(Description);
            t.Add(Article);
            t.Add(Price1);
            t.Add(Price2);
            t.Add(Amount);
            t.Add(Guarantee);
            return t.ToArray();
        }
        public static Product FromArray(object[] line)
        {
            try
            {
                Product product = new Product(
                    name: (string)line[0],
                    description: (string)line[1],
                    article: (string)line[2],
                    amount: (int)line[3],
                    price1: (double)line[4],
                    price2: (double)line[5],
                    guarantee: (string)line[6]
                    );
                return product;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
