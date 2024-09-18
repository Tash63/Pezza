﻿namespace Common.Models.Side
{
    public class CreateSideModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool InStock { get; set; }
    }
}