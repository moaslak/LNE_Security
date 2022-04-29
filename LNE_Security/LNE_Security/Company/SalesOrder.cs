using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class SalesOrder
{
    const double VATS = 1.25;

    public List<OrderLine> OrderLines = new List<OrderLine>();
    public UInt16 ContactInfoID { get; set; }

    public UInt32 OrderID { get; set; }

    public double TotalPrice { get; set; }

    public DateTime OrderTime { get; set; }
    public DateTime? CompletionTime { get; set; }

    public OrderLine? orderLine { get; set; }
    public UInt16 CID {get; set; }
    public string FullName { get; set; }
    public UInt16 CompanyID { get; set; }
    public UInt16 OLID { get; set; }
    public enum States { Created, Confirmed, Packed, Closed, Canceled, Error, Incomplete };
    public States State
    {
        get
        {
            OrderLines = Database.Instance.GetOrderLines(OrderID);
            int[] sameStateCount = new int[7];

            for(int i = 0; i < OrderLines.Count; i++)
            {
                if (OrderLines[i].State == OrderLine.States.Created)
                {
                    sameStateCount[0]++;
                    if (OrderLines.Count == sameStateCount[0])
                        return SalesOrder.States.Created;
                }
                if (OrderLines[i].State == OrderLine.States.Confirmed)
                {
                    sameStateCount[1]++;
                    if (OrderLines.Count == sameStateCount[1])
                        return SalesOrder.States.Confirmed;
                }
                if (OrderLines[i].State == OrderLine.States.Packed)
                {
                    sameStateCount[2]++;
                    if (OrderLines.Count == sameStateCount[2])
                        return SalesOrder.States.Packed;
                }
                if (OrderLines[i].State == OrderLine.States.Closed)
                {
                    sameStateCount[3]++;
                    if (OrderLines.Count == sameStateCount[3])
                        return SalesOrder.States.Closed;
                }
                if (OrderLines[i].State == OrderLine.States.Canceled)
                {
                    sameStateCount[4]++;
                    if (OrderLines.Count == sameStateCount[4])
                        return SalesOrder.States.Canceled;
                }
                if (OrderLines[i].State == OrderLine.States.Error)
                {
                    sameStateCount[5]++;
                    if (OrderLines.Count == sameStateCount[5])
                        return SalesOrder.States.Error;
                }
                if (OrderLines[i].State == OrderLine.States.Incomplete)
                {
                    sameStateCount[6]++;
                    if (OrderLines.Count == sameStateCount[6])
                        return SalesOrder.States.Incomplete;
                }
            }
            //Database.Instance.EditSalesOrder(Database.Instance.SelectSalesOrder(OrderID));
            return SalesOrder.States.Incomplete;
        }
        set { }
    }

    /// <summary>
    /// Adds the VATS to the total price
    /// </summary>
    /// <param name="VATS"></param>
    /// <param name="TotalPrice"></param>
    /// <returns>double</returns>
    public double CalculateVATS(double VATS, double TotalPrice)
    {
        return TotalPrice * VATS;
    }

    /// <summary>
    /// Calculates the total price from ALL the orderlines.
    /// </summary>
    /// <param name="orderLines"></param>
    /// <returns>double</returns>
    public double CalculateTotalPrice(List<OrderLine> orderLines)
    {
        if(orderLines == null)
            return 0;
        
        double totalPrice = 0;
        foreach(OrderLine orderLine in orderLines)
        {
            if (orderLine.Product.SalesPrice < 0)
                return 0;
            else
                totalPrice = totalPrice + orderLine.Product.SalesPrice * orderLine.Quantity;
        }
        return totalPrice;
    }        
}
