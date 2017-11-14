using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recipes.Models
{
    public class MenuRecipe
    {
        [Key]
        public int Id { get; set; }
        
        public int MenuId { get; set; }
        public int RecipeID { get; set; }
        public int Servings { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}