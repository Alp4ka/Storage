using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Storage
{
    public partial class ProductCardView : Form
    {
        private Product _result;
        public Product Result { get => _result; }
        private Product _toChange = null;
        public ProductCardView()
        {
            InitializeComponent();
            _result = null;
        }

        public ProductCardView(ProductRow productRow) : this()
        {
            _toChange = productRow.Product;
            var product = productRow.Product;
            nameBox.Text = product.Name != null ? product.Name : "";
            descriptionBox.Text = product.Description != null ? product.Description : "";
            price1Box.Text = product.Price1.ToString();
            price2Box.Text = product.Price2.ToString();
            amountBox.Text = product.Amount.ToString();
            articleBox.Text = product.Article != null ? product.Article : "";
            guaranteeBox.Text = product.Guarantee != null ? product.Guarantee : "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            var ext = Path.GetExtension(filename);
            try
            {
                if (new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }.Contains(ext))
                {
                    pictureBox1.Image = Image.FromFile(filename);
                }
            }
            catch
            {
                
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _result = null;
            Close();
        }
        private bool CheckFields()
        {
            if (nameBox.Text.Length < 3)
            {
                MessageBox.Show("Minimal length of Name is 3!");
                return false;
            }
            else if (!double.TryParse(price1Box.Text, out double tempPrice))
            {
                MessageBox.Show($"Strange Price1 *hm*... Should be real number, but found {price1Box.Text}");
                return false;
            }
            else if (!double.TryParse(price2Box.Text, out tempPrice))
            {
                MessageBox.Show($"Strange Price2 *hm*... Should be real number, but found {price2Box.Text}");
                return false;
            }
            else if (!int.TryParse(amountBox.Text, out int tempAmount))
            {
                MessageBox.Show($"Strange Amount *hm*... Should be Int, but found {amountBox.Text}");
                return false;
            }
            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!CheckFields())
            {
                return;
            }
            int amount = int.Parse(amountBox.Text);
            double price1 = double.Parse(price1Box.Text);
            double price2 = double.Parse(price2Box.Text);
            string name = nameBox.Text;
            string description = descriptionBox.Text;
            string guarantee = guaranteeBox.Text;
            string article = articleBox.Text;
            if (_toChange != null)
            {
                _toChange.Amount = amount;
                _toChange.Price1 = price1;
                _toChange.Price2 = price2;
                _toChange.Name = name;
                _toChange.Description = description;
                _toChange.Article = article;
                _toChange.Guarantee = guarantee;
                _result = _toChange;
            }
            else
            {
                Product product = new Product(
                    name: name,
                    price1: price1,
                    price2: price2,
                    article: article,
                    amount: amount,
                    description: description,
                    guarantee: guarantee
                    );
                _result = product;
            }
            Close();
        }

        private void textChanged(object sender, EventArgs e)
        {
            string name = nameBox.Text;
            string description = descriptionBox.Text;
            string guarantee = guaranteeBox.Text;
            Product product = new Product(
                    name: name,
                    description: description,
                    guarantee: guarantee
                    );
            int seed = product.GetHashCode();
            Random rnd = new Random(seed);
            var hash = (product.GetHashCode() + rnd.Next()).GetHashCode();
            hash *= hash;
            File.WriteAllText("hash_check.txt", hash.ToString());
        }
    }
}