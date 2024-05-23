namespace Projet.Models.Repositories
{
    public interface ICategoryRepostiory
    {
        public interface ICategoryRepository
        {
            IList<Category> GetAll();
            Category GetById(int id);
            void Add(Category s);
            void Edit(Category s);
            void Delete(Category s);
            IList<Category> GetCategoriesByCategoryID(int? CategoryId);
            IList<Category> FindByName(string? name);

        }
    }
}
