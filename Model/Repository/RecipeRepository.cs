
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
    /// <summary>
    /// The interface contains methods for Operations on Collections
    /// </summary>
    /// <typeparam name="TModel"-></typeparam>
    //public interface IDbCollectionOperationsRepository<TModel, in TPk>
    //{
    //    Task<IEnumerable<TModel>> GetItemsFromCollectionAsync();
    //    Task<TModel> GetItemFromCollectionAsync(TPk id);
    //    Task<TModel> AddDocumentIntoCollectionAsync(TModel item);
    //    Task<TModel> UpdateDocumentFromCollection(TPk id, TModel item);
    //    Task DeleteDocumentFromCollectionAsync(TPk id);
    //}
    public class RecipeRepository : RepositoryBase<Recipe, string> //IDbCollectionOperationsRepository<Recipe, string>
    {
        //private static readonly string Endpoint = "https://rcpt-cosmos-db.documents.azure.com:443/";
        //private static readonly string Key = "b1KD3ujrFRXJpyFxRJDnyo4mdNp3jzV8e8QTe0msge3uAw36We9WbazevGTUc9pwjR8HmvQ2DKBAhlN12refXA==";
        //private readonly string DatabaseId = "RecipesDB";
        //private static readonly string CollectionId = "Recipes";
        //private static DocumentClient docClient;

        public RecipeRepository() 
        {
            DatabaseId = "RecipesDB";
            CollectionId = "Recipes";
        }

        //#region Private methods to create Database and Collection if not Exist
        ///// <summary>
        ///// The following function has following steps
        ///// 1. Try to read database based on the DatabaseId passed as URI link, if it is not found the exception will be thrown
        ///// 2. In the exception, the database will be created of which Id will be set as DatabaseId 
        ///// </summary>
        ///// <returns></returns>
        //private static async Task CreateDatabaseIfNotExistsAsync()
        //{
        //    try
        //    {
        //        //1.
        //        await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            //2.
        //            await docClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
        ///// <summary>
        ///// The following function has following steps
        ///// 1.Read the collection based on the DatabaseId and Collectionid passed as URI, if not found then throw exception
        ///// //2.In exception create a collection.
        ///// </summary>
        ///// <returns></returns>
        //private static async Task CreateCollectionIfNotExistsAsync()
        //{
        //    try
        //    {
        //        //1.
        //        await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            //2.
        //            await docClient.CreateDocumentCollectionAsync(
        //                UriFactory.CreateDatabaseUri(DatabaseId),
        //                new DocumentCollection { Id = CollectionId },
        //                new RequestOptions { OfferThroughput = 1000 });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
        //#endregion

        //public async Task<Recipe> AddDocumentIntoCollectionAsync(Recipe item)
        //{
        //    try
        //    {
        //        var document = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        //        var res = document.Resource;
        //        var recipe = JsonConvert.DeserializeObject<Recipe>(res.ToString());
        //        return recipe;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task DeleteDocumentFromCollectionAsync(string id)
        //{
        //    await docClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        //}

        //public async Task<Recipe> GetItemFromCollectionAsync(string id)
        //{
        //    try
        //    {
        //        Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        //        return JsonConvert.DeserializeObject<Recipe>(doc.ToString());
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

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

        //public async Task<Recipe> UpdateDocumentFromCollection(string id, Recipe item)
        //{
        //    try
        //    {
        //        var document = await docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        //        var data = document.Resource.ToString();
        //        var person = JsonConvert.DeserializeObject<Recipe>(data);
        //        return person;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
    }
}
