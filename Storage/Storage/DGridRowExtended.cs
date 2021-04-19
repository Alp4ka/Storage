using Storage;

namespace System.Windows.Forms
{
    /// <summary>
    /// Немного более удобный Роу.
    /// </summary>
    public class ProductRow : DataGridViewRow
    {
        /// <summary>
        /// Продукт, привязанный к строке.
        /// </summary>
        public Product Product;
        public ProductRow() : base() { }
    }
}

