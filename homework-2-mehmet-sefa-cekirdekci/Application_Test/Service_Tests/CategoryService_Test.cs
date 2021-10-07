using Application.Models.CategoryModels;
using Application.Responses;
using Application.ServiceMessages;
using Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application_Test.Service_Tests
{
    public class CategoryService_Test
    {
        [Fact]
        public void Add_Return_Response()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var categoryDTOList = GetAllCategoryDTOs();
            int categoryDTOListCount = categoryDTOList.Count;
            CategoryCreateDTO categoryCreateDTO = new CategoryCreateDTO
            {
                CategoryName = $"{categoryDTOListCount} Name"
            };
            categoryServiceMock.Setup(service => service.Add(It.IsAny<CategoryCreateDTO>())).Returns(() =>
            {
                CategoryDTO categoryDTO = new CategoryDTO
                {
                    CategoryId = categoryDTOListCount,
                    CategoryName = categoryCreateDTO.CategoryName

                };
                categoryDTOList.Add(categoryDTO);
                return new SuccessResponse(Messages.CategoryAdded);
            });
            ICategoryService categoryService = categoryServiceMock.Object;


            //Act
            categoryService.Add(categoryCreateDTO);

            //Assert
            Assert.True(categoryDTOListCount < categoryDTOList.Count);
            Assert.NotNull(categoryCreateDTO);
            Assert.True(!string.IsNullOrWhiteSpace(categoryCreateDTO.CategoryName));

        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            CategoryCreateDTO categoryCreateDTO = new CategoryCreateDTO();
            categoryServiceMock.Setup(service => service.Add(It.IsAny<CategoryCreateDTO>())).Callback(() =>
            {
                if (categoryCreateDTO.CategoryName == null)
                {
                    throw new ApplicationException("Category dont added.");
                }
                else
                {
                    return;
                }
            });
            ICategoryService categoryService = categoryServiceMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => categoryService.Add(categoryCreateDTO));
        }

        [Fact]
        public void Delete_Return_Response()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var categorytDTOList = GetAllCategoryDTOs();
            int categoryDTOCount = categorytDTOList.Count;
            categoryServiceMock.Setup(service => service.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var categoryDTO = categorytDTOList.FirstOrDefault(x => x.CategoryId == id);
                categorytDTOList.Remove(categoryDTO);
            });
            ICategoryService categoryService = categoryServiceMock.Object;

            //Act
            categoryService.Delete(2);

            //Assert
            Assert.True(categoryDTOCount > categorytDTOList.Count);
        }

        [Fact]
        public void GetAll_Return_Response()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var categorytDTOList = GetAllCategoryDTOs();
            categoryServiceMock.Setup(service => service.GetAll()).Returns(() =>
            {
                return new SuccessDataResponse<List<CategoryDTO>>(categorytDTOList);
            });
            ICategoryService categoryService = categoryServiceMock.Object;
            var categoryDTO = categorytDTOList[0];

            //Act
            var result = categoryService.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(categorytDTOList.Count, result.Data.Count);
            Assert.True(!string.IsNullOrWhiteSpace(categoryDTO.CategoryId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(categoryDTO.CategoryName));
        }

        [Fact]
        public void GetById_Return_Response()
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var categoryDTOList = GetAllCategoryDTOs();
            categoryServiceMock.Setup(service => service.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var categoryDTO = categoryDTOList.FirstOrDefault(x => x.CategoryId == id);
                return new SuccessDataResponse<CategoryDTO>(categoryDTO);
            });
            ICategoryService categoryService = categoryServiceMock.Object;

            //Act
            var result = categoryService.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.CategoryId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.CategoryName));
        }

        [Theory]
        [InlineData(2)]
        public void Update_Return_Response(int id)
        {
            //Arrange
            var categoryServiceMock = new Mock<ICategoryService>();
            var categoryDTOList = GetAllCategoryDTOs();
            CategoryUpdateDTO categoryUpdateDTO = new CategoryUpdateDTO
            {
                CategoryId = id + 1,
                CategoryName = $"{id} Name"
            };
            categoryServiceMock.Setup(service => service.Update(It.IsAny<CategoryUpdateDTO>(), It.IsAny<int>())).Returns(() =>
            {
                return new SuccessResponse(Messages.CategoryAdded);
            });
            CategoryDTO categoryDTO = new CategoryDTO
            {
                CategoryId = categoryUpdateDTO.CategoryId,
                CategoryName = categoryUpdateDTO.CategoryName
            };
            var updatedCategory = categoryDTOList.FirstOrDefault(x => x.CategoryId == categoryUpdateDTO.CategoryId);
            updatedCategory = categoryDTO;
            ICategoryService categoryService = categoryServiceMock.Object;

            //Act
            categoryService.Update(categoryUpdateDTO, 2);

            //Assert
            Assert.Same(categoryDTO, updatedCategory);
        }


        //The categoryDTOs list which i created to this unit test.
        private List<CategoryDTO> GetAllCategoryDTOs()
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            for (int i = 0; i < 5; i++)
            {
                CategoryDTO categoryDTO = new CategoryDTO();
                categoryDTO.CategoryId = i;
                categoryDTO.CategoryName = $"{i} Name";
                categoryDTOs.Add(categoryDTO);
            }
            return categoryDTOs;
        }
    }
}
