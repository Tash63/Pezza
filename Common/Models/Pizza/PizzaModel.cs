﻿namespace Common.Models.Pizza
{
    public class PizzaModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public required PizzaCategory Category { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool InStock {  get; set; }
    }
}
