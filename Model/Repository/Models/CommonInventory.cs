using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class CommonInventory
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<CommonInventoryItem> InventoryItems {get;set;}
    }

    public class CommonInventoryItem
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        
        public float UnitsInStock { get; set; }
        public float MinimumUnitsInStock { get; set; }
    }
}