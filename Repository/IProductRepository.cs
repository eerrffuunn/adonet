using System.Collections.Generic;

namespace adonet
{
    // Do not change anything in this interface.
    public interface IProductRepository
    {
        Product FindById(int id);
        IReadOnlyList<Product> Search(string name);
        void Update(Product p);
        bool Delete(int id);
        bool UpdateWithConcurrencyCheck(Product p);
    }
}