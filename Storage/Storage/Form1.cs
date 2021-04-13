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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCategories();
        }

        private void InitializeCategories()
        {
            TreeNode[] nodes = Utils.InitializeCategories();
            storageTree.Nodes.Clear();
            foreach (var node in nodes)
            {
                storageTree.Nodes.Add(node);
            }
        }

        private void storageTree_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {

            }
        }

        private void createNewCategory_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            var ccvDialog = new CategoryCreationView(selectedNode);
            ccvDialog.ShowDialog();
            var result = ccvDialog.Result;
            if (result != null)
            {
                if (selectedNode != null)
                {
                    selectedNode.Nodes.Add(result);
                }
                else
                {
                    storageTree.Nodes.Add(result);
                }
                Utils.CreateCategoryInPath(result);
            }
        }

        private void deleteSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if(selectedNode != null)
            {
                DialogResult result = MessageBox.Show($"Are you sure want to delete {selectedNode.Text}?", "Deletion", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Utils.RemoveCategoryFromPath(selectedNode);
                    var parent = selectedNode.Parent;
                    if (parent != null)
                    {
                        selectedNode.Parent.Nodes.Remove(selectedNode);
                    }
                    else
                    {
                        storageTree.Nodes.Remove(selectedNode);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Utils.HighlightNodesByContaining(storageTree, textBox1.Text);
        }

        private void changeSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if(selectedNode != null)
            {
                CategoryCreationView ccv = new CategoryCreationView(selectedNode.Parent);
                ccv.ShowDialog();
                if (ccv.Result != null)
                {
                    Utils.RenameCategoryTo(selectedNode, ccv.Result.Text);
                }
            }
            
        }

        private void storageTree_DoubleClick(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if(selectedNode!= null)
            {
                string csvPath = Utils.GetCsvByNode(selectedNode);
                dataGrid.Columns.Clear();
                dataGrid.Rows.Clear();
                var csvResult = SuperSmartCsvManager.ReadCsv(csvPath);
                if(csvResult == null)
                {
                    MessageBox.Show("Error while reading csv file!");
                    return;
                }
                foreach (string h in csvResult[0])
                {
                    dataGrid.Columns.Add(h, h);
                }
                for(int i = 1; i < csvResult.Length; ++i)
                {
                    dataGrid.Rows.Add(csvResult[i]);
                }
            }
        }

        // TODO Здесь нужно смотреть не на селектед нод, а на датагрид.
        private void addButton_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if (selectedNode != null)
            {
                var pcv = new ProductCardView();
                pcv.ShowDialog();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure want to delete this row?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var selectedCells = dataGrid.SelectedCells;
                if (selectedCells != null && selectedCells.Count > 0)
                {
                    dataGrid.Rows.RemoveAt(selectedCells[0].RowIndex);
                }
            }
        }
    }
}
