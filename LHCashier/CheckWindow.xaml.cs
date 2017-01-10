using LHCashier.EntityClass;
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
    /// CheckWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CheckWindow : Window
    {
        public delegate void AfterPayMoney();

        private AfterPayMoney afterPayMoney;
        TotalPurchase totalPurchase;
        public CheckWindow(TotalPurchase totalPurchase, AfterPayMoney afterPayMoney)
        {
            this.totalPurchase = totalPurchase;
            this.afterPayMoney = afterPayMoney;
            InitializeComponent();
            this.tbShouldMoney.Text = totalPurchase.TotalMoney.ToString();
            this.txtPaidMoney.Text = this.tbShouldMoney.Text;
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(()=>this.txtPaidMoney.Focus()));
            this.txtPaidMoney.SelectionStart = this.txtPaidMoney.Text.Length;
        }

        private void txtPaidMoney_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (this.tabPayMethod.SelectedIndex == 0)
            {
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Decimal ||
                    (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back ||
                    e.Key == Key.Left || e.Key == Key.Right)
                {
                    e.Handled = false;
                }
                else
                {

                    e.Handled = true;
                }
            }
        }

        private void btnPayMoney_Click(object sender, RoutedEventArgs e)
        {
            RMBPayMethod();
            if(afterPayMoney != null)
            {
                afterPayMoney();
            }
        }

        /// <summary>
        /// 现金付款
        /// </summary>
        private void RMBPayMethod()
        {
            double paidMoney = double.Parse(this.txtPaidMoney.Text);
            if (paidMoney < totalPurchase.TotalMoney)
            {
                this.txtPaidMoney.Focus();
                return;
            }
            double returnMoney = Math.Round(paidMoney - totalPurchase.TotalMoney, 2);
            this.tbReturnMoney.Text = returnMoney.ToString();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            Label label = sender as Label;
            int tabIndex = int.Parse(label.Tag.ToString());
            ChangePayMethod(tabIndex);
        }

        /// <summary>
        /// 更换付款方式
        /// </summary>
        /// <param name="sender"></param>
        private void ChangePayMethod(int tabItemIndex)
        {
            this.tabPayMethod.SelectedIndex = tabItemIndex;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    RMBPayMethod();
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.F1:
                    this.txtPaidMoney.Focus();
                    ChangePayMethod(0);
                    break;
                case Key.F2:
                    ChangePayMethod(1);
                    break;
                case Key.F3:
                    ChangePayMethod(2);
                    break;
                case Key.F4:
                    ChangePayMethod(3);
                    break;
                case Key.F5:
                    ChangePayMethod(4);
                    break;
                case Key.F6:
                    ChangePayMethod(5);
                    break;
                case Key.F7:
                    ChangePayMethod(6);
                    break;


            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
