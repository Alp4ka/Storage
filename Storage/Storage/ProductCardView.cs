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
    public partial class ProductCardView : Form
    {
        public ProductCardView()
        {
            InitializeComponent();
        }
        public ProductCardView(DataGridViewRow row):this()
        {
            
        }
    }
}
