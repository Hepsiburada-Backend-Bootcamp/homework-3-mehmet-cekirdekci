using Application.Models.CategoryModels;
using Application.Responses;
using Application.ServiceMessages;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IResponse Add(CategoryCreateDTO categoryCreateDTO)
        {
            var category = _mapper.Map<Category>(categoryCreateDTO);

            if (category.CategoryName == null)
            {
                return new ErrorResponse(Messages.CategoryDontAdded);
            }

            else
            {
                _categoryRepository.Add(category);

                return new SuccessResponse(Messages.CategoryAdded);
            }

            
        }

        public IResponse Delete(int categoryId)
        {
            _categoryRepository.Delete(categoryId);

            return new SuccessResponse(Messages.CategoryDeleted);
        }

        public IDataResponse<List<CategoryDTO>> GetAll()
        {
            var categories = _categoryRepository.GetList();
            var dtos = _mapper.Map<List<CategoryDTO>>(categories);
            return new SuccessDataResponse<List<CategoryDTO>>(dtos);
        }

        public IDataResponse<CategoryDTO> GetById(int categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);
            var mappedCategory = _mapper.Map<CategoryDTO>(category);

            return new SuccessDataResponse<CategoryDTO>(mappedCategory);
        }

        public IResponse Update(CategoryUpdateDTO categoryUpdateDTO, int id)
        {
            var mappedCategory = _mapper.Map<Category>(categoryUpdateDTO);
            var updatedCategory = _categoryRepository.GetById(id);

            updatedCategory.CategoryId = mappedCategory.CategoryId;
            updatedCategory.CategoryName = mappedCategory.CategoryName;
            _categoryRepository.Update(updatedCategory);

            return new SuccessResponse(Messages.CategoryUpdated);
        }
    }
}
