using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNE_Security;


public class Invoice : Sales
{
    public UInt32 OrderID { get; set; }

    public DateTime? OrderTime {  get; set; }
    public DateTime CompletionTime { get; set; }
    public ushort CustomerID { get; set; }
    public enum States { Created, Confirmed, Packed, Done};
    public States State{ get; set; }
    public double TotalPrice { get; set; }
    public List<OrderLine> OrderLines { get; set; }

    public Invoice(UInt32 orderID, DateTime? orderTime, DateTime completionTime,
        double totalPrice, ushort customerID)
    {
        OrderID = orderID;
        OrderTime = orderTime;
        CompletionTime = completionTime;
        TotalPrice = totalPrice;
        CustomerID = customerID;
        
    }
    public Invoice(List<OrderLine> orderLines)
    {
        this.OrderLines = orderLines;
    }
}
