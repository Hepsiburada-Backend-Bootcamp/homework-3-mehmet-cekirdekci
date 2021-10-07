using Application.Models.ProductModels;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IProductService
    {
        IDataResponse<ProductDTO> GetById(int productId);
        IDataResponse<List<ProductDTO>> GetAll();
        IResponse Add(ProductCreateDTO productCreateDTO);
        IResponse Update(ProductUpdateDTO productUpdateDTO, int id);
        IResponse Delete(int productId);
    }
}
