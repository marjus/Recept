
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

    public class RecipeRepository : RepositoryBase<Recipe, string> //IDbCollectionOperationsRepository<Recipe, string>
    {

        public RecipeRepository() : base("RecipesDB", "Recipes")
        {
        }

        public override async Task<IEnumerable<Recipe>> GetItemsFromCollectionAsync()
        {
            var documents = docClient.CreateDocumentQuery<Recipe>(
                  UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                  new FeedOptions { MaxItemCount = -1 })
                  .AsDocumentQuery();
            List<Recipe> persons = new List<Recipe>();
            while (documents.HasMoreResults)
            {
                persons.AddRange(await documents.ExecuteNextAsync<Recipe>());
            }
            return persons;
        }
    }
}
