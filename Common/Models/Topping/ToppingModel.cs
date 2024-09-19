﻿namespace Common.Models.Topping
{
    public class ToppingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PizzaId { get; set; }

        public double Price { get; set; }

        public bool Additional { get; set; }

        public bool InStock { get; set; }

    }
}