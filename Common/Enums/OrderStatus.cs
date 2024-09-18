namespace Common.Enums;

// This ENUM is used to show the order status at any given time
public enum OrderStatus
{// Status to indicate that the order has been placed by the user
    [Description("Placed")]
    Placed = 0,

    // Status to indicate that the order has been accepted by the resturant
    [Description("Accepted")]
    Accepted=1,
    
    // Status to indicate that the order has been cancelled by the user
    [Description("Cancelled")]
    Cancelled=2,

    // Status to indicate that the order has been rejected by the staff
    [Description("Rejected")]
    Rejected=3,

    // Status to indicate that the order is in the process of being forfulliled
    [Description("OrderInProcess")]
    OrderInProcess=4,

    // Status to indicate the order is complete
    [Description("OrderComplete")]
    OrderComplete=5,

    // Status to indicate that the order is ready to be collected
    [Description("Collected")]
    Collected=6
}