using PracticaCSR.Entities;
using PracticaCSR.Models.DTOs.Requests;
using PracticaCSR.Models.DTOs.Reponses;
using PracticaCSR.Repositories.Implementations;
namespace PracticaCSR.Services.Implementations;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService()
    {
        _productRepository = new ProductRepository();
    }
    
    public List<ProductForReadDto> GetAllProducts()
    {
        var products = _productRepository.GetAllProducts();
        var productDtos = products.Select(p => new ProductForReadDto()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        }).ToList();
        return productDtos;
    }

    public ProductForReadDto? GetProductById(int id)
    {
        var product = _productRepository.GetProductById(id);
        if (product == null)
        {
            return null;
        }
        return new ProductForReadDto()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public ProductForReadDto CreateProduct(ProductForCreateDto dto)
    {
        int id = _productRepository.GetAllProducts().Max(p=>p.Id) + 1;
        Product product = new Product()
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price
        };
        _productRepository.AddProduct(product);
        return new ProductForReadDto()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    public void UpdateProduct(int id, ProductForUpdateDto dto)
    {
        var product = new Product()
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price
        };
        _productRepository.UpdateProduct(product);
    }

    public void DeleteProduct(int id)
    {
        var product = _productRepository.GetProductById(id);
        _productRepository.DeleteProduct(product);
    }
}