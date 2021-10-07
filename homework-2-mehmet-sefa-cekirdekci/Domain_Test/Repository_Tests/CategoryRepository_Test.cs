using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain_Test.Repository_Tests
{
    public class CategoryRepository_Test
    {
        [Fact]
        public void GetList_Return_CategoryList()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoryList = GetAllCategories();
            categoryRepositoryMock.Setup(repository => repository.GetList()).Returns(categoryList);
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;
            var category = categoryList[0];

            //Act
            var result = categoryRepository.GetList();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(categoryList.Count, result.Count);
            Assert.True(!string.IsNullOrWhiteSpace(category.CategoryId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(category.CategoryName));
        }

        [Fact]
        public void GetList_ThrowException()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            List<Category> categoryList = new();
            categoryRepositoryMock.Setup(repository => repository.GetList()).Returns(() =>
            {
                if (categoryList.Count == 0)
                {
                    throw new ApplicationException("Categories not found");
                }
                else
                {
                    return categoryList;
                }
            });
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;


            //Assert
            Assert.Throws<ApplicationException>(() => categoryRepository.GetList());


        }

        [Fact]
        public void GetById_Return_Category()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoryList = GetAllCategories();
            categoryRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var category = categoryList.FirstOrDefault(x => x.CategoryId == id);
                return category;
            });
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Act
            var result = categoryRepository.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.CategoryId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.CategoryName));
        }

        [Fact]
        public void GetById_ThrowException()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoryList = GetAllCategories();
            categoryRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var category = categoryList.FirstOrDefault(x => x.CategoryId == id);

                if (category == null)
                {
                    throw new ApplicationException("not found");
                }
                else
                {
                    return category;
                }
            });
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;



            //Assert
            Assert.Throws<ApplicationException>(() => categoryRepository.GetById(categoryList.Count + 1));

        }

        [Fact]
        public void Add()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoryList = GetAllCategories();
            int categoryListCount = categoryList.Count;
            categoryRepositoryMock.Setup(repository => repository.Add(It.IsAny<Category>()));
            Category category = new Category
            {
                CategoryId = categoryListCount,
                CategoryName = $"{categoryListCount} Name"
            };
            categoryList.Add(category);
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Act
            categoryRepository.Add(category);

            //Assert
            Assert.True(categoryListCount < categoryList.Count);
            Assert.NotNull(category);
            Assert.True(!string.IsNullOrWhiteSpace(category.CategoryId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(category.CategoryName));

        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            Category category = new Category();
            categoryRepositoryMock.Setup(repository => repository.Add(It.IsAny<Category>())).Callback(() =>
            {
                if (category.CategoryName == null)
                {
                    throw new ApplicationException("Categories could not added.");
                }
                else
                {
                    return;
                }
            });
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => categoryRepository.Add(category));

        }

        [Fact]
        public void Delete()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var list = GetAllCategories();
            int categoryCount = list.Count;
            categoryRepositoryMock.Setup(repository => repository.Delete(It.IsAny<int>())).Callback((int id) =>
            {

                var category = list.FirstOrDefault(x => x.CategoryId == id);
                list.Remove(category);


            });

            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Act
            categoryRepository.Delete(2);

            //Assert
            Assert.True(categoryCount > list.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Update(int id)
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            var categoryList = GetAllCategories();
            categoryRepositoryMock.Setup(repository => repository.Update(It.IsAny<Category>()));
            Category category = new Category
            {
                CategoryId = id + 1,
                CategoryName = $"{id} Name"
            };
            var updatedCategory = categoryList.FirstOrDefault(x => x.CategoryId == category.CategoryId);
            updatedCategory = category;
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Act
            categoryRepository.Update(category);

            //Assert
            Assert.Same(category, updatedCategory);

        }

        [Fact]
        public void Update_ThrowException()
        {
            //Arrange
            var categoryRepositoryMock = new Mock<ICategoryRepository>();
            Category category = new Category();
            categoryRepositoryMock.Setup(repository => repository.Update(It.IsAny<Category>())).Callback(() =>
            {
                if (category.CategoryName == null)
                {
                    throw new ApplicationException("Category could not be updated.");
                }
                else
                {
                    return;
                }
            });
            ICategoryRepository categoryRepository = categoryRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => categoryRepository.Update(category));
        }

        //The categories list which i created to this unit test.
        private List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            for (int i = 0; i < 5; i++)
            {
                Category category = new Category();
                category.CategoryId = i;
                category.CategoryName = $"{i} Name";

                categories.Add(category);
            }
            return categories;
        }
    }
}
