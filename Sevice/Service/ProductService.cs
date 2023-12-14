using WebApi.Dto;
using WebApi.Models;
using WebApi.Models.Enum;
using WebApi.MyDbContext;
using WebApi.Sevice.Interface;
using WebApi.Reposetory.Interface;

namespace WebApi.Sevice.Service
{
    public class ProductService : IProductService
    {
        private readonly MyDb _myDb;
        private readonly IRepository _iRepository;
        public ProductService(MyDb myDb, IRepository repository)
        {
            _myDb = myDb;
            _iRepository = repository;
        }
        string PathImgFoder = "C:\\Users\\thaic\\OneDrive\\Máy tính\\ImgP3";

        //----Quản lý ảnh. khi store lưu ảnh về server - mỗi store = 1 forder + tên store(xử lý xong)
        public string AddNewProduct(int UserId, int ShopId, ProductDto productDto, IFormFile image)
        {
            try
            {
                Shop shop = _iRepository.GetShopByID(ShopId);
                if (shop == null)
                {
                    throw new Exception("Not Found!");
                }
                if (shop.UserId != UserId)
                {
                    return "You do not have access to this shop!!";
                }
                if (shop._shopStatus == ShopStatus.Active)
                {

                    Product newProduct = new Product
                    {
                        ProductName = productDto.ProductName,
                        Description = productDto.Description,
                        Category = productDto.Category,
                        Price = productDto.Price,
                        Quantity = productDto.Quantity,
                        ShopId = shop.Id,
                        _productStatus = ProductStatus.InOfStock
                    };

                    if (newProduct.Quantity == 0)
                    {
                        newProduct._productStatus = ProductStatus.OutOfStock;
                    }


                    if (image != null && image.Length > 0)
                    {
                        string imagePath = SaveProductImage(image, shop.ShopName);
                        newProduct.ImagePath = imagePath;

                        _myDb.Products.Add(newProduct);
                        _myDb.SaveChanges();

                        return "Product added successfully!";
                    }
                    else
                    {
                        return "No image provided.";
                    }
                }
                else
                {
                    return "You need to activate the shop!";
                }
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        private string SaveProductImage(IFormFile image, string shopName)
        {
            try
            {
                string shopFolder = Path.Combine(PathImgFoder, shopName);
                if (!Directory.Exists(shopFolder))
                {
                    Directory.CreateDirectory(shopFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(shopFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                return filePath;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred while saving the image: {e.Message}");
            }
        }


        public Product GetProductByID(int productId)
        {
            return _iRepository.GetProductByID(productId);
        }

        public string UpdateProduct(int userId, int shopId, int productId, ProductDto productDto, IFormFile image)
        {
            try
            {
                // Kiểm tra xem người dùng có quyền chỉnh sửa sản phẩm hay không
                var shop = _iRepository.GetShopByID(shopId);
                if (shop == null)
                {
                    return "Shop not found!";
                }

                if (shop.UserId != userId)
                {
                    return "You do not have access to this shop!";
                }

                if (shop._shopStatus != ShopStatus.Active)
                {
                    return "You need to activate the shop!";
                }

                // Tìm sản phẩm cần chỉnh sửa
                var product = _iRepository.GetProductByID(productId);

                if (product == null)
                {
                    return "Product not found!";
                }

                // Kiểm tra xem sản phẩm thuộc gian hàng của người dùng hay không
                if (product.ShopId != shop.Id)
                {
                    return "You do not have access to this product!";
                }

                if (!string.IsNullOrEmpty(productDto.ProductName))
                {
                    product.ProductName = productDto.ProductName;
                }

                if (!string.IsNullOrEmpty(productDto.Description))
                {
                    product.Description = productDto.Description;
                }

                if (!string.IsNullOrEmpty(productDto.Category))
                {
                    product.Category = productDto.Category;
                }

                if (productDto.Price > 0)
                {
                    product.Price = productDto.Price;
                }

                if (productDto.Quantity > 0)
                {
                    product.Quantity = productDto.Quantity;
                }

                if (productDto.Price < 0)
                {
                    return "Invalid price!";
                }

                if (productDto.Quantity == 0)
                {
                    product._productStatus = ProductStatus.OutOfStock;
                }
                else if (productDto.Quantity < 0)
                {
                    return "Invalid quantity!";
                }
                else
                {
                    product.Price = productDto.Price;
                    product.Quantity = productDto.Quantity;
                    if (product._productStatus == ProductStatus.OutOfStock)
                    {
                        product._productStatus = ProductStatus.InOfStock;
                    }
                }

                if (image != null && image.Length > 0)
                {
                    string imageUrl = SaveProductImage(image, shop.ShopName);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        product.ImagePath = imageUrl;
                    }
                    else
                    {
                        return "Failed to save the new image.";
                    }
                }

                _myDb.SaveChanges();

                // Sau khi cập nhật sản phẩm, gọi hàm UpdateCart để cập nhật giỏ hàng
                bool cartUpdated = UpdateCart(productId, productDto.ProductName, productDto.Price);

                if (cartUpdated)
                {
                    return "Product updated successfully!";
                }
                else
                {
                    return "Product updated successfully! Product not found .";
                }
            }
            catch (Exception e)
            {
                return $"An error occurred: {e.Message}";
            }
        }

        public bool UpdateCart(int productId, string productName, decimal price)
        {
            try
            {
                CartItem cartItem = _iRepository.GetCart(productId);

                if (cartItem != null)
                {
                    // Cập nhật thông tin sản phẩm trong giỏ hàng
                    cartItem.ProductName = productName;
                    cartItem.Price = price;
                    cartItem.PriceQuantity = cartItem.Price * cartItem.Quantity;

                    _myDb.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Product> GetAllByShopID(int ShopId)
        {
            try
            {
                // Kiểm tra sự tồn tại của cửa hàng với shopId
                if (!_iRepository.IsShopActive(ShopId))
                {
                    throw new Exception("Shop not found!");
                }

                List<Product> products = _iRepository.GetProductsByShop(ShopId);

                return products;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        // keyword == shop name - get shop + product 
        //keyword != shopname - get product 

        public List<Object> SearchProducts(string keyword)
        {
            keyword = keyword?.ToLower();
            List<object> results = new List<object>();

            IQueryable<Product> products = _myDb.Products;

            // so sánh keyword với shopName
            Shop matchingShops = _iRepository.FindShopByKeyword(keyword);

            List<Product> product = _iRepository.FindProductsByKeyword(keyword);

            if (matchingShops != null || product.Any())
            {
                if (matchingShops != null)
                {
                    // Nếu có cửa hàng trùng khớp với từ khóa
                    // hiển thị tất cả sản phẩm của cửa hàng đó
                    results.Add(new { Shop = matchingShops, Product = product });
                }
                else
                {
                    // Nếu không có cửa hàng trùng khớp, hiển thị tất cả sản phẩm có từ khóa trong tên sản phẩm
                    results.Add(product);
                }
            }
            else
            {
                results.Add(new { Message = "Không tìm thấy kết quả." });
            }
            return results;
        }

        public List<Product> GetAll(int page = 0)
        {
            try
            {
                int pageSize = 10; // Giá trị mặc định cho pageSize
                int skip = page  * pageSize;

                List<Product> products = _myDb.Products.Skip(skip).Take(pageSize).ToList();

                return products;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        public byte[] GetProductImageBytes(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    throw new FileNotFoundException("Image not found!");
                }

                return File.ReadAllBytes(imagePath);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

    }
}
