using LHCashier.EntityClass;
using System.Data.SqlClient;
using System.Data;
using System;

namespace LHCashier.DataAccess
{
    public class GoodsService
    {
        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="keyWords">关键字(编码、条码、拼音码、名称)</param>
        /// <returns></returns>
        public static Goods GetGoods(string keyWords)
        {
            Goods goods = null;
            using(SqlConnection conn = SQLHelper.GetConnection())
            {
                conn.Open();
                using(SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;  //使用存储过程
                    command.CommandText = "proc_GetGoods";
                    SqlParameter[] param = new SqlParameter[] { new SqlParameter("@keyWords", keyWords) };
                    command.Parameters.AddRange(param);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        goods = new Goods();
                        goods.PluCode = Convert.ToString(reader["PluCode"]);
                        goods.BarCode = Convert.ToString(reader["BarCode"]);
                        goods.PluName = Convert.ToString(reader["PluName"]);
                        goods.PluMN = Convert.ToString(reader["PluMN"]);
                        goods.ClsCode = Convert.ToString(reader["ClsCode"]);
                        goods.Brand = Convert.ToString(reader["Brand"]);
                        goods.Unit = Convert.ToString(reader["Unit"]);
                        goods.VendorCode = Convert.ToString(reader["VendorCode"]);
                        goods.Price = Convert.ToDouble(reader["Price"]);
                        goods.MinPrice = Convert.ToDouble(reader["MinPrice"]);
                        goods.VIPPrice = Convert.ToDouble(reader["VIPPrice"]);
                        goods.PromotionPrice = Convert.ToDouble(reader["PromotionPrice"]);
                        //goods.PromotionType =  (PromotionType)reader["PromotionType"];
                        goods.CanDiscount = Convert.ToBoolean(reader["CanDiscount"]);
                        goods.CanChangePrice = Convert.ToBoolean(reader["CanChangePrice"]);
                        goods.CanSend = Convert.ToBoolean(reader["CanSend"]);
                        goods.DiscountRate = Convert.ToDouble(reader["DiscountRate"]);
                        goods.LastInputDate = Convert.ToDateTime(reader["LastInputDate"].GetType() == typeof(DBNull)? DateTime.MinValue: reader["LastInputDate"]);
                        goods.LastSaleDate = Convert.ToDateTime(reader["LastSaleDate"].GetType() == typeof(DBNull) ? DateTime.MinValue : reader["LastSaleDate"]);
                        goods.SaleState = (SaleState)reader["SaleState"];
                        goods.SalerNo = Convert.ToString(reader["SalerNo"]);
                        goods.Remark = Convert.ToString(reader["Remark"]);

                    }

                    return goods;
                }
            }
        }
    }
}
