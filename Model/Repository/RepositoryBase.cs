using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{

    public abstract class RepositoryBase<TModel, TPk> : IDbCollectionOperationsRepository<TModel, TPk>
    {
        private static readonly string Endpoint = "https://rcpt-cosmos-db.documents.azure.com:443/";
        private static readonly string Key = "b1KD3ujrFRXJpyFxRJDnyo4mdNp3jzV8e8QTe0msge3uAw36We9WbazevGTUc9pwjR8HmvQ2DKBAhlN12refXA==";
        protected static string DatabaseId;
        protected static string CollectionId;
        protected static DocumentClient docClient;

        public  RepositoryBase()
        {
            docClient = new DocumentClient(new Uri(Endpoint), Key);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        #region Private methods to create Database and Collection if not Exist
        /// <summary>
        /// The following function has following steps
        /// 1. Try to read database based on the DatabaseId passed as URI link, if it is not found the exception will be thrown
        /// 2. In the exception, the database will be created of which Id will be set as DatabaseId 
        /// </summary>
        /// <returns></returns>
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                //1.
                await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2.
                    await docClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// The following function has following steps
        /// 1.Read the collection based on the DatabaseId and Collectionid passed as URI, if not found then throw exception
        /// //2.In exception create a collection.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                //1.
                await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2.
                    await docClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        public async Task<TModel> AddDocumentIntoCollectionAsync(TModel item)
        {
            try
            {
                var document = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
                var res = document.Resource;
                var result = JsonConvert.DeserializeObject<TModel>(res.ToString());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteDocumentFromCollectionAsync(TPk id)
        {
            await docClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id.ToString()));
        }

        public async Task<TModel> GetItemFromCollectionAsync(TPk id)
        {
            try
            {
                Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id.ToString()));
                return JsonConvert.DeserializeObject<TModel>(doc.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(TModel);
                }
                else
                {
                    throw;
                }
            }
        }

        public abstract Task<IEnumerable<TModel>> GetItemsFromCollectionAsync();

        public async Task<TModel> UpdateDocumentFromCollection(TPk id, TModel item)
        {
            try
            {
                var document = await docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id.ToString()), item);
                var data = document.Resource.ToString();
                var result = JsonConvert.DeserializeObject<TModel>(data);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

