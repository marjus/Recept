using System;
using System.Collections.Generic;
using System.Text;
using Recipes.Models;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryBase<TModel, TPk>
    {
        Task<TModel> AddDocumentIntoCollectionAsync(TModel item);
        Task DeleteDocumentFromCollectionAsync(TPk id);
        Task<TModel> GetItemFromCollectionAsync(TPk id);
        Task<IEnumerable<TModel>> GetItemsFromCollectionAsync();
        Task<TModel> UpdateDocumentFromCollection(TPk id, TModel item);
    }
}
