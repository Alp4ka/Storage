using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Storage
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Вьюшка для главной формы.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            InitializeCategories();
            InitializeHeader(Program.CsvHeader);
            pathLabel.Text = Utils.Root;
        }
        /// <summary>
        /// Подсветить продукты по запросу.
        /// </summary>
        /// <param name="article"></param>
        private void HighlightProducts(string article)
        {
            Color toColor;
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                var prow = (ProductRow)row;
                if (prow.Product.Article.Replace("-", "").StartsWith(article.Replace("-", "")) && !String.IsNullOrWhiteSpace(article.Replace("-", "")))
                {
                    toColor = Color.FromArgb(255, 255, 0);
                }
                else
                {
                    toColor = Color.FromArgb(255, 255, 255);
                }
                foreach (DataGridViewCell cell in row.Cells)
                {
                    var style = new DataGridViewCellStyle(cell.Style);
                    style.ForeColor = toColor;
                    cell.Style = style;
                }
            }
        }
        /// <summary>
        /// Проинициализирвать заголовки.
        /// </summary>
        /// <param name="header"></param>
        private void InitializeHeader(string[] header)
        {
            dataGrid.Columns.Clear();
            foreach (string h in header)
            {
                dataGrid.Columns.Add(h, h);
            }
        }

        /// <summary>
        /// Проинициализировать категории.
        /// </summary>
        private void InitializeCategories()
        {
            StorageNode[] nodes = Utils.InitializeCategories(Utils.Root);
            storageTree.Nodes.Clear();
            dataGrid.Rows.Clear();
            foreach (var node in nodes)
            {
                storageTree.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Создать новую категорию(клик).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewCategory_Click(object sender, EventArgs e)
        {
            var selectedNode = (StorageNode)storageTree.SelectedNode;
            var ccvDialog = new CategoryCreationView(selectedNode);
            ccvDialog.ShowDialog();
            var result = ccvDialog.Result;
            if (result != null)
            {
                Cathegory cathegory = new Cathegory(result.Text);
                result.Cathegory = cathegory;
                if (selectedNode != null)
                {
                    selectedNode.Cathegory.Cathegories.Add(cathegory);
                    selectedNode.Nodes.Add(result);
                }
                else
                {
                    Storage.Cathegories.Add(cathegory);
                    storageTree.Nodes.Add(result);
                }
                Utils.CreateCategoryInPath(result);
            }
        }

        /// <summary>
        /// Удалить категорию(клик).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if (selectedNode != null)
            {
                DialogResult result = MessageBox.Show($"Are you sure want to delete {selectedNode.Text}?", "Deletion", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Utils.RemoveCategoryFromPath((StorageNode)selectedNode);
                    var parent = selectedNode.Parent;
                    var subcathegory = ((StorageNode)selectedNode).Cathegory;
                    if (parent != null)
                    {

                        ((StorageNode)parent).Cathegory.Cathegories.Remove(subcathegory);
                        parent.Nodes.Remove(selectedNode);
                    }
                    else
                    {
                        Storage.Cathegories.Remove(subcathegory);
                        storageTree.Nodes.Remove(selectedNode);
                    }
                    Storage.RemoveCathegory(subcathegory);
                }
            }
        }

        /// <summary>
        /// Ого, текст сменился.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Utils.HighlightNodesByContaining(storageTree, textBox1.Text);
        }

        /// <summary>
        /// Сменить имя категории.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeSubCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if (selectedNode != null)
            {
                CategoryCreationView ccv = new CategoryCreationView((StorageNode)selectedNode.Parent);
                ccv.ShowDialog();
                if (ccv.Result != null)
                {
                    Utils.RenameCategoryTo((StorageNode)selectedNode, ccv.Result.Text);
                }
            }

        }

        /// <summary>
        /// Дабл клик по тривью.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void storageTree_DoubleClick(object sender, EventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if (selectedNode != null)
            {
                dataGrid.Rows.Clear();
                var products = ((StorageNode)selectedNode).Cathegory.Products;

                for (int i = 0; i < products.Count(); ++i)
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

        /// <summary>
        /// Нажатие на кнопку добавить.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    Storage.Products.Add(pr);
                    ((StorageNode)selectedNode).Cathegory.Products.Add(pr);
                    SuperSmartCsvManager.WriteToCsv((StorageNode)selectedNode);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Удаление(нажатие на кнопку).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            var selectedNode = (StorageNode)storageTree.SelectedNode;
            if (selectedNode != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure want to delete this row?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var selectedCells = dataGrid.SelectedCells;
                        if (selectedCells != null && selectedCells.Count > 0)
                        {
                            var productRow = (ProductRow)selectedCells[0].OwningRow;
                            Storage.Products.Remove(productRow.Product);
                            selectedNode.Cathegory.Products.Remove(productRow.Product);
                            dataGrid.Rows.RemoveAt(selectedCells[0].RowIndex);
                            SuperSmartCsvManager.WriteToCsv((StorageNode)selectedNode);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Choose correct row.");
                }
            }
            else
            {
                MessageBox.Show("Choose cathegory.");
            }
        }


        /// <summary>
        /// Редактировать товар(нажатие на кнопку).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    if (pcv.Result != null)
                    {
                        object[] toSave = productRow.Product.GetArray();
                        for (int i = 0; i < toSave.Length; ++i)
                        {
                            productRow.Cells[i].Value = toSave[i];
                        }
                        SuperSmartCsvManager.WriteToCsv((StorageNode)selectedNode);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Choose correct row.");
            }
        }

        /// <summary>
        /// Сменилос выделение, обрабатываем событие.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if ((sender as DataGridView).CurrentCell != null)
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

        /// <summary>
        /// Создать новую категорию(клик).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoryCreationStrip_Opening(object sender, CancelEventArgs e)
        {
            var selectedNode = storageTree.SelectedNode;
            if (selectedNode != null)
            {
                deleteSubCategoryToolStripMenuItem.Enabled = changeSubCategoryToolStripMenuItem.Enabled = true;
            }
            else
            {
                deleteSubCategoryToolStripMenuItem.Enabled = changeSubCategoryToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Открыть новый склад(или проиницилизировать)(((клик))).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string path = folderBrowserDialog1.SelectedPath;
            try
            {
                Utils.Root = path;
                InitializeCategories();
                InitializeHeader(Program.CsvHeader);
                pathLabel.Text = Utils.Root;
            }
            catch
            {
                MessageBox.Show("Wrong Storage root!");
            }
        }

        /// <summary>
        /// Вызываем вьюшку настроек InNeed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsView sview = new SettingsView();
            sview.ShowDialog();
        }

        /// <summary>
        /// Создать репорт о товарах, которые скоро закончатся.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".csv";
            string date = DateTime.Now.ToString().Replace(" ", "_").Replace(":", ".");
            saveFileDialog1.FileName = $"Products_in_need_{date}";
            saveFileDialog1.Filter = "CSV File | .csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string path = saveFileDialog1.FileName;
            var productsInNeed = Storage.GetProductsInWarn();
            var content = Storage.GetCsvFromList(productsInNeed);
            try
            {
                File.WriteAllLines(path, content);
            }
            catch
            {
                MessageBox.Show("Something went wrong:c");
            }
        }

        /// <summary>
        /// Поиск по артикулу.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void articleSearchBox_TextChanged(object sender, EventArgs e)
        {
            HighlightProducts(articleSearchBox.Text);
        }
        private void orderCathegoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ProductRow> rows = new List<ProductRow>();
            foreach (ProductRow row in dataGrid.Rows)
            {
                rows.Add(row);
            }
            rows = rows.OrderBy(x => x.Product.Name).ThenBy(x => x.Product.Article).ToList();
            dataGrid.Rows.Clear();
            foreach (ProductRow row in rows)
            {
                dataGrid.Rows.Add(row);
            }
        }
        /// <summary>
        /// Нажатие на хелп.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var help = new HelpView();
            help.ShowDialog();
        }
    }
}
