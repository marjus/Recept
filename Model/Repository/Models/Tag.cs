using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recipes.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; }
        public string Tagname { get; set; }

        public ICollection<SubRecipe> SubRecipes { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}