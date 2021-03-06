using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Storage
{
    /// <summary>
    /// Вь.щка для карточки товара.
    /// </summary>
    public partial class ProductCardView : Form
    {
        private Product _result;
        /// <summary>
        /// Переменная, которая хранит в себе результат работы в окошке.
        /// </summary>
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

        /// <summary>
        /// Загрузить изображение.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Нажатие на кнопку канкел.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            _result = null;
            Close();
        }
        /// <summary>
        /// Проверить правильность введенных данных.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Нажатие на Add.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Смена текста в текстбоксе.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            string generatedArticle = HashStringUtils.CreateArticle(product, Utils.ArticleNumericLength);
            articleBox.Text = generatedArticle;
        }

    }

}