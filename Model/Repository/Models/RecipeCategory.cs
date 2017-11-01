using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class RecipeCategory
    {
        [Key]
        public int ID { get; set; }

        [Display(Name ="Typ")]
        public string Category { get; set; }
    }
}