/*using System.Collections.Generic;
using System.Linq;

namespace MarketPlace.Models.Repositories
{
    public class AssociatedProductAccountRepository : IProductRepository<AssociatedProductAccount>
    {
        List<AssociatedProductAccount> associatedProductAccounts;

        public AssociatedProductAccountRepository()
        {
            associatedProductAccounts = new List<AssociatedProductAccount>() {
            new AssociatedProductAccount
            {
                ProductId = 1,
                ProductQuantity = 6,
                

            },
            new AssociatedProductAccount
            {
                ProductId = 2,
                ProductQuantity = 7,
                SellerId = 2,
                ProductImageUrls = new List<string> {"image-1.jpg", "image-2.jpg", "image-3.jpg" },

            },
            new AssociatedProductAccount
            {
                ProductId = 3,
                ProductQuantity = 8,
                SellerId = 3,
                ProductImageUrls = new List<string> {"image-1.jpg", "image-2.jpg", "image-3.jpg" },

            },
            };
        }
        public void Add(AssociatedProductAccount entity)
        {
            associatedProductAccounts.Add(entity);
        }
        public int IsExist(AssociatedProductAccount entity) {
            return 0;
        }
        public void Delete(int ProductId,int SellerId)
        {
            var associatedProductAccount = Find(ProductId,SellerId); 
            associatedProductAccounts.Remove(associatedProductAccount);
        }

        public void Edit(int ProductId, int SellerId, AssociatedProductAccount entity)
        {
            var associatedProductAccount = Find(ProductId, SellerId);
            associatedProductAccount.ProductQuantity = entity.ProductQuantity;
            associatedProductAccount.ProductImageUrls = entity.ProductImageUrls;
        }

        public AssociatedProductAccount Find(int ProductId, int SellerId)
        {
            return associatedProductAccounts.SingleOrDefault(p=> p.ProductId==ProductId && p.SellerId==SellerId);
        }

        public IList<AssociatedProductAccount> List()
        {
            throw new System.NotImplementedException();
        }
    }
}
*/