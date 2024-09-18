﻿using Common.Enums;

namespace Common.Models.Order;

public sealed class CreateOrderModel
{
    public required int CustomerId { get; set; }

    public required List<int> PizzaIds { get; set; }

    public required List<int> SideIds { get; set; }

    public required OrderStatus Status { get; set; }
}