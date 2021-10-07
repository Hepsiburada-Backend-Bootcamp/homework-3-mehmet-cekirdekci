using Domain.Entities;
using Domain.Repositories;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain_Test.Repository_Tests
{
    public class ProductRepository_Test
    {
        [Fact]
        public void GetList_Return_ProductList()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productList = GetAllProducts();
            productRepositoryMock.Setup(repository => repository.GetList()).Returns(productList);
            IProductRepository productRepository = productRepositoryMock.Object;
            var product = productList[0];

            //Act
            var result = productRepository.GetList();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(productList.Count, result.Count);
            Assert.True(!string.IsNullOrWhiteSpace(product.ProductId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(product.ProductName));
        }

        [Fact]
        public void GetList_ThrowException()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            List<Product> productList = new();
            productRepositoryMock.Setup(repository => repository.GetList()).Returns(() => 
            {
                if (productList.Count == 0)
                {
                    throw new ApplicationException("Products not found");
                }
                else
                {
                    return productList;
                }
            });
            IProductRepository productRepository = productRepositoryMock.Object;


            //Assert
            Assert.Throws<ApplicationException>(() => productRepository.GetList());


        }

        [Fact]
        public void GetById_Return_Product()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productList = GetAllProducts();
            productRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var product = productList.FirstOrDefault(x => x.ProductId == id);
                return product;
            });
            IProductRepository productRepository = productRepositoryMock.Object;

            //Act
            var result = productRepository.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.ProductId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.ProductName));
        }

        [Fact]
        public void GetById_ThrowException()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productList = GetAllProducts();
            productRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var product = productList.FirstOrDefault(x => x.ProductId == id);

                if (product == null)
                {
                    throw new ApplicationException("not found");
                }
                else
                {
                    return product;
                }
            });
            IProductRepository productRepository = productRepositoryMock.Object;



            //Assert
            Assert.Throws<ApplicationException>(() => productRepository.GetById(productList.Count + 1));

        }

        [Fact]
        public void Add()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productList = GetAllProducts();
            int productListCount = productList.Count;
            productRepositoryMock.Setup(repository => repository.Add(It.IsAny<Product>()));
            Product product = new Product
            {
                ProductId = productListCount,
                ProductName = $"{productListCount} Name",
                CategoryId = productListCount,
                SupplierId = productListCount
            };
            productList.Add(product);
            IProductRepository productRepository = productRepositoryMock.Object;

            //Act
            productRepository.Add(product);

            //Assert
            Assert.True(productListCount < productList.Count);
            Assert.NotNull(product);
            Assert.True(!string.IsNullOrWhiteSpace(product.ProductId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(product.ProductName));
            
        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            
            Product product = new Product();
            productRepositoryMock.Setup(repository => repository.Add(It.IsAny<Product>())).Callback(() =>
            {
                if (product.ProductName == null)
                {
                    throw new ApplicationException("Product could not added.");
                }
                else
                {
                    return;
                }
            });
            IProductRepository productRepository = productRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => productRepository.Add(product));
            
        }

        [Fact]
        public void Delete()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var list = GetAllProducts();
            int productCount = list.Count;
            productRepositoryMock.Setup(repository => repository.Delete(It.IsAny<int>())).Callback((int id) =>
            {

                var product = list.FirstOrDefault(x => x.ProductId == id);
                list.Remove(product);


            });

            IProductRepository productRepository = productRepositoryMock.Object;

            //Act
            productRepository.Delete(2);

            //Assert
            Assert.True(productCount > list.Count);
        }

       [Theory]
       [InlineData(2)]
       [InlineData(3)]
        public void Update(int id)
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var productList = GetAllProducts();
            productRepositoryMock.Setup(repository => repository.Update(It.IsAny<Product>()));
            Product product = new Product
            {
                ProductId = id,
                ProductName = $"{id} Name",
                CategoryId = id + 1,
                SupplierId = id + 2
            };
            var updatedProduct = productList.FirstOrDefault(x => x.ProductId == product.ProductId);
            updatedProduct = product;
            IProductRepository productRepository = productRepositoryMock.Object;

            //Act
            productRepository.Update(product);

            //Assert
            Assert.Same(product, updatedProduct);

        }

        [Fact]
        public void Update_ThrowException()
        {
            //Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            Product product = new Product();
            productRepositoryMock.Setup(repository => repository.Update(It.IsAny<Product>())).Callback(() =>
            {
                if (product.ProductName == null)
                {
                    throw new ApplicationException("Product could not be updated.");
                }
                else
                {
                    return;
                }  
            });
            IProductRepository productRepository = productRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => productRepository.Update(product));
        }

        //The products list which i created to this unit test.
        private List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            for (int i = 0; i < 5; i++)
            {
                Product product = new Product();
                product.ProductId = i;
                product.ProductName = $"{i} Name";
                product.CategoryId = i + 1;
                product.SupplierId = i + 1;

                products.Add(product);
            }
            return products;
        }
    }
}
