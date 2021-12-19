using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdoClass.DbUtil;

// Assignment 6 .net
public class AdoCommand
{
    // Declare Sql Conncetion and name it conn
    SqlConnection conn;

    public string DBConnectionString = "";

    public AdoCommand()
    {
        conn = new SqlConnection("Data Source = lugh4.it.nuigalway.ie; Initial Catalog = msdb2782; Persist Security Info = True; UID=msdb2782; Password = msdb2782JO");
    }

    public AdoCommand(string DBConnectionString)
    {
        this.DBConnectionString = DBConnectionString;
    }

    static void Main()
    {
    }

    #region 新增訂單
    public string InsOrders(object createOrder)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string OrderID = string.Empty;
        string insSQL = @" INSERT INTO [ORDERS] (CustomerID, OrderDate, ShipAddress, ShipCity)
                           VALUES (@CustomerID, GETDATE(), @ShipAddress, @ShipCity)  
                           SELECT TOP 1 OrderID FROM [ORDERS] WHERE CustomerID = @CustomerID ORDER BY OrderDate Desc ";

        OrderID = shaneDbUtil.ExecScalar(insSQL, createOrder);

        return OrderID;
    }

    public int InsOrdersDetails(string OrderID, string ProductID, string Quantity)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string insSQL = @"   INSERT INTO [Order Details] (OrderID,ProductID, UnitPrice, Quantity)
                             VALUES (@OrderID, @ProductID, (SELECT UnitPrice FROM [Products] WHERE ProductID = @ProductID ), @Quantity) ";


        var result = shaneDbUtil.Exec(insSQL, new { OrderID, ProductID, Quantity });
        return result;
    }
    #endregion

    #region 訂單控制類

    /// <summary>
    /// 查詢訂單
    /// </summary>
    /// <typeparam name="VModel"></typeparam>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    public List<VModel> GetOrderByOrderID<VModel>(string OrderID)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        List<VModel> result = new List<VModel>();

        string selSQL = @" SELECT  OS.OrderID,
                                   OS.CustomerID,
                                   OS.OrderDate,
                                   OS.ShipAddress,
                                   OS.ShipCity,
                                   PS.ProductName,
                                   ODS.UnitPrice,
                                   ODS.Quantity,
                                   OS.OrderStatus
                            FROM   [Orders] OS
                                   LEFT JOIN [Order Details] ODS
                                          ON OS.OrderID = ODS.OrderID
                                   LEFT JOIN Products PS
                                          ON ODS.ProductID = PS.ProductID 
                            WHERE  OS.OrderID = @OrderID  ";

        result = shaneDbUtil.Exec<VModel>(selSQL, new { OrderID });

        return result;
    }

    /// <summary>
    /// 修改訂單內容
    /// </summary>
    /// <typeparam name="VModel"></typeparam>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    public int UpdOrderWithOrderInfo(string OrderID, string ShipCity, string ShipAddress)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string updSQL = @" UPDATE [Orders]
                              SET ShipCity = @ShipCity,
                                  ShipAddress = @ShipAddress
                            WHERE OrderID = @OrderID ";


        int result = shaneDbUtil.Exec(updSQL, new { OrderID, ShipCity, ShipAddress });

        return result;
    }

    /// <summary>
    /// 刪除訂單
    /// </summary>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    public int DelOrder(string OrderID)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string updSQL = @" DELETE FROM [Order Details]
                            WHERE OrderID = @OrderID
                            DELETE FROM [Orders]
                            WHERE OrderID = @OrderID ";

        int updResult = shaneDbUtil.Exec(updSQL, new { OrderID });
        return updResult;
    }
    #endregion


    #region 其他類
    /// <summary>
    /// 依據交易結果回壓訂單主檔狀態
    /// </summary>
    /// <param name="OrderID"></param>
    /// <param name="OrderStatus"></param>
    /// <returns></returns>
    public int UpdOrderStatusByRtnCode(string OrderID, string OrderStatus)
    {
        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string updSQL = @"  UPDATE Orders
                            SET OrderStatus = @OrderStatus
                            WHERE OrderID= @OrderID  ";

        int updResult = shaneDbUtil.Exec(updSQL, new { OrderID, OrderStatus });
        return updResult;
    }

    /// <summary>
    /// 新增歷史訂單主檔
    /// </summary>
    /// <typeparam name="VModel"></typeparam>
    /// <param name="OrderID"></param>
    /// <returns></returns>
    public string InsOrdersHistory(object createOrder)
    {

        ShaneDbUtil shaneDbUtil = new ShaneDbUtil(this.DBConnectionString);

        string OrderID = string.Empty;
        string insSQL = @" INSERT INTO [ORDERSHISTORY] (OrderID, CustomerID, OrderDate, ShipAddress, ShipCity)
                           VALUES (@OrderID, @CustomerID, GETDATE(), @ShipAddress, @ShipCity)  
                           SELECT TOP 1 OrderID FROM [ORDERSHISTORY] WHERE OrderID = @OrderID  ";

        OrderID = shaneDbUtil.ExecScalar(insSQL, createOrder);

        return OrderID;
    }
    #endregion
}