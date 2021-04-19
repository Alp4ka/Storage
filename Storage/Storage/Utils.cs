using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Storage
{
    public static class Utils
    {
        private static  string _root = ".\\CATEGORIES";
        public const int ArticleNumericLength = 9;
        private static string _csvName = "products.csv";
        public static string Root { get => _root; set { _root = value; } }
        public static string CsvName { get => _csvName;}
        public static StorageNode FindNode(StorageNode treeNode, string name)
        {
            foreach (StorageNode tn in treeNode.Nodes)
            {
                if (tn.Text == name)
                {
                    return tn;
                }
            }
            return null;
        }
        public static string GetCsvByNode(StorageNode tnode)
        {
            return Path.Combine(Path.Combine(Root, tnode.FullPath), CsvName);
        }
        public static StorageNode[] InitializeCategories(string path)
        {
            TreeView tempTreeView = new TreeView();
            List<StorageNode> nodes = new List<StorageNode>();
            int recursionDepth = 7;
            try
            {
                if (Directory.Exists(path))
                {
                    string[] directories = Directory.GetDirectories(path);
                    foreach(string d in directories)
                    {
                        string fileName = Path.GetFileName(d);
                        StorageNode tnode = new StorageNode(fileName);
                        tempTreeView.Nodes.Add(tnode);
                        nodes.Add(tnode);

                        string csvPath = Path.Combine(d, CsvName);
                        List<Product> productsToAdd = ParseProducts(csvPath);

                        Cathegory cathegoryToAdd = new Cathegory(tnode.Text);
                        cathegoryToAdd.Products.AddRange(productsToAdd);
                        Storage.Products.AddRange(productsToAdd);

                        tnode.Cathegory = cathegoryToAdd;
                        Storage.Cathegories.Add(cathegoryToAdd);
                        TreeNodeByPath(tnode, recursionDepth-1);
                    }
                    tempTreeView.Nodes.Clear();
                    return nodes.ToArray();
                }
                else
                {
                    Directory.CreateDirectory(Root);
                    return new StorageNode[] { };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new StorageNode[] { };
            }
        }
        private static List<Product> ParseProducts(string path)
        {
            List<Product> result = new List<Product>();
            var csvResult = SuperSmartCsvManager.ReadCsv(path);
            for(int i =1; i < csvResult.Length; ++i)
            {
                try
                {
                    Product product = Product.FromArray(csvResult[i]);
                    result.Add(product);
                }
                catch
                {

                }
            }
            return result;
        }
        private static void TreeNodeByPath(StorageNode parent, int remain)
        {
            string parentPath = Path.Combine(Root, parent.FullPath);
            if (remain <= 0 || Directory.GetDirectories(parentPath).Length == 0)
            {
                return;
            }
            string[] directories = Directory.GetDirectories(parentPath);
            foreach (string d in directories)
            {
                StorageNode tnode = new StorageNode(Path.GetFileName(d));

                Cathegory cathegoryToAdd = new Cathegory(tnode.Text);
                tnode.Cathegory = cathegoryToAdd;
                parent.Cathegory.Cathegories.Add(cathegoryToAdd);

                string csvPath = Path.Combine(d, CsvName);
                List<Product> productsToAdd = ParseProducts(csvPath);
                cathegoryToAdd.Products.AddRange(productsToAdd);
                Storage.Products.AddRange(productsToAdd);

                parent.Nodes.Add(tnode);
                TreeNodeByPath(tnode, remain - 1);
            }
        }
        public static StorageNode FindNode(TreeView treeView, string name)
        {
            foreach (StorageNode tn in treeView.Nodes)
            {
                if (tn.Text == name)
                {
                    return tn;
                }
            }
            return null;
        }
        public static void CreateCategoryInPath(StorageNode node)
        {
            try
            {
                string fullpath = Path.Combine(Root, node.FullPath);
                Directory.CreateDirectory(fullpath);
                string filename = Path.Combine(fullpath, "products.csv");
                File.WriteAllText(filename, String.Join(",", Program.CsvHeader.Select(x=>'"'+ x.ToString() + '"')));
                //Cathegory cathegory = new Cathegory(node.Text);
                //node.Cathegory = cathegory;
                //Storage.Cathegories.Add(cathegory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void HighlightNodesByContaining(TreeView treeView, string text)
        {
            Color highlighter;
            Color basic = Color.FromArgb(255, 255, 255);
            if (String.IsNullOrWhiteSpace(text))
            {
                highlighter = basic;
            }
            else
            {
                highlighter = Color.FromArgb(255, 255, 0);
            }
            
            foreach(StorageNode tn in treeView.Nodes)
            {
                if (tn.Text.ToLower().Contains(text.ToLower()))
                {
                    tn.ForeColor = highlighter;
                    if (highlighter != basic)
                    {
                        tn.Parent?.Expand();
                    }
                }
                else
                {
                    tn.ForeColor = basic;
                }
                HightlightRecursive(tn, text, highlighter, basic, 10);
            }
        }
        private static void HightlightRecursive(StorageNode node, string text, Color highlighter, Color basic, int remain)
        {
            if(node.Nodes.Count == 0 || remain <= 0)
            {
                return;
            }
            foreach (StorageNode tn in node.Nodes)
            {
                if (tn.Text.ToLower().Contains(text.ToLower()))
                {
                    tn.ForeColor = highlighter;
                    if(highlighter != basic)
                    {
                        tn.Parent?.Expand();
                    }
                }
                else
                {
                    tn.ForeColor = basic;
                }
                HightlightRecursive(tn, text, highlighter, basic,  remain-1);
            }
            return;
        }
        public static void RemoveCategoryFromPath(StorageNode node)
        {
            try
            {
                string fullpath = Path.Combine(Root, node.FullPath);
                Storage.Cathegories.Remove(node.Cathegory);
                Directory.Delete(fullpath, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void RenameCategoryTo(StorageNode node, string name)
        {
            try
            {
                string fullpath = Path.Combine(Root, node.FullPath);
                DirectoryInfo dinfo = new DirectoryInfo(fullpath);
                dinfo.RenameTo(name);
                node.Text = name;
                node.Cathegory.Name = name;
            }
            catch (IOException)
            {
                MessageBox.Show("You've opened this category in explorer. Please close it and try again.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void RenameTo(this DirectoryInfo directory, string name)
        {
            directory.MoveTo(Path.Combine(directory.Parent.FullName, name));
        }
    }
}
