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
            foreach (string h in Program.CsvHeader)
            {
                dataGrid.Columns.Add(h, h);
            }
        }

        private void InitializeCategories()
        {
            StorageNode[] nodes = Utils.InitializeCategories();
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
            var selectedNode = (StorageNode)storageTree.SelectedNode;
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
                    Utils.RemoveCategoryFromPath((StorageNode)selectedNode);
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
                CategoryCreationView ccv = new CategoryCreationView((StorageNode)selectedNode.Parent);
                ccv.ShowDialog();
                if (ccv.Result != null)
                {
                    Utils.RenameCategoryTo((StorageNode)selectedNode, ccv.Result.Text);
                }
            }
            
        }

        private void storageTree_DoubleClick(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if(selectedNode!= null)
            {
                dataGrid.Rows.Clear();
                var products = ((StorageNode)selectedNode).Cathegory.Products;

                /*foreach (string h in Program.CsvHeader)
                {
                    dataGrid.Columns.Add(h, h);
                }*/
                for(int i = 0; i < products.Count(); ++i)
                {
                    var row = new ProductRow();
                    var array = products[i].GetArray();
                    row.CreateCells(dataGrid);
                    for (int j = 0; j < array.Length; ++j)
                    {
                        row.Cells[j].Value = array[j];
                    }
                    row.Product = products[i];
                    dataGrid.Rows.Add(row);

                }
            }
        }

        // TODO Здесь нужно смотреть не на селектед нод, а на датагрид.
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedNode = storageTree.SelectedNode;
                if (selectedNode != null)
                {
                    var pcv = new ProductCardView();
                    pcv.ShowDialog();
                    Product pr = pcv.Result;
                    if (pr == null)
                    {
                        return;
                    }
                    var array = pr.GetArray();
                    var row = new ProductRow();
                    row.Product = pr;
                    row.CreateCells(dataGrid);
                    for (int j = 0; j < array.Length; ++j)
                    {
                        row.Cells[j].Value = array[j];
                    }
                    dataGrid.Rows.Add(row);
                }
            }
            catch
            {

            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
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
            catch (InvalidOperationException)
            {
                MessageBox.Show("Choose correct row.");
            }
        }

        private void dataGrid_Enter(object sender, EventArgs e)
        {
            //editButton.Visible = true;
        }

        private void dataGrid_Leave(object sender, EventArgs e)
        {
            //editButton.Visible = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedNode = storageTree.SelectedNode;
                if (selectedNode != null)
                {
                    if (dataGrid.SelectedCells.Count == 0)
                    {
                        return;
                    }
                    var productRow = ((ProductRow)dataGrid.SelectedCells[0].OwningRow);
                    ProductCardView pcv = new ProductCardView(productRow);
                    pcv.ShowDialog();
                    if(pcv.Result != null)
                    {
                        object[] toSave = productRow.Product.GetArray();
                        for(int i=0; i < toSave.Length; ++i)
                        {
                            productRow.Cells[i].Value = toSave[i];
                        }
                        
                    }
                }
            }
            catch
            {
                MessageBox.Show("Choose correct row.");
            }
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if((sender as DataGridView).CurrentCell != null)
            {
                editButton.Enabled = true;
                deleteButton.Enabled = true;
            }
            else
            {
                editButton.Enabled = false;
                deleteButton.Enabled = false;
            }
        }

        private void categoryCreationStrip_Opening(object sender, CancelEventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if(selectedNode != null)
            {
                deleteSubCategoryToolStripMenuItem.Enabled = changeSubCategoryToolStripMenuItem.Enabled = true;
            }
            else
            {
                deleteSubCategoryToolStripMenuItem.Enabled = changeSubCategoryToolStripMenuItem.Enabled = false;
            }
        }
    }
}
