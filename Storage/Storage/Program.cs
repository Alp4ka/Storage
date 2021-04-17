using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storage
{
    static class Program
    {
        public static string[] CsvHeader = new string[] { "Name", "Description", "Article", "Amount", "Price1", "Price2", "Guarantee" };
        public static List<Type> ConvertTo = new List<Type>() {typeof(string), typeof(string), typeof(string), typeof(int), typeof(double), typeof(double), typeof(string) };
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
