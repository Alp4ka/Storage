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
    public partial class SettingsView : Form
    {
        public SettingsView()
        {
            InitializeComponent();
            integerBox.Text = Storage.WarnProductAmount.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            int temp;
            if(int.TryParse(integerBox.Text, out temp))
            {
                if (temp >= 1)
                {
                    Storage.WarnProductAmount = temp;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Expected value >= 1, got: {temp}!");
                }
            }
            else
            {
                MessageBox.Show($"Expected integer, got: {integerBox.Text}!");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
