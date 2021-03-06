using System;
using System.Text;

namespace Storage
{
    /// <summary>
    /// Класс-генератор хэшей и всего того, чего я очень не люблю.
    /// </summary>
    public static class HashStringUtils
    {
        /// <summary>
        /// Метод расширения для умножения строки на число.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static string Multiply(this string source, int multiplier)
        {
            StringBuilder sb = new StringBuilder(multiplier * source.Length);
            for (int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Генерация артикля.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateArticle(Product product, int length)
        {
            string result = "";
            int seed = product.GetHashCode();
            Random rnd = new Random(seed);
            var hash = (product.GetHashCode() + rnd.Next()).GetHashCode();
            hash *= hash;
            string hashline = Math.Abs(hash).ToString();
            int repeat = (int)Math.Ceiling(length / (hashline.Length * 1.0));
            hashline = hashline.Multiply(repeat);
            for (int i = 0; i < length; ++i)
            {
                result += hashline[i];
                if (i % 3 == 2 && i != length - 1)
                {
                    result += "-";
                }
            }
            return result;
        }
    }
}
