using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Customer
{
    public class UpdateCustomerModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Cellphone { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
