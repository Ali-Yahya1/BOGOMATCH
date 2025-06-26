using BOGOMATCH_DOMAIN.MODELS.Product;

namespace BOGOMATCH_DOMAIN.INTERFACE
{
    public interface IProducts
    {
        Task<List<Product>> GetAllProducts();
    }
}
