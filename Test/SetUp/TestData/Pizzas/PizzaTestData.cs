﻿using Common.Entities;

namespace Test.Setup.TestData.Pizzas;

public static class PizzaTestData
{
    public static Faker faker = new();

    public static Pizza pizza = new()
    {
        Id = 1,
        Name = faker.PickRandom(pizzas),
        Description = string.Empty,
        Price = faker.Finance.Amount(),
        DateCreated = DateTime.Now,
    };

    public static PizzaModel PizzaModel = new()
    {
        Id = 1,
        Name = faker.PickRandom(pizzas),
        Description = string.Empty,
        Price = faker.Finance.Amount(),
        DateCreated = DateTime.Now

    };

    private static readonly List<string> pizzas = new()
    {
        "Veggie Pizza",
        "Pepperoni Pizza",
        "Meat Pizza",
        "Margherita Pizza",
        "BBQ Chicken Pizza",
        "Hawaiian Pizza"
    };
}
