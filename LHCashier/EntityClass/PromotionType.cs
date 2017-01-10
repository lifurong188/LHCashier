using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCashier.EntityClass
{
    /// <summary>
    /// 促销类型
    /// </summary>
    public enum PromotionType
    {
        /// <summary>
        /// 低价销售
        /// </summary>
        LowPrice = 1,
        /// <summary>
        /// 买几送几(买自己送自己)
        /// </summary>
        BuySelfSendSelf = 2,
        /// <summary>
        /// 买几送几(买自己送别人)
        /// </summary>
        BuySelfSendOther = 3,
        /// <summary>
        /// 打折销售
        /// </summary>
        Discount = 4,
        /// <summary>
        /// 第二件半价
        /// </summary>
        SecondHalfPrice = 5,
        /// <summary>
        /// 套餐销售
        /// </summary>
        Suit = 6,
        /// <summary>
        /// 几元买几个
        /// </summary>
        PayForSome = 7


    }
}
