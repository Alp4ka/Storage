using System.Drawing;
using System.Windows.Forms;

namespace Storage
{
    /// <summary>
    /// Вьюшка.
    /// </summary>
    public partial class HelpView : Form
    {
        public HelpView()
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(".\\..\\Help.jpg");
        }
    }
}
