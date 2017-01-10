using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LHCashier.EntityClass;
using LHCashier.Business;
using System.Collections.Generic;

namespace LHCashier
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<Purchase> purchaseList = new ObservableCollection<Purchase>();       //本次交易购物清单
        ObservableCollection<TotalPurchase> noPayTotalPurchaseList = new ObservableCollection<TotalPurchase>();   //未付款交易集合
        List<Purchase> purchaseList;                                                              //本次交易购物清单

        Thread timeThread;                                                                        //更新时间线程
        TotalPurchase totalPurchase;                                                              //本次交易的总交易对象，包含总金额、总优惠、销售单号等信息

        public MainWindow()
        {
            InitializeComponent();

            totalPurchase = FindResource("totalPurchase") as TotalPurchase;
            CreateTotalPurchase();  //创建一个交易
            
            //绑定此单交易的交易数据
            Goods goods1 = new Goods() { PluCode="01020202", BarCode = "6965413225698", PluName = "白象老坛酸菜牛肉面", Price = 1.5,  Unit = "包",CanChangePrice=true,CanSend=true,CanDiscount=true };
            goods1.PropertyChanged += Goods_PropertyChanged;
            Purchase purchase1 = new Purchase() { No = 1,Goods = goods1, Amount = 2, Saler = "1002" };
            //purchase1.TotalMoney = Math.Round((purchase1.Goods.Price * purchase1.Amount - purchase1.Preferential) * purchase1.Goods.DiscountRate, 2);
            //purchase1.Preferential = purchase1.SendAmount * purchase1.Goods.Price +
            //              (purchase1.Amount * purchase1.Goods.Price - purchase1.SendAmount * purchase1.Goods.Price) * (1 - purchase1.Goods.DiscountRate);
            //totalPurchase.TotalMoney += purchase1.TotalMoney;
            //totalPurchase.TotalPreferential += purchase1.Preferential;
            //totalPurchase.TotalAmount += purchase1.Amount;
            this.CalcTotalPurchase(purchase1);

            Goods goods2 = new Goods() {PluCode = "02040577", BarCode = "6925360232558", PluName = "康师傅茉莉蜜茶500ml", Price = 3.0, DiscountRate = 0.85, Unit = "瓶", CanChangePrice = true, CanSend = true, CanDiscount = true };
            goods2.PropertyChanged += Goods_PropertyChanged;
            Purchase purchase2 = new Purchase() { No = 2,Goods=goods2, Amount = 3, Preferential = 0, Saler = "1004" };
            //purchase2.TotalMoney = Math.Round((purchase2.Goods.Price * purchase2.Amount - purchase2.Preferential) * purchase2.Goods.DiscountRate, 2);
            //purchase2.Preferential = purchase2.SendAmount * purchase2.Goods.Price +
            //              (purchase2.Amount * purchase2.Goods.Price - purchase2.SendAmount * purchase2.Goods.Price) * (1 - purchase2.Goods.DiscountRate);
            //totalPurchase.TotalMoney += purchase2.TotalMoney;
            //totalPurchase.TotalPreferential += purchase2.Preferential;
            //totalPurchase.TotalAmount += purchase2.Amount;
            this.CalcTotalPurchase(purchase2);

            Goods goods3 = new Goods() {PluCode = "03050424", BarCode = "6955246755890", PluName = "伊利百利包【件】", Price = 36,  Unit = "提", CanChangePrice = true, CanSend = true, CanDiscount = true };
            goods3.PropertyChanged += Goods_PropertyChanged;
            Purchase purchase3 = new Purchase() { No = 3,Goods = goods3, Amount = 2, Saler = "1006" };
            //purchase3.TotalMoney = Math.Round((purchase3.Goods.Price * purchase3.Amount - purchase3.Preferential) * purchase3.Goods.DiscountRate, 2);
            //purchase3.Preferential = purchase3.SendAmount * purchase3.Goods.Price +
            //              (purchase3.Amount * purchase3.Goods.Price - purchase3.SendAmount * purchase3.Goods.Price) * (1 - purchase3.Goods.DiscountRate);
            //totalPurchase.TotalMoney += purchase3.TotalMoney;
            //totalPurchase.TotalPreferential += purchase3.Preferential;
            //totalPurchase.TotalAmount += purchase3.Amount;
            this.CalcTotalPurchase(purchase3);

            Goods goods4 = new Goods() {PluCode = "11040523", BarCode = "6944212333690", PluName = "牛栏山二锅头【壶】", Price = 12.5,  Unit = "壶", CanChangePrice = true, CanSend = true, CanDiscount = true };
            goods4.PropertyChanged += Goods_PropertyChanged;
            Purchase purchase4 = new Purchase() { No = 4,Goods=goods4, Amount = 4, Saler = "1004" };
            //purchase4.TotalMoney = Math.Round((purchase4.Goods.Price * purchase4.Amount - purchase4.Preferential) * purchase4.Goods.DiscountRate, 2);
            //purchase4.Preferential = purchase4.SendAmount * purchase4.Goods.Price +
            //              (purchase4.Amount * purchase4.Goods.Price - purchase4.SendAmount * purchase4.Goods.Price) * (1 - purchase4.Goods.DiscountRate);
            //totalPurchase.TotalMoney += purchase4.TotalMoney;
            //totalPurchase.TotalPreferential += purchase4.Preferential;
            //totalPurchase.TotalAmount += purchase4.Amount;
            this.CalcTotalPurchase(purchase4);

            totalPurchase.PurchaseList.Add(purchase1);
            totalPurchase.PurchaseList.Add(purchase2);
            totalPurchase.PurchaseList.Add(purchase3);
            totalPurchase.PurchaseList.Add(purchase4);


            //条码窗口默认获取焦点
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(()=>this.txtPurchase.Focus()));

            //开启一个线程，用于更新当前时间
            UpdateCurrentTime();

        }

        private void Goods_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Goods goods = sender as Goods;
            int selectedIndex = this.lvGoods.SelectedIndex;
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            int index = totalPurchase.PurchaseList.IndexOf(purchase);  //获取到该交易商品的索引
            totalPurchase.PurchaseList.RemoveAt(index);                //先移除这个交易商品(即从lvGoods上移除旧的该交易商品的那一项)
            totalPurchase.PurchaseList.Insert(index, purchase);        //再添加这一项(即在lvGoods上添加该交易商品的那一项，但这一项已经是新的)
            this.lvGoods.SelectedIndex = selectedIndex;                //重新选中这一项

            UpdateTotalMoney(purchase,purchase.Amount);
            UpdateTotalPreferential(purchase, 0);
            
        }

        /// <summary>
        /// 计算总交易的各项属性
        /// </summary>
        /// <param name="purchase"></param>
        private void CalcTotalPurchase(Purchase purchase)
        {
            #region 原代码区
            ////总金额=原总金额-原小计金额
            //totalPurchase.TotalMoney -= purchase.TotalMoney;
            ////总优惠=原总优惠-原小计优惠
            //totalPurchase.TotalPreferential -= purchase.Preferential;
            ////总交易件数=原总交易件数-原小计件数
            //totalPurchase.TotalAmount -= purchase.Amount;
            ////小计金额=购买数量*单价*折扣
            ////先将原小计优惠置为0
            //purchase.Preferential = 0;
            //purchase.TotalMoney = Math.Round((purchase.Goods.Price * purchase.Amount - purchase.Preferential) * purchase.Goods.DiscountRate, 2);
            ////小计优惠=赠送数量*单价*折扣
            //purchase.Preferential = purchase.SendAmount * purchase.Goods.Price +
            //              (purchase.Amount * purchase.Goods.Price - purchase.SendAmount * purchase.Goods.Price) * (1 - purchase.Goods.DiscountRate);
            ////purchase.Preferential = (purchase.Amount - purchase.SendAmount) - purchase.Goods.Price * (1 - purchase.Goods.DiscountRate);
            ////小计实际金额=小计金额-小计优惠
            ////purchase.TotalMoney = purchase.TotalMoney - purchase.Preferential;
            ////总金额=原总金额+新小计金额
            //totalPurchase.TotalMoney += purchase.TotalMoney;
            ////总优惠=原总优惠+新小计优惠
            //totalPurchase.TotalPreferential += purchase.Preferential;
            ////总实际金额=总金额-总优惠
            ////double realTotalMoney = totalPurchase.TotalMoney - totalPurchase.TotalPreferential;
            ////totalPurchase.TotalMoney = realTotalMoney;
            ////总交易件数=原总交易件数+新小计交易件数
            //totalPurchase.TotalAmount += purchase.Amount;
            #endregion
            //总金额=原总金额-原小计金额
            totalPurchase.TotalMoney -= purchase.TotalMoney;
            //总优惠=原总优惠-原小计优惠
            totalPurchase.TotalPreferential -= purchase.Preferential;
            //总交易件数=原总交易件数-原小计件数
            //totalPurchase.TotalAmount += purchase.Amount;
            //小计金额=单价×数量×折扣
            purchase.TotalMoney = purchase.Goods.Price * purchase.Amount * purchase.Goods.DiscountRate;
            //小计总金额=单价×(数量+赠送数量)
            double purchaseNoPrerefentialMoney = purchase.Goods.Price * (purchase.Amount + purchase.SendAmount);
            //小计优惠=小计总金额-小计金额
            purchase.Preferential = purchaseNoPrerefentialMoney - purchase.TotalMoney;
            //总金额=原总金额+新小计金额
            totalPurchase.TotalMoney += purchase.TotalMoney;
            //总优惠=原总优惠+新小计优惠
            totalPurchase.TotalPreferential += purchase.Preferential;
            if (purchaseNoPrerefentialMoney == 0)
            {
                int index = lvGoods.SelectedIndex;
                totalPurchase.PurchaseList.Remove(purchase);
                lvGoods.SelectedIndex = totalPurchase.PurchaseList.Count > 0 ? index - 1 : -1;
                if (index - 1 == -1)
                {
                    lvGoods.SelectedIndex = 0;
                }

            }
        }

        /// <summary>
        /// 创建一个交易
        /// </summary>
        private void CreateTotalPurchase()
        {
            DateTime now = DateTime.Now;
            totalPurchase.PurchaseTime = now;
            totalPurchase.PurchaseNum = "001001" + now.ToString("yyyyMMddHHmmss");   //交易单号
            totalPurchase.CashierNo = this.tbCashierNo.Text;                                  //收银员编号
            totalPurchase.PurchaseList = new ObservableCollection<Purchase>();                //初始化商品清单
            totalPurchase.TotalAmount = 0;
            totalPurchase.TotalDiscount = 0.00;
            totalPurchase.TotalMoney = 0.00;
            totalPurchase.TotalPreferential = 0.00;
            totalPurchase.IsRefund = false;
            totalPurchase.VIPCustomer = new VIPCustomer();
            totalPurchase.VIPCustomer.CustomerType = CustomerType.Common;
            totalPurchase.VIPCustomer.PropertyChanged += VIPCustomer_PropertyChanged;
            totalPurchase.CustomerType = totalPurchase.VIPCustomer.CustomerType;
            BindingTotalPurchaseAndUI();  //将总交易属性与相应UI控件绑定

        }

        private void VIPCustomer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VIPCustomer vip = sender as VIPCustomer;
            this.lblVipNo.Content = vip.No;       //会员编号
            this.lblVipName.Content = vip.Name;   //会员姓名
            //会员可用积分
            string exchangeScore = string.Empty;  
            if(vip.CustomerType == CustomerType.IntegralVIP || vip.CustomerType == CustomerType.IntegralStoredVIP)
            {
                exchangeScore = vip.ExchangeScore.ToString();
            }else
            {
                exchangeScore = "-";
            }
            this.lblVipExchangeScore.Content = exchangeScore;
            //会员可用余额
            string storedMoney = string.Empty;
            if(vip.CustomerType == CustomerType.StoredVIP || vip.CustomerType == CustomerType.IntegralStoredVIP)
            {
                storedMoney = vip.StoredMoney.ToString();
            }else
            {
                storedMoney = "-";
            }
            this.lblVipStoredMoney.Content = storedMoney;

            //条码窗口默认获取焦点
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));

        }

        /// <summary>
        /// 将总交易属性与相应UI控件绑定
        /// </summary>
        private void BindingTotalPurchaseAndUI()
        {
            //将交易单号与UI的"交易单号"控件绑定
            Binding numBinding = new Binding("PurchaseNum") { Source = totalPurchase };
            BindingOperations.SetBinding(this.tbPurchaseNum, TextBlock.TextProperty, numBinding);

            //将顾客类型与UI的"顾客类型"控件绑定

            //将交易件数与UI的"交易件数"控件绑定
            Binding countBinding = new Binding("TotalAmount") { Source = totalPurchase };
            BindingOperations.SetBinding(this.tbPurchaseAmount, TextBlock.TextProperty, countBinding);

            //将总金额与UI的"总金额"控件绑定
            Binding moneyBinding = new Binding("TotalMoney") { Source = totalPurchase };
            BindingOperations.SetBinding(this.tbTotalMoney, TextBlock.TextProperty, moneyBinding);

            //将总优惠与UI的"总优惠"控件绑定
            Binding preferentialBinding = new Binding("TotalPreferential") { Source = totalPurchase };
            BindingOperations.SetBinding(this.tbTotalPreferential, TextBlock.TextProperty, preferentialBinding);

            //为购物清单列表绑定数据源
            this.lvGoods.ItemsSource = totalPurchase.PurchaseList;
        }

        /// <summary>
        /// 更新当前时间
        /// </summary>
        private void UpdateCurrentTime()
        {
            timeThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                    string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
                    string current = now.ToString("yyyy-MM-dd HH:mm:ss ") + week;
                    sbtCurrentTime.Dispatcher.Invoke(new Action(() =>
                    {
                        sbtCurrentTime.Content = current;
                    }));
                    Thread.Sleep(1000);
                }

            }));
            timeThread.IsBackground = true;
            timeThread.Start();
        }
        /// <summary>
        /// "更多"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            ShowMoreCommandButton();
        }

        /// <summary>
        /// 显示更多命令按钮
        /// </summary>
        private void ShowMoreCommandButton()
        {
            StackPanel pnl = this.stackPnlMoreMenu;
            pnl.Visibility = pnl.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// "退货"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            RefundTotalPurchase();
        }

        /// <summary>
        /// 将整单交易退货
        /// </summary>
        private void RefundTotalPurchase()
        {
            if(totalPurchase.PurchaseList.Count > 0)
            {
                MessageBox.Show("当前商品列表数目不为0,不允许退货!","提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            totalPurchase.IsRefund = !totalPurchase.IsRefund;
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        /// <summary>
        /// "删除"单个商品按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelSingle_Click(object sender, RoutedEventArgs e)
        {
            DeleteSinglePurchase();
        }

        /// <summary>
        /// 删除单个商品
        /// </summary>
        private void DeleteSinglePurchase()
        {
            int index = lvGoods.SelectedIndex;
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            if (purchase == null) { return; }
            totalPurchase.TotalMoney -= purchase.TotalMoney;
            totalPurchase.TotalPreferential -= purchase.Preferential;
            totalPurchase.TotalAmount -= purchase.Amount;
            totalPurchase.PurchaseList.Remove(purchase);
            lvGoods.SelectedIndex = totalPurchase.PurchaseList.Count > 0 ? index - 1 : -1;
            if(index - 1 == -1)
            {
                lvGoods.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// "赠送"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendPurchaseGoods();
        }

        /// <summary>
        /// 赠送单个交易的商品
        /// </summary>
        private void SendPurchaseGoods()
        {
            //double sendAmount = 0.00;
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            if (purchase == null) { return; }
            if (!purchase.Goods.CanSend)
            {
                MessageBox.Show("当前商品不允许赠送!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //先计算允许赠送的数量 = 购买数量-赠送数量
            double allowSendAmount = purchase.Amount - purchase.SendAmount;
            SendWindow send = new SendWindow(
                delegate(double sendAmount)
                {
                    if(sendAmount > allowSendAmount)
                    {
                        MessageBox.Show("赠送数量不能大于最大赠送数量,最大赠送数量是" + (int)allowSendAmount, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    purchase.SendAmount = sendAmount;  //更新赠送数量
                    purchase.Amount -= sendAmount;
                    //UpdateTotalMoney(purchase,purchase.Amount);     //更新总交易金额
                    //UpdateTotalPreferential(purchase, 0);
                    CalcTotalPurchase(purchase);

                    //条码窗口默认获取焦点
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));
                });
            send.ShowDialog();
        }


        /// <summary>
        /// "改价"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePrice_Click(object sender, RoutedEventArgs e)
        {
            ChangePurchasePrice();
        }

        /// <summary>
        /// 更改交易商品的单价
        /// </summary>
        private void ChangePurchasePrice()
        {
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            if (purchase == null) { return; }
            if (!purchase.Goods.CanChangePrice)
            {
                MessageBox.Show("当前商品不允许改价!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ChangePriceWindow changePrice = new ChangePriceWindow(
                delegate (double newPrice)
                {
                    //后续需要修改：新售价不能低于最低售价
                    if(newPrice < purchase.Goods.MinPrice)
                    {
                        MessageBox.Show("新价格不能低于最低售价,最低售价是"+purchase.Goods.MinPrice,"提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    purchase.Goods.Price = newPrice;

                    //UpdateTotalMoney(purchase,purchase.Amount);  //更新总金额
                    CalcTotalPurchase(purchase);

                    //条码窗口默认获取焦点
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));
                });
            changePrice.ShowDialog();
        }

        /// <summary>
        /// "数量"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAmount_Click(object sender, RoutedEventArgs e)
        {
            ChangePurchaseAmount();
        }

        /// <summary>
        /// 更改单个交易商品的数量
        /// </summary>
        private void ChangePurchaseAmount()
        {
            //double newAmount = 0.00;
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            if (purchase == null) { return; }
            SetAmountWindow setAmount = new SetAmountWindow(
                delegate (double amount)
                {
                    if(amount == 0)
                    {
                        MessageBox.Show("商品数量不能是0!","提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    if(amount<purchase.SendAmount)
                    {
                        MessageBox.Show("商品数量不能小于优惠数量!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    //UpdatePurchaseAmount(purchase, amount);
                    purchase.Amount = amount;
                    CalcTotalPurchase(purchase);

                    //条码窗口默认获取焦点
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));
                });
            setAmount.ShowDialog();
        }

        /// <summary>
        /// 更新指定商品的数量
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="newAmount"></param>
        private void UpdatePurchaseAmount(Purchase purchase,double newAmount)
         {
            //总交易件数先减去旧交易件数，再加上新的交易件数
            int index = lvGoods.SelectedIndex;
            if(newAmount * purchase.Goods.Price - purchase.Preferential < 0)
            {
                purchase.Preferential = 0;
            }
            totalPurchase.TotalAmount -= purchase.Amount;
            //purchase.Amount = newAmount;
            Console.WriteLine("newAmount = "+newAmount + " --- " + " purchase.Preferential = "+purchase.Preferential);
            totalPurchase.TotalAmount += newAmount;
            UpdateTotalMoney(purchase,newAmount);                     //更新总交易金额
            UpdateTotalPreferential(purchase, newAmount);   //更新总优惠
            purchase.Amount = newAmount;
            if (purchase.Amount == 0)
            {
                totalPurchase.PurchaseList.Remove(purchase);
                lvGoods.SelectedIndex = totalPurchase.PurchaseList.Count > 0 ? index - 1 : -1;
            }
        }

        /// <summary>
        /// 更新总交易金额
        /// </summary>
        /// <param name="purchase"></param>
        private void UpdateTotalMoney(Purchase purchase,double newAmount)
        {
            //更新总金额
            //先减去原金额，再加上赠送后的实际金额
            totalPurchase.TotalMoney -= purchase.TotalMoney;
            purchase.TotalMoney = (newAmount * purchase.Goods.Price - purchase.SendAmount * purchase.Goods.Price) * purchase.Goods.DiscountRate;
            totalPurchase.TotalMoney += purchase.TotalMoney;
        }
        /// <summary>
        /// 更新总交易优惠
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="newAmount"></param>
        private void UpdateTotalPreferential(Purchase purchase,double newAmount)
        {
            //更新总优惠
            //totalPurchase.TotalPreferential -= purchase.Preferential;
            double preferential = 0;
            if(purchase.Amount < newAmount)
            {
                preferential = purchase.SendAmount * purchase.Goods.Price +
                         ((newAmount - purchase.Amount) * purchase.Goods.Price - purchase.SendAmount * purchase.Goods.Price) * (1 - purchase.Goods.DiscountRate);
                totalPurchase.TotalPreferential += preferential;
                purchase.Preferential += preferential;
            }
            else if(purchase.Amount > newAmount)
            {
                preferential = purchase.SendAmount * purchase.Goods.Price +
                         ((purchase.Amount - newAmount) * purchase.Goods.Price - purchase.SendAmount * purchase.Goods.Price) * (1 - purchase.Goods.DiscountRate);
                totalPurchase.TotalPreferential -= preferential;
                //if (newAmount > 0)
                //{
                    purchase.Preferential -= preferential;
                //}
            }
        }

        /// <summary>
        /// "挂单"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNoPayPurchase_Click(object sender, RoutedEventArgs e)
        {
            AddNoPayPurchase();
        }

        /// <summary>
        /// "挂单"方法
        /// </summary>
        private void AddNoPayPurchase()
        {
            /*
                         * 思路:
                         * 1.如果当前交易的商品列表不为空，那么直接挂单，不显示挂单窗口，同时新建一个交易
                         * 2.如果当前交易的商品列表为空，那么显示挂单窗口，让用户选择未付款交易
                         */
            if (totalPurchase.PurchaseList.Count > 0)
            {
                TotalPurchase currentTotalPurchase = new TotalPurchase()
                {
                    PurchaseNum = totalPurchase.PurchaseNum,
                    PurchaseTime = totalPurchase.PurchaseTime,
                    CashierNo = totalPurchase.CashierNo,
                    PurchaseList = totalPurchase.PurchaseList,
                    TotalAmount = totalPurchase.TotalAmount,
                    IsRefund = totalPurchase.IsRefund,
                    TotalMoney = totalPurchase.TotalMoney,
                    TotalPreferential = totalPurchase.TotalPreferential,
                    TotalDiscount = totalPurchase.TotalDiscount,
                    CustomerType = totalPurchase.CustomerType,
                    VIPCustomer = totalPurchase.VIPCustomer
                    
                };
                /*
                 * 如果挂单字典不包含当前交易，那么直接把当前交易存入挂单字典
                 * 如果挂单字典包含当前交易，那么直接把当前交易替换挂单字典中相同key的交易
                 */
                noPayTotalPurchaseList.Add(currentTotalPurchase);
                CreateTotalPurchase();
            }
            else
            {
                if(noPayTotalPurchaseList.Count == 0)
                {
                    MessageBox.Show("无挂单交易!","提示",MessageBoxButton.OK,MessageBoxImage.Information);
                    return;
                }
                NoPayPurchaseWindow noPayPurchase = new NoPayPurchaseWindow(noPayTotalPurchaseList,
                    delegate (TotalPurchase totalPurchase)
                    {
                        this.totalPurchase.PurchaseNum = totalPurchase.PurchaseNum;
                        this.totalPurchase.PurchaseTime = totalPurchase.PurchaseTime;
                        this.totalPurchase.PurchaseList = totalPurchase.PurchaseList;
                        this.totalPurchase.TotalMoney = totalPurchase.TotalMoney;
                        this.totalPurchase.TotalPreferential = totalPurchase.TotalPreferential;
                        this.totalPurchase.TotalDiscount = totalPurchase.TotalDiscount;
                        this.totalPurchase.TotalAmount = totalPurchase.TotalAmount;
                        this.tbPurchaseAmount.Text = this.totalPurchase.TotalAmount.ToString();
                        this.totalPurchase.CashierNo = totalPurchase.CashierNo;
                        this.totalPurchase.CustomerType = totalPurchase.VIPCustomer.CustomerType;
                        this.totalPurchase.IsRefund = totalPurchase.IsRefund;

                        this.lvGoods.ItemsSource = this.totalPurchase.PurchaseList;

                        this.lvGoods.SelectedIndex = 0;

                    });
                noPayPurchase.ShowDialog();
            }
        }

        /// <summary>
        /// "整单取消"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelAll_Click(object sender, RoutedEventArgs e)
        {
            DeleteAllPurchase();
        }

        /// <summary>
        /// "整单取消"方法
        /// </summary>
        private void DeleteAllPurchase()
        {
            CreateTotalPurchase(); //"全部取消"即放弃本次交易，重新开始新的交易
            this.stackPnlMoreMenu.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// "整单优惠"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePriceAll_Click(object sender, RoutedEventArgs e)
        {
            PreferentialAllPurchase();
        }

        /// <summary>
        /// "整单优惠"方法
        /// </summary>
        private void PreferentialAllPurchase()
        {
            this.stackPnlMoreMenu.Visibility = Visibility.Hidden;
            if (totalPurchase.PurchaseList.Count == 0) { return; }
            double preferentialMoney = 0.00;
            ChangePriceAllWindow priceAll = new ChangePriceAllWindow(
                delegate (double money)
                {
                    if (money < 0)
                    {
                        MessageBox.Show("优惠金额不能小于零!", "提示", MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    preferentialMoney = money;
                    totalPurchase.TotalMoney -= preferentialMoney;
                    totalPurchase.TotalPreferential += preferentialMoney;

                    //条码窗口默认获取焦点
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));

                });
            priceAll.ShowDialog();
        }

        /// <summary>
        /// 整单打折
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiscountAll_Click(object sender, RoutedEventArgs e)
        {
            DiscountAllPurchase();
        }

        /// <summary>
        /// "整单打折"方法
        /// </summary>
        private void DiscountAllPurchase()
        {
            this.stackPnlMoreMenu.Visibility = Visibility.Hidden;
            if (totalPurchase.PurchaseList.Count == 0) { return; }
            double discountAll = 0.00;
            DiscountAllWindow discountAllWindow = new DiscountAllWindow(
                delegate (double discount)
                {
                    if (discount < 0)
                    {
                        MessageBox.Show("折扣不能小于零!", "提示", MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    if (discount > 1.0)
                    {
                        MessageBox.Show("折扣不能大于1.0!", "提示", MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }
                    discountAll = discount;
                    totalPurchase.TotalDiscount = discountAll;
                    totalPurchase.TotalPreferential = totalPurchase.TotalMoney * (1 - totalPurchase.TotalDiscount);
                    totalPurchase.TotalMoney *= totalPurchase.TotalDiscount;


                });
            discountAllWindow.ShowDialog();
        }

        /// <summary>
        /// 全局快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

            Purchase purchase;
            double newAmount = 0;
            switch (e.Key)
            {
                //上箭头
                case Key.Up:
                    lvGoods.SelectedIndex = lvGoods.SelectedIndex - 1 == -1 ? 0 : lvGoods.SelectedIndex - 1;
                    break;
                //下箭头
                case Key.Down:
                    lvGoods.SelectedIndex = lvGoods.SelectedIndex + 1 == lvGoods.Items.Count ? lvGoods.Items.Count - 1 : lvGoods.SelectedIndex + 1;
                    break;
                //+键
                case Key.Add:
                    purchase = lvGoods.SelectedItem as Purchase;
                    purchase.Amount += 1;
                    CalcTotalPurchase(purchase);
                    break;
                //-键
                case Key.Subtract:
                    purchase = lvGoods.SelectedItem as Purchase;
                    purchase.Amount -= 1;
                    CalcTotalPurchase(purchase);
                    break;
                //"/"键
                case Key.Divide:
                case Key.Oem2:
                    //弹出结账窗口
                    CheckTotalPurchase();
                    break;
                //F1键-删除
                case Key.F1:
                    DeleteSinglePurchase();
                    break;
                //F2键-改价
                case Key.F2:
                    ChangePurchasePrice();
                    break;
                //F3键-数量
                case Key.F3:
                    ChangePurchaseAmount();
                    break;
                //F4键-赠送
                case Key.F4:
                    SendPurchaseGoods();
                    break;
                //F5键-挂单
                case Key.F5:
                    AddNoPayPurchase();
                    break;
                //F6键-退货
                case Key.F6:
                    RefundTotalPurchase();
                    break;
                //F7键-更多
                case Key.F7:
                    ShowMoreCommandButton();
                    break;
                //F8键-整单取消
                case Key.F8:
                    DeleteAllPurchase();
                    break;
                //F9键-整单优惠
                case Key.F9:
                    PreferentialAllPurchase();
                    break;
                //F10键-单项打折
                case Key.F10:
                case Key.System:
                    DiscountSinglePurchase();
                    break;
                //F11键-更改营业员
                case Key.F11:
                    break;
                //F12键-开钱箱
                case Key.F12:
                    break;

            }
       
        }

        private void txtPurchase_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) ||
               (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
               (e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.Back)
            {
                e.Handled = false;
            }
            else
            {

                e.Handled = true;
            }

            if (e.Key == Key.Enter)
            {
                string keyWords = this.txtPurchase.Text;
                Goods goods = GoodsManager.GetGoods(keyWords);
                if (goods != null)
                {
                    if(goods.SaleState ==  SaleState.Stop)
                    {
                        MessageBox.Show("该商品已停止销售!","提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                        this.txtPurchase.Clear();
                        return;
                    }
                    AddPurchase(goods);
                }
            }

        }

        /// <summary>
        /// "结账"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckTotalPurchase();
        }

        /// <summary>
        /// 结账
        /// </summary>
        private void CheckTotalPurchase()
        {
            CheckWindow check = new CheckWindow(totalPurchase, 
                delegate ()
                {
                    CreateTotalPurchase();
                });
            check.ShowDialog();
        }

        /// <summary>
        /// 单项打折
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDiscountSingle_Click(object sender, RoutedEventArgs e)
        {
            DiscountSinglePurchase();
        }

        /// <summary>
        /// "单项打折"方法
        /// </summary>
        private void DiscountSinglePurchase()
        {
            this.stackPnlMoreMenu.Visibility = Visibility.Hidden;
            Purchase purchase = lvGoods.SelectedItem as Purchase;
            if (purchase == null) { return; }
            if (!purchase.Goods.CanDiscount)
            {
                MessageBox.Show("当前商品不允许打折!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DiscountSingleWindow discountSingle = new DiscountSingleWindow(
                delegate (double discount)
                {
                    if (discount < 0)
                    {
                        MessageBox.Show("折扣不能小于零!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (discount > 1.0)
                    {
                        MessageBox.Show("折扣不能大于1.0!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    purchase.Goods.DiscountRate = discount;

                    //UpdateTotalMoney(purchase,purchase.Amount);   //更新总交易属性
                    CalcTotalPurchase(purchase);

                    //条码窗口默认获取焦点
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));

                });
            discountSingle.ShowDialog();
        }

        private void txtVIP_KeyDown(object sender, KeyEventArgs e)
        {
            /*
             * 输入会员编号/姓名/手机号，即可查出该会员编号、姓名、性别、手机号、住址、会员类型、可用积分(如果有)、可用储值(如果有)，
             * 并分别绑定到对应的控件上
             * 其中，如果是积分型会员，那么可用余额显示为-，如果是储值型会员，那么可用积分显示为-
             * 如果是储值积分型会员，这两个属性都显示
             * 
             */
             if(e.Key == Key.Enter)
            {
                VIPCustomer vip = totalPurchase.VIPCustomer;
                if (txtVIP.Text.Trim().Equals("8008"))
                { 
                    vip.No = "8008";
                    vip.Name = "张无忌";
                    vip.CustomerType = CustomerType.IntegralVIP;
                    vip.ExchangeScore = 3024;

                   

                }else if (txtVIP.Text.Trim().Contains("柳三姐"))
                {
                    vip.No = "8071";
                    vip.Name = "柳三姐";
                    vip.CustomerType = CustomerType.StoredVIP;
                    vip.StoredMoney = 12580;
                }
                else if (txtVIP.Text.Trim().Contains("15835407768"))
                {
                    vip.No = "8260";
                    vip.Name = "张学友";
                    vip.CustomerType = CustomerType.IntegralStoredVIP;
                    vip.StoredMoney = 30000;
                    vip.ExchangeScore = 5257;
                }
                totalPurchase.CustomerType = vip.CustomerType;
            }
        }

        private void btnInputVip_Click(object sender, RoutedEventArgs e)
        {
            this.stackPnlMoreMenu.Visibility = Visibility.Collapsed;
            this.txtVIP.Focus();
        }

        private void btnNoBarCodeSale_Click(object sender, RoutedEventArgs e)
        {
            this.stackPnlMoreMenu.Visibility = Visibility.Collapsed;
            this.txtNoBarCodeSale.Focus();
        }

        private void txtNoBarCodeSale_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Purchase purchase = new Purchase();
                Goods tempGoods = new Goods();
                tempGoods.BarCode = "9999";
                tempGoods.PluName = "无码商品";
                tempGoods.Price = double.Parse(this.txtNoBarCodeSale.Text.Trim());
                tempGoods.Unit = "个";
                tempGoods.PropertyChanged += this.Goods_PropertyChanged;
                purchase.Goods = tempGoods;
                purchase.Amount = 1;
                purchase.No = totalPurchase.PurchaseList.Count + 1;
                purchase.TotalMoney = Math.Round((purchase.Goods.Price * purchase.Amount - purchase.Preferential) * purchase.Goods.DiscountRate, 2);
                totalPurchase.TotalMoney += purchase.TotalMoney;
                totalPurchase.PurchaseList.Add(purchase);
                this.txtNoBarCodeSale.Clear();
                lvGoods.SelectedIndex = lvGoods.Items.Count - 1;

                //条码窗口默认获取焦点
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => this.txtPurchase.Focus()));

            }
        }

        /// <summary>
        /// 添加一项交易
        /// </summary>
        /// <param name="goods"></param>
        private void AddPurchase(Goods goods)
        {
            purchaseList = new List<Purchase>(totalPurchase.PurchaseList);                       //将本次交易的购物清单同步
            Purchase purchase = purchaseList.Find(tempPurchase => tempPurchase.Goods.PluCode.Equals(goods.PluCode));
            int index = 0;
            if(purchase != null)
            {
                purchase.Amount += 1;
                UpdateTotalMoney(purchase,purchase.Amount);
                index = purchaseList.IndexOf(purchase);

            }else
            {
                goods.PropertyChanged += this.Goods_PropertyChanged;
                int no = totalPurchase.PurchaseList.Count + 1;
                purchase = new Purchase(no, goods);
                totalPurchase.TotalMoney += purchase.TotalMoney;
                totalPurchase.TotalAmount += purchase.Amount;
                totalPurchase.PurchaseList.Add(purchase);
                index = lvGoods.Items.Count - 1; ;
            }

            lvGoods.SelectedIndex = index;
            this.txtPurchase.Clear();
        }
    }

   
}
