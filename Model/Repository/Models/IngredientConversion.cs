using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class IngredientConversion
    {
        [Key]
        public int Id { get; set; }
        
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public float ConversionFactor { get; set; }
    }
}