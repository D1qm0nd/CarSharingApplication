using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarSharingApplication.Windows.XAMLModels
{
    /// <summary>
    /// Логика взаимодействия для CreditCard.xaml
    /// </summary>
    public partial class CreditCard : UserControl
    {
        public CreditCard()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (((byte)e.Key) < 34 || ((byte)e.Key) > 43)
            { 
                if (tb.Text.Length > 0)
                {
                    tb.Text = tb.Text.Remove(tb.Text.Length - 1, 1);
                }
            } else
            if ((tb.Text.Length+1) % 5 == 0 && tb.Text.Length < 19)
            {
                tb.Text = tb.Text+' ';
            }
            tb.CaretIndex = tb.Text.Length;
        }

        private void DateTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (((byte)e.Key) < 34 || ((byte)e.Key) > 43)
            {
                if (tb.Text.Length > 0)
                {
                    tb.Text = tb.Text.Remove(tb.Text.Length - 1, 1);
                }
            }
            else
            if ((tb.Text.Length + 1) % 3 == 0 && tb.Text.Length<5)
            {
                tb.Text = tb.Text + '/';
            }
            tb.CaretIndex = tb.Text.Length;
        }

        public bool isEmpty()
        {
            return (CardNumber.Text == "") || (CardDate.Text == "") || (CVS.Password == "");
        }
    }
}
