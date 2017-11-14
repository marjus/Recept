using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recipes.Models
{
    public class StandardConversions
    {
        [Key]
        public int Id { get; set; }
        public ConverterGroup ConverterGroup { get; set; }
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public float ConversionFactor { get; set; }
    }
}