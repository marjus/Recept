using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recipes.Models
{
    public class Recipe
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Titel")]
        public string Title { get; set; }
        [Display(Name ="Beskrivning")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [NotMapped]
        public int ContentSize { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual List<SubRecipe> SubRecipes { get; set; }

        public virtual ICollection<MenuRecipe> MenuRecipes { get; set; }

        public string CreatedByUserName { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool Public { get; set; }

        public Recipe()
        {
            this.SubRecipes = new List<SubRecipe>();
        }
    }
}