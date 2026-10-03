using PracticaCSR.Entities;
using PracticaCSR.Models.DTOs.Requests;
using PracticaCSR.Models.DTOs.Reponses;
using PracticaCSR.Repositories.Implementations;
using PracticaCSR.Repositories.Interfaces;
using PracticaCSR.Services.Interfaces;

namespace PracticaCSR.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository repository)
    {
        _productRepository = repository;
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
        if (ProductExistsWithName(dto.Name))
        {
            throw new InvalidOperationException("Ya existe un producto con ese nombre.");
        }
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

    public List<ProductForReadDto> SearchProductsByName(string name)
    {
        var products = _productRepository.SearchProductsByName(name);
        return products.Select(p => new ProductForReadDto
        {
            Id = p.Id,
            Name = p.Name,
        }).ToList();
    }

    public ProductStatsDto GetStats()
    {
        var products = _productRepository.GetAllProducts();
        
        if (products == null || !products.Any())
        {
            return new ProductStatsDto
            {
                Total = 0,
                AveragePrice = 0,
                MostExpensiveName = "Sin productos"
            };
        }
        
        int total = products.Count;
        decimal averagePrice = products.Average(p => p.Price); 
        string mostExpensiveName = products
            .OrderByDescending(p => p.Price)
            .First()
            .Name;

        return new ProductStatsDto
        {
            Total = total,
            AveragePrice = averagePrice,
            MostExpensiveName = mostExpensiveName
        };
    }
    
    public bool ProductExistsWithName(string name)
    {
        var allProducts = _productRepository.GetAllProducts();
        return allProducts.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}