
using Microsoft.EntityFrameworkCore;

namespace Projet.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
            private readonly ProductContext context;

            public ProductRepository(ProductContext context)
            {
                this.context = context;
            }

            public IList<Product> GetAll()
            {
                return context.Products.OrderBy(p => p.Category).ToList();
            }

            public Product GetById(int id)
            {
                return context.Products.Find(id);
            }

            public void Add(Product product)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

        public Product Update(Product product)
        {
            // Check if the associated category exists
            var existingCategory = context.Categories.Find(product.CategoryId);
            if (existingCategory == null)
            {
                
                throw new Exception("Associated category does not exist.");
            }

            // Attach the product entity to the context and mark it as modified
            var updatedProduct = context.Products.Attach(product);
            updatedProduct.State = EntityState.Modified;

            // Save changes to the database
            context.SaveChanges();

            return product;
        }

        public void Delete(Product product)
        {
                context.Products.Remove(product);
                context.SaveChanges();
        }

        public IList<Product> GetProductByCategoryId(int categoryId)
        {
                return context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId == categoryId)
                    .ToList();
        }

         public IList<Product> FindByName(string name)
         {
                return context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(name))
                    .ToList();
         }
        public IList<Product> GetProductsByPriceAscendant(double price)
        {
            return context.Products.Where(p => p.Price >= price)
                                           .OrderBy(p => p.Price)
                                           .ToList();
        }
        public IList<Product> GetProductsByPriceDescendant(double price)
        {
            return context.Products
                .Where(p => p.Price >= price)
                .OrderByDescending(p => p.Price)
                .ToList();
        }


    }
}