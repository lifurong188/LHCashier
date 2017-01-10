using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCashier.EntityClass
{
    /// <summary>
    /// 商品销售状态
    /// </summary>
    public enum SaleState
    {
        /// <summary>
        /// 正常销售
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 促销销售
        /// </summary>
        Promotion = 1,
        /// <summary>
        /// 停止销售
        /// </summary>
        Stop = 2
    }

    
}
