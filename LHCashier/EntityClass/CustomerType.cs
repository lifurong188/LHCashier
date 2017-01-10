using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LHCashier.EntityClass
{
    /// <summary>
    /// 顾客类型
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// 零客
        /// </summary>
        Common = 1,            
        /// <summary>
        /// 积分型会员
        /// </summary>
        IntegralVIP = 2,
        /// <summary>
        /// 储值型会员
        /// </summary>
        StoredVIP = 3,
        /// <summary>
        /// 积分储值型会员
        /// </summary>
        IntegralStoredVIP = 4   
    }

    public class CustomerTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value.ToString();
            switch (type)
            {
                case "Common":
                    return "零客";
                case "IntegralVIP":
                    return "积分型会员";
                case "StoredVIP":
                    return "储值型会员";
                case "IntegralStoredVIP":
                    return "积分储值型会员";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
