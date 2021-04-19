using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Storage
{
    public partial class CategoryCreationView : Form
    {
        private StorageNode _parentNode;
        private StorageNode _result;
        public StorageNode Result { get => _result; }
        public CategoryCreationView(StorageNode parentNode)
        {
            _result = null;
            _parentNode = parentNode;
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            string text = nameBox.Text;
            if (text.Length < 4)
            {
                MessageBox.Show("Category name must contain at least 4 symbols.");
                return;
            }
            if (text.Length > 50)
            {
                MessageBox.Show("Category name must contain less than 50 symbols.");
                return;
            }
            if (text.IndexOfAny( new char[] { '/','\\','|', '?', '*', '!', ',', '<', '>'}) >= 0)
            {
                MessageBox.Show("Forbidden symbols!");
                return;
            }
            if(_parentNode != null)
            {
                if (Utils.FindNode((StorageNode)_parentNode, text) == null)
                {
                    _result = new StorageNode(text);
                }
                else
                {
                    MessageBox.Show($"There is already a node with name {text} in {_parentNode.Text}.");
                    return;
                }
            }
            else
            {
                _result = new StorageNode(text);
            }
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _result = null;
            Close();
        }
    }
}
