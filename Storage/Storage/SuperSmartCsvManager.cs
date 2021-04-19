using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Storage
{
    /// <summary>
    /// Название говорит само за себя, мне стыдно, я писал это пьяным(оставшуюся часть, видимо, тоже).
    /// </summary>
    public class SuperSmartCsvManager
    {
        /// <summary>
        /// Прочитать csv, вернуть массив массивов.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object[][] ReadCsv(string path)
        {
            List<List<object>> result = new List<List<object>>();
            string[] lines = File.ReadAllLines(path);
            try
            {
                result.Add(new List<object>(Split(lines[0])));
                for (int i = 1; i < lines.Length; ++i)
                {
                    List<object> temp = new List<object>();
                    string[] line = Split(lines[i]);
                    for (int j = 0; j < line.Length; ++j)
                    {
                        temp.Add(ConvertStringToObj(line[j], j));
                    }
                    result.Add(temp);
                }
                return result.Select(x => x.ToArray()).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Законвертить массив в цсв строчку.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertArrayToCsvLine(object[] array)
        {
            string result = "\"";
            List<string> temp = new List<string>();
            foreach (object o in array)
            {
                temp.Add(o.ToString());
            }
            result += String.Join("\",\"", temp) + "\"";
            return result;
        }
        /// <summary>
        /// Строка в объект.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object ConvertStringToObj(string input, int index)
        {
            try
            {
                if (Program.ConvertTo[index] == typeof(double))
                {
                    return double.Parse(input);
                }
                else if (Program.ConvertTo[index] == typeof(int))
                {
                    return int.Parse(input);
                }
                else if (Program.ConvertTo[index] == typeof(string))
                {
                    return input;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Записать в цсв файл.
        /// </summary>
        /// <param name="storageNode"></param>
        /// <returns></returns>
        public static bool WriteToCsv(StorageNode storageNode)
        {
            try
            {
                string path = Utils.GetCsvByNode(storageNode);
                List<string> result = new List<string>();
                result.Add(ConvertArrayToCsvLine(Program.CsvHeader));
                foreach (var product in storageNode.Cathegory.Products)
                {
                    result.Add(product.GetLine());
                }
                File.WriteAllLines(path, result);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Засплитить строку как цсвшку.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string[] Split(string line)
        {
            string[] result = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
            int length = result.Length;
            result[0] = result[0].Substring(1);
            result[length - 1] = result[length - 1].Substring(0, result[length - 1].Length - 1);
            return result;
        }
    }
}
