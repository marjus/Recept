using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recipes.Models
{
    public class IngredientMapper
    {
        public int ID { get; set; }
        public string FromValue { get; set; }
        public string ToValue { get; set; }
    }
}