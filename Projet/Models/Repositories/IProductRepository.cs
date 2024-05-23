namespace Projet.Models.Repositories
{
    public interface IProductRepository
    {
        IList<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
		Product Update(Product product);
        void Delete(Product product);
        IList<Product> GetProductByCategoryId (int categoryId);
        IList<Product> FindByName(string name);

         IList<Product> GetProductsByPriceAscendant(double price);
         IList<Product> GetProductsByPriceDescendant(double price);

    }
}
