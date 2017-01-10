using LHCashier.DataAccess;
using LHCashier.EntityClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCashier.Business
{
    public class GoodsManager
    {
        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="keyWords">关键字(编码、条码、拼音码、名称)</param>
        /// <returns></returns>
        public static Goods GetGoods(string keyWords)
        {
            return GoodsService.GetGoods(keyWords);
        }
    }
}
