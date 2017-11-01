using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string  Name { get; set; }
        
        public string PrimaryShoppingUnit { get; set; }

        public bool HaveInStock { get; set; }
        public bool Stockable { get; set; }
    }
}