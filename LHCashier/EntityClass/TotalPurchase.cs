using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LHCashier.EntityClass
{
    public class TotalPurchase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 交易单号
        /// </summary>
        public string PurchaseNum
        {
            get { return (string)GetValue(PurchaseNumProperty); }
            set { SetValue(PurchaseNumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PurchaseNum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PurchaseNumProperty =
            DependencyProperty.Register("PurchaseNum", typeof(string), typeof(TotalPurchase));


        private DateTime purchaseTime;
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime PurchaseTime
        {
            set
            {
                purchaseTime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PurchaseTime"));
                }
            }
            get
            {
                return purchaseTime;
            }
        }

        private bool isRefund;
        /// <summary>
        /// 是否退货
        /// </summary>
        public bool IsRefund
        {
            set
            {
                isRefund = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRefund"));
                }
            }
            get
            {
                return isRefund;
            }
        }

        /// <summary>
        /// 顾客类型
        /// </summary>
        public CustomerType CustomerType
        {
            get { return (CustomerType)GetValue(CustomerTypeProperty); }
            set { SetValue(CustomerTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomerType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerTypeProperty =
            DependencyProperty.Register("CustomerType", typeof(CustomerType), typeof(TotalPurchase));


        /// <summary>
        /// 会员信息
        /// </summary>
        public VIPCustomer VIPCustomer
        {
            get { return (VIPCustomer)GetValue(VIPCustomerProperty); }
            set { SetValue(VIPCustomerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VIPCustomer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VIPCustomerProperty =
            DependencyProperty.Register("VIPCustomer", typeof(VIPCustomer), typeof(TotalPurchase));

        //private VIPCustomer vipCustomer;
        ////
        //public VIPCustomer VIPCustomer
        //{
        //    set
        //    {
        //        vipCustomer = value;
        //        if(PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("VIPCustomer"));
        //        }
        //    }
        //    get
        //    {
        //        return vipCustomer;
        //    }
        //}



        private ObservableCollection<Purchase> purchaseList;
        /// <summary>
        /// 本次交易商品列表
        /// </summary>
        public ObservableCollection<Purchase> PurchaseList
        {
            set
            {
                purchaseList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PurchaseList"));
                }
            }
            get
            {
                return purchaseList;
            }
        }

        /// <summary>
        /// 总交易件数
        /// </summary>
        public double TotalAmount
        {
            get { return (double)GetValue(TotalAmountProperty); }
            set { SetValue(TotalAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalAmountProperty =
            DependencyProperty.Register("TotalAmount", typeof(double), typeof(TotalPurchase), new PropertyMetadata(0.00));

        private string cashierNo;
        /// <summary>
        /// 收银员编号
        /// </summary>
        public string CashierNo
        {
            set
            {
                cashierNo = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CashierNo"));
                }
            }
            get
            {
                return cashierNo;
            }
        }


        /// <summary>
        /// 总金额
        /// </summary>
        public double TotalMoney
        {
            get { return Math.Round((double)GetValue(TotalMoneyProperty),2); }
            set { SetValue(TotalMoneyProperty, value); }
        }
    
        // Using a DependencyProperty as the backing store for TotalMoney.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalMoneyProperty =
            DependencyProperty.Register("TotalMoney", typeof(double), typeof(TotalPurchase), new PropertyMetadata(0.00));



        /// <summary>
        /// 总优惠
        /// </summary>
        public double TotalPreferential
        {
            get { return Math.Round((double)GetValue(TotalPreferentialProperty),2); }
            set { SetValue(TotalPreferentialProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalPreferential.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalPreferentialProperty =
            DependencyProperty.Register("TotalPreferential", typeof(double), typeof(TotalPurchase), new PropertyMetadata(0.00));


        private double totalDiscount;
        /// <summary>
        /// 总折扣
        /// </summary>
        public double TotalDiscount
        {
            set
            {
                totalDiscount = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalDiscount"));
                }
            }
            get
            {
                return Math.Round(totalDiscount,2);
            }
        }

        private double totalRebateMoney;
        /// <summary>
        /// 总参与返利的金额
        /// </summary>
        public double TotalRebateMoney
        {
            set
            {
                totalRebateMoney = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalRebateMoney"));
                }
            }
            get
            {
                return totalRebateMoney;
            }
        }
    }

    public class TotalPurchaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isRefund = bool.Parse(value.ToString());
            if (isRefund)
            {
                return "退货单号：";
            }
            else
            {
                return "销售单号：";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
