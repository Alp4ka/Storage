using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price1 { get; set; }
        public int Price2 { get; set; }
        public string Article { get; set; }
        public int Amount { get; set; }
        public string Guarantee { get; set; }
        public bool InNeed {
            get
            {
                if (Amount <= Program.WarnProductAmount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
    }
}
