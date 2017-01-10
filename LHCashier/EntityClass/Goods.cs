using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LHCashier.EntityClass
{
    public class Goods: DependencyObject, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        

        /// <summary>
        /// 编码
        /// </summary>
        public string PluCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PluName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PluMN { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepCode { get; set; }
        /// <summary>
        /// 品类编码
        /// </summary>
        public string ClsCode { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string VendorCode { get; set; }
        private double price;
        /// <summary>
        /// 售价
        /// </summary>
        public double Price
        {
            set
            {
                price = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Price"));
                }
            }
            get
            {
                return price;
            }
        }
        /// <summary>
        /// 最低售价
        /// </summary>
        public double MinPrice { get; set; }

        private double promotionPrice;
        /// <summary>
        /// 促销价格
        /// </summary>
        public double PromotionPrice
        {
            set
            {
                promotionPrice = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PromotionPrice"));
                }
            }
            get
            {
                return promotionPrice;
            }
        }

        private PromotionType promotionType;
        /// <summary>
        /// 促销类型
        /// </summary>
        public PromotionType PromotionType
        {
            set
            {
                promotionType = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PromotionType"));
                }
            }
            get
            {
                return promotionType;
            }
        }

        
        private double vipPrice;
        /// <summary>
        /// 会员价格
        /// </summary>
        public double VIPPrice
        {
            set
            {
                vipPrice = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("VIPPrice"));
                }
            }
            get
            {
                return vipPrice;
            }
        }
        /// <summary>
        /// 是否允许打折
        /// </summary>
        public bool CanDiscount { get; set; }
        /// <summary>
        /// 是否允许改价
        /// </summary>
        public bool CanChangePrice { get; set; }
        /// <summary>
        /// 是否允许赠送
        /// </summary>
        public bool CanSend { get; set; }


        private double discountRate = 1.00;
        /// <summary>
        /// 折扣
        /// </summary>
        public double DiscountRate
        {
            set
            {
                discountRate = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DiscountRate"));
                }
            }
            get
            {
                return discountRate;
            }
        }
        /// <summary>
        /// 商品销售状态
        /// </summary>
        public SaleState SaleState { get; set; }
        /// <summary>
        /// 最新进货日期
        /// </summary>
        public DateTime LastInputDate { get; set; }
        /// <summary>
        /// 最新销售日期
        /// </summary>
        public DateTime LastSaleDate { get; set; }
        /// <summary>
        /// 营业员编号
        /// </summary>
        public string SalerNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }

    
    public class GoodsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Goods goods = value as Goods;
            string property = parameter.ToString();
            switch(property)
            {
                case "BarCode":
                    return goods.BarCode;
                case "PluCode":
                    return goods.PluCode;
                case "PluName":
                    return goods.PluName;
                case "Unit":
                    return goods.Unit;
                case "Price":
                    return goods.Price;
                case "PromotionPrice":
                    if(goods.PromotionPrice > 0)
                    {
                        return goods.PromotionPrice;
                    }
                    return "-";
                case "VIPPrice":
                    return goods.VIPPrice;
                case "Discount":
                    return goods.DiscountRate;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
