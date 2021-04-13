using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Storage
{
    public static class Utils
    {
        private const string _root = ".\\CATEGORIES";
        private const string _csvName = "products.csv";
        public static TreeNode FindNode(TreeNode treeNode, string name)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                if (tn.Text == name)
                {
                    return tn;
                }
            }
            return null;
        }
        public static string GetCsvByNode(TreeNode tnode)
        {
            return Path.Combine(Path.Combine(_root, tnode.FullPath), _csvName);
        }
        public static TreeNode[] InitializeCategories(string path=_root)
        {
            TreeView tv = new TreeView();
            List<TreeNode> nodes = new List<TreeNode>();
            int recursionDepth = 7;
            try
            {
                if (Directory.Exists(path))
                {
                    string[] directories = Directory.GetDirectories(path);
                    foreach(string d in directories)
                    {
                        string fileName = Path.GetFileName(d);
                        TreeNode tnode = new TreeNode(fileName);
                        tv.Nodes.Add(tnode);
                        nodes.Add(tnode);
                        TreeNodeByPath(tnode, recursionDepth-1);
                    }
                    tv.Nodes.Clear();
                    return nodes.ToArray();
                }
                else
                {
                    Directory.CreateDirectory(_root);
                    return new TreeNode[] { };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new TreeNode[] { };
            }
        }
        private static void TreeNodeByPath(TreeNode parent, int remain)
        {
            string parentPath = Path.Combine(_root, parent.FullPath);
            if (remain <= 0 || Directory.GetDirectories(parentPath).Length == 0)
            {
                return;
            }
            string[] directories = Directory.GetDirectories(parentPath);
            foreach (string d in directories)
            {
                TreeNode tnode = new TreeNode(Path.GetFileName(d));
                parent.Nodes.Add(tnode);
                TreeNodeByPath(tnode, remain - 1);
            }
        }
        public static TreeNode FindNode(TreeView treeView, string name)
        {
            foreach (TreeNode tn in treeView.Nodes)
            {
                if (tn.Text == name)
                {
                    return tn;
                }
            }
            return null;
        }
        public static void CreateCategoryInPath(TreeNode node)
        {
            try
            {
                string fullpath = Path.Combine(_root, node.FullPath);
                Directory.CreateDirectory(fullpath);
                string filename = Path.Combine(fullpath, "products.csv");
                File.WriteAllText(filename, String.Join(",", Program.CsvHeader.Select(x=>'"'+ x.ToString() + '"')));
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
            
            foreach(TreeNode tn in treeView.Nodes)
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
        private static void HightlightRecursive(TreeNode node, string text, Color highlighter, Color basic, int remain)
        {
            if(node.Nodes.Count == 0 || remain <= 0)
            {
                return;
            }
            foreach (TreeNode tn in node.Nodes)
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
        public static void RemoveCategoryFromPath(TreeNode node)
        {
            try
            {
                string fullpath = Path.Combine(_root, node.FullPath);
                Directory.Delete(fullpath, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void RenameCategoryTo(TreeNode node, string name)
        {
            try
            {
                string fullpath = Path.Combine(_root, node.FullPath);
                DirectoryInfo dinfo = new DirectoryInfo(fullpath);
                dinfo.RenameTo(name);
                node.Text = name;
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
