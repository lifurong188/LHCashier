using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LHCashier.EntityClass
{
    public class VIPCustomer:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string no;
        /// <summary>
        /// 会员编号(也就是会员卡号)
        /// </summary>
        public string No
        {
            set
            {
                no = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("No"));
                }
            }
            get
            {
                return no;
            }
        }
        private CustomerType customerType;  
        /// <summary>
        /// 会员类型
        /// </summary>
        public CustomerType CustomerType
        {
            set
            {
                customerType = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CustomerType"));
                }
            }
            get
            {
                return customerType;
            }
        }
        private string name; 
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set
            {
                name = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
            get
            {
                return name;
            }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int Score { get; set; }

        private int exchangeScore;  
        /// <summary>
        /// 可兑换积分
        /// </summary>
        public int ExchangeScore
        {
            set
            {
                exchangeScore = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ExchangeScore"));
                }
            }
            get
            {
                return exchangeScore;
            }
        }

        private double storedMoney; 
        /// <summary>
        /// 储值金额
        /// </summary>
        public double StoredMoney
        {
            set
            {
                storedMoney = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StoredMoney"));
                }
            }
            get
            {
                return storedMoney;
            }
        }


    }

    public class VIPCustomerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VIPCustomer vip = value as VIPCustomer;
            string property = parameter.ToString();
            switch (property)
            {
                case "No":
                    return vip.No;
                case "Name":
                    return vip.Name;
                case "Mobile":
                    return vip.Mobile;
            }
            if (property.Equals("ExchangeScore"))
            {
                if(vip.CustomerType == CustomerType.IntegralVIP || vip.CustomerType == CustomerType.IntegralStoredVIP)
                {
                    return vip.ExchangeScore;

                    
                }
                else
                {
                    return "-";
                }
            }
            if (property.Equals("StoredMoney"))
            {
                if(vip.CustomerType == CustomerType.StoredVIP || vip.CustomerType == CustomerType.IntegralStoredVIP)
                {
                    return vip.StoredMoney;
                }
                else
                {
                    return "-";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
