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
        public ProductCardView()
        {
            InitializeComponent();
        }
        public ProductCardView(ProductRow productRow):this()
        {
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
    }
}
