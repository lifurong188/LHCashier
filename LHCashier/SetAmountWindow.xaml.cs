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
using System.Windows.Shapes;

namespace LHCashier
{
    /// <summary>
    /// SetAmountWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetAmountWindow : Window
    {
        public delegate void SetAmount(double amount);

        private SetAmount setAmount;

        public SetAmountWindow(SetAmount setAmount)
        {
            this.setAmount = setAmount;
            InitializeComponent();
            this.txtAmount.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (setAmount != null)
                {
                    double amount = 0;
                    double.TryParse(this.txtAmount.Text.Trim(), out amount);
                    setAmount(amount);
                    this.Close();
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void txtAmount_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
               (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
               e.Key == Key.Decimal || e.Key == Key.Back)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
