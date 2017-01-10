using LHCashier.EntityClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// NoPayPurchaseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NoPayPurchaseWindow : Window
    {
        public delegate void ExtractTotalPurchase(TotalPurchase totalPurse);

        private ExtractTotalPurchase extractTotalPurchase;

        ObservableCollection<TotalPurchase> noPayTotalPurchaseList;
        public NoPayPurchaseWindow(ObservableCollection<TotalPurchase> noPayTotalPurchaseList, ExtractTotalPurchase extractTotalPurchase)
        {
            this.noPayTotalPurchaseList = noPayTotalPurchaseList;
            this.extractTotalPurchase = extractTotalPurchase;
            InitializeComponent();
            this.lbPurchaseNumList.ItemsSource = noPayTotalPurchaseList;
            this.lbPurchaseNumList.SelectedIndex = 0;
        }

        /// <summary>
        /// 当选择一个挂单交易单号时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbPurchaseNumList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbPurchaseNumList.SelectedItem != null)
            {
                TotalPurchase selectTotalPurchase = lbPurchaseNumList.SelectedItem as TotalPurchase;
                ObservableCollection<Purchase> purchaseList = selectTotalPurchase.PurchaseList;
                this.lvPurchaseDetail.ItemsSource = purchaseList;
            }else
            {
                this.lvPurchaseDetail.ItemsSource = null;
            }
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// "删除一个挂单交易"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelPurchase_Click(object sender, RoutedEventArgs e)
        {
            DeleteNoPayTotalPurchase();

        }

        /// <summary>
        /// "删除一个挂单交易"方法
        /// </summary>
        private void DeleteNoPayTotalPurchase()
        {
            if (lbPurchaseNumList.SelectedItem != null)
            {
                TotalPurchase selectTotalPurchase = lbPurchaseNumList.SelectedItem as TotalPurchase;
                noPayTotalPurchaseList.Remove(selectTotalPurchase);
            }
            if (noPayTotalPurchaseList.Count == 0)
            {
                this.Close();
            }
        }

        /// <summary>
        /// "提取一个挂单交易"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExtractPurchase_Click(object sender, RoutedEventArgs e)
        {
            ExtractPurchase();
        }

        /// <summary>
        /// "提取一个挂单交易"方法
        /// </summary>
        private void ExtractPurchase()
        {
            if (extractTotalPurchase != null)
            {
                TotalPurchase selectTotalPurchase = lbPurchaseNumList.SelectedItem as TotalPurchase;
                extractTotalPurchase(selectTotalPurchase);
                noPayTotalPurchaseList.Remove(selectTotalPurchase);
                this.Close();
            }
        }

        /// <summary>
        /// 全局快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //提取
                case Key.Enter:
                    ExtractPurchase();
                    break;
                //删除
                case Key.F4:
                    DeleteNoPayTotalPurchase();
                    break;
                //关闭
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
