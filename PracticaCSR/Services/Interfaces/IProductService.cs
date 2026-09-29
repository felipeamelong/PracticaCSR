using PracticaCSR.Models.DTOs.Reponses;
using PracticaCSR.Models.DTOs.Requests;
namespace PracticaCSR.Services.Interfaces;

public interface IProductService
{
    List<ProductForReadDto> GetAllProducts();
    ProductForReadDto? GetProductById(int id);
    ProductForReadDto CreateProduct(ProductForCreateDto dto);
    void UpdateProduct(int id, ProductForUpdateDto dto);
    void DeleteProduct(int id);
}