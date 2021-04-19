using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Storage
{
    /// <summary>
    /// Класс продукта.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дескрипшн.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Цена 1. В рублях, потому что люблю нашу страну.
        /// </summary>
        public double Price1 { get; set; }
        /// <summary>
        /// Цена 2. 
        /// </summary>
        public double Price2 { get; set; }
        /// <summary>
        /// Артикул.
        /// </summary>
        public string Article { get; set; }
        /// <summary>
        /// Число товара.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Гарантия.
        /// </summary>
        public string Guarantee { get; set; }
        /// <summary>
        /// Проверить, как там с товаром обстоит вопрос.
        /// </summary>
        public bool InNeed
        {
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
        public Product(string name, string description = "", string article = "", int amount = 0, double price1 = 0, double price2 = 0, string guarantee = "")
        {
            Name = name;
            Description = description;
            Article = article;
            Price1 = price1;
            Price2 = price2;
            Amount = amount;
            Guarantee = guarantee;
        }
        /// <summary>
        /// Получить цсв представление продукта.
        /// </summary>
        /// <returns></returns>
        public string GetLine()
        {
            string result = "\"";
            List<string> t = new List<string>();
            t.Add(Name);
            t.Add(Description);
            t.Add(Article);
            t.Add(Amount.ToString());
            t.Add(Price1.ToString());
            t.Add(Price2.ToString());
            t.Add(Guarantee);
            result += String.Join("\",\"", t) + "\"";
            return result;
        }
        /// <summary>
        /// Получить массив объектов(свойства товара).
        /// </summary>
        /// <returns></returns>
        public object[] GetArray()
        {
            List<object> t = new List<object>();
            t.Add(Name);
            t.Add(Description);
            t.Add(Article);
            t.Add(Amount);
            t.Add(Price1);
            t.Add(Price2);
            t.Add(Guarantee);
            return t.ToArray();
        }
        /// <summary>
        /// Создать продукт по массиву(его освойств).
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
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
