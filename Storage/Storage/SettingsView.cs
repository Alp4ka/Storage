using System;
using System.Windows.Forms;

namespace Storage
{
    /// <summary>
    /// Вьюшка для настроек.
    /// </summary>
    public partial class SettingsView : Form
    {
        public SettingsView()
        {
            InitializeComponent();
            integerBox.Text = Storage.WarnProductAmount.ToString();
        }
        /// <summary>
        /// Событие нажатия на кнопку ок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            int temp;
            if (int.TryParse(integerBox.Text, out temp))
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
        /// <summary>
        /// Событие нажатия на кнопку неок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
