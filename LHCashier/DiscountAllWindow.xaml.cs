﻿using System;
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
    /// DiscountAllWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DiscountAllWindow : Window
    {
        public delegate void DiscountAll(double discountAll);

        private DiscountAll discountAll;

        public DiscountAllWindow(DiscountAll discountAll)
        {
            this.discountAll = discountAll;
            InitializeComponent();
            this.txtDiscount.Focus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtDiscount != null)
                {
                    double discount = 0.00;
                    double.TryParse(this.txtDiscount.Text.Trim(), out discount);
                    discountAll(discount);
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
