using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage;

namespace System.Windows.Forms
{
    public class StorageNode : TreeNode{
        public Cathegory Cathegory { get; set; }
        public StorageNode() : base() { }
        public StorageNode(string text) : base(text) { }
    }
}