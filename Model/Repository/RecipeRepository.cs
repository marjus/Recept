
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recipes.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Linq;

namespace Repository
{

    public class RecipeRepository : RepositoryBase<Recipe, string>, IRecipeRepository //IDbCollectionOperationsRepository<Recipe, string>
    {

        public RecipeRepository() : base("RecipesDB", "Recipes")
        {
        }
    }
}
