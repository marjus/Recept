using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data.Entity;
using Rcpt.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace Rcpt.Hubs
{
    [HubName("shoppingListHub")]
    public class ShoppingListHub : Hub
    {
        [HubMethodName("setProductBought")]
        public void SetShoppingListItemStatus(int shoppingListItemId, bool status)
        {
            using (Rcpt.DAL.RecipeContext db = new Rcpt.DAL.RecipeContext())
            {
                var sli = db.ShoppingListItems.SingleOrDefault(s => s.Id == shoppingListItemId);

                if (sli != null)
                {
                    sli.Done = status;
                    db.SaveChanges();

                    Clients.All.ItemStatusChanged(
                       sli.ShoppingListId,
                       sli.Id,
                       status
                       );
                }
            }
        }

        [HubMethodName("setShoppingListClosed")]
        public void SetShoppingListClosed(int shoppingListId)
        {
            using (Rcpt.DAL.RecipeContext db = new Rcpt.DAL.RecipeContext())
            {
                var sl = db.ShoppingLists.SingleOrDefault(s => s.Id == shoppingListId);

                if (sl != null)
                {
                    sl.IsOpen = false;
                    db.SaveChanges();

                    Clients.All.ListClosed(
                       sl.Id
                    );
                }
            }
        }

        [HubMethodName("addProduct")]
        public void AddProduct(string inputProduct)
        {
            var product = Util.InputParser.parseProduct(inputProduct);

            if (!string.IsNullOrEmpty(product.product))
            {

                using (Rcpt.DAL.RecipeContext db = new Rcpt.DAL.RecipeContext())
                {
                    // Get latest open shoppinglist
                    var list = db.ShoppingLists.Where(s => s.IsOpen && s.UserName == System.Environment.UserName && !s.MenuId.HasValue).OrderByDescending(s => s.CreatedDate).FirstOrDefault();

                    if (list == null)
                        list = db.ShoppingLists.Add(new ShoppingList { CreatedDate = DateTime.Now, IsOpen = true, UserName = System.Environment.UserName });

                    var prod = db.Products.SingleOrDefault(p => p.Name.ToLower().Equals(product.product.ToLower()));

                    if (prod == null)
                        prod = db.Products.Add(new Rcpt.Models.Product { Name = product.product });

                    var sli = new ShoppingListItem { Product = prod, Unit = product.unit };

                    if (product.amount > 0)
                        sli.Amount = product.amount;

                    list.ShoppingListItems.Add(sli);

                    db.SaveChanges();

                    Clients.All.ProductAdded(
                        product.amount.HasValue ? product.amount.ToString() : "", 
                        string.IsNullOrEmpty(product.unit) ? "" : product.unit, 
                        product.product, 
                        list.Id,
                        sli.Id
                        );
                }
            }
            
        }
    }
}