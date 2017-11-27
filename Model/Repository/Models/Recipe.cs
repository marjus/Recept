using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Recipes.Models
{
    public class Recipe
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [Display(Name = "Titel")]
     //   [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [Display(Name ="Beskrivning")]
        [DataType(DataType.MultilineText)]
       // [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        
        //    public virtual ICollection<Tag> Tags { get; set; }

        public List<SubRecipe> SubRecipes { get; set; }

        //    public virtual ICollection<MenuRecipe> MenuRecipes { get; set; }

     //   [JsonProperty(PropertyName = "created_by")]
        public string CreatedByUserName { get; set; }

     //   [JsonProperty(PropertyName = "created_date")]
        public DateTime CreatedDate { get; set; }
        
        public Recipe()
        {
    //        this.SubRecipes = new List<SubRecipe>();
        }
    }
}