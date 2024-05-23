using static Projet.Models.Repositories.ICategoryRepostiory;

namespace Projet.Models.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext context;

        public CategoryRepository(ProductContext context)
        {
            this.context = context;
        }

        public IList<Category> GetAll()
        {
            return context.Categories.OrderBy(c => c.CategoryName).ToList();
        }

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Edit(Category category)
        {
            context.Categories.Update(category);
            context.SaveChanges();
        }

        public void Delete(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public IList<Category> GetCategoriesByCategoryID(int? categoryId)
        {
            if (categoryId == null)
            {
                return new List<Category>();
            }
            return context.Categories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public IList<Category> FindByName(string name)
        {
            return context.Categories.Where(c => c.CategoryName.Contains(name)).ToList();
        }
    }
}
