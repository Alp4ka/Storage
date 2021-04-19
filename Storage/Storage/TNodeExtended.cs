using Storage;

namespace System.Windows.Forms
{
    /// <summary>
    /// Немножечко более удобный тринод.
    /// </summary>
    public class StorageNode : TreeNode
    {
        /// <summary>
        /// Категория, привязанная к ноду.
        /// </summary>
        public Cathegory Cathegory { get; set; }
        public StorageNode() : base() { }
        public StorageNode(string text) : base(text) { }
    }
}