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
    /// ChangePriceAllWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePriceAllWindow : Window
    {
        public delegate void PreferentialMoney(double money);

        private PreferentialMoney preferentialMoney;

        public ChangePriceAllWindow(PreferentialMoney preferentialMoney)
        {
            this.preferentialMoney = preferentialMoney;
            InitializeComponent();
            this.txtChangeMoney.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (preferentialMoney != null)
                {
                    double money = 0;
                    double.TryParse(this.txtChangeMoney.Text.Trim(), out money);
                    preferentialMoney(money);
                    this.Close();
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
