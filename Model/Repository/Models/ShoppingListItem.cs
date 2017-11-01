using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class ShoppingListItem
    {
        [Key]
        public int Id { get; set; }

        public bool Done { get; set; }

        public bool DefaultHide { get; set; }

        public float? Amount { get; set; }

        public string Unit { get; set; }

        public virtual Product Product { get; set; }

        public int? FromRecipeId { get; set; }
        public virtual Recipe FromRecipe { get; set; }

        public int ShoppingListId { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }

    }
}