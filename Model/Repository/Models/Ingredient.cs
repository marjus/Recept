using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recept.Models
{
    public class Ingredient : Product
    {
        public bool? AvoidFloatingPoints { get; set; }
        public virtual ConverterGroup ConverterGroup { get; set; }

        public virtual List<IngredientConversion> Converters{ get; set; }

    }
}