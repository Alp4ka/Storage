using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Storage
{
    public class SuperSmartCsvManager
    {
        public static object[][] ReadCsv(string path)
        {
            List<List<object>> result = new List<List<object>>();
            string[] lines = File.ReadAllLines(path);
            try
            {
                result.Add(new List<object>(Split(lines[0])));
                for(int i = 1; i < lines.Length; ++i)
                {
                    List<object> temp = new List<object>();
                    string[] line = Split(lines[i]);
                    for(int j = 0; j < line.Length; ++j)
                    {
                        temp.Add(ConvertStringToObj(line[j], j));
                    }
                    result.Add(temp);
                }
                return result.Select(x => x.ToArray()).ToArray();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        } 
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
        private static string[] Split(string line)
        {
            string[] result = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
            int length = result.Length;
            result[0] = result[0].Substring(1);
            result[length-1] = result[length - 1].Substring(0, result[length - 1].Length-1);
            return result;
        }
    }
}
