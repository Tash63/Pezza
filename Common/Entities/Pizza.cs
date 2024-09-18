﻿namespace Common.Entities
{
    public class Pizza
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateTime? DateCreated { get; set; }

        public PizzaCategory Category { get; set; } 

        public bool InStock { get; set; }
    }
}
