using Application.Models.CategoryModels;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICategoryService
    {
        
        IDataResponse<CategoryDTO> GetById(int categoryId);
        IDataResponse<List<CategoryDTO>> GetAll();
        IResponse Add(CategoryCreateDTO categoryCreateDTO);
        IResponse Update(CategoryUpdateDTO categoryUpdateDTO, int id);
        IResponse Delete(int categoryId);
    }
}
