using LHCashier.EntityClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LHCashier.EntityClass
{
    public class Purchase: DependencyObject,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Purchase() { }
        public Purchase(int no,Goods goods)
        {
            this.No = no;
            //数量为1
            this.Amount = 1;
            //赠送数量为1
            this.SendAmount = 0;
            //优惠金额为0
            this.Preferential = 0.00;

            this.Goods = goods;
            this.TotalMoney = Math.Round((this.Goods.Price * this.Amount - this.Preferential) * this.Goods.DiscountRate, 2);
        }

        /// <summary>
        /// 销售序号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 交易的商品
        /// </summary>
        private Goods goods;
        public Goods Goods
        {
            set
            {
                goods = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Goods"));
                }
            }
            get
            {
                return goods;
            }
        }

        private double amount;
        /// <summary>
        /// 数量
        /// </summary>
        public double Amount
        {
            set
            {
                amount = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
                }
            }
            get
            {
                return Math.Round(amount,2);
            }
        }

        private double sendAmount;
        /// <summary>
        /// 赠送数量
        /// </summary>
        public double SendAmount
        {
            set
            {
                sendAmount = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SendAmount"));
                }
            }
            get
            {
                return Math.Round(sendAmount,2);
            }
        }
        private double preferential;
        /// <summary>
        /// 单项优惠金额
        /// </summary>
        public double Preferential
        {
            set
            {
                preferential = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Preferential"));
                }
            }
            get
            {
                return Math.Round(preferential,2);
            }
        }
   
        private double totalMoney;
        /// <summary>
        /// 单项金额小计
        /// </summary>
        public double TotalMoney
        {
            set
            {
                totalMoney = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalMoney"));
                }
            }
            get
            {
                return Math.Round(totalMoney,2);
            }
        }
        private string saler;
        /// <summary>
        /// 售货员
        /// </summary>
        public string Saler
        {
            set
            {
                saler = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Saler"));
                }
            }
            get
            {
                return saler;
            }
        }



    }
}
