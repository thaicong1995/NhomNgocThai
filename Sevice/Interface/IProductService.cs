using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Sevice.Interface
{
    public interface IProductService
    {
        string AddNewProduct(int UserId, int ShopId, ProductDto productDto, IFormFile image);
        Product GetProductByID(int productId);
        string UpdateProduct(int userId, int shopId, int productId, ProductDto productDto, IFormFile image);
        bool UpdateCart(int productId, string productName, decimal price);
        List<Product> GetAllByShopID(int ShopId);
        List<Object> SearchProducts(string keyword);
        List<Product> GetAll(int page = 1);
        byte[] GetProductImageBytes(string imagePath);

    }

}

