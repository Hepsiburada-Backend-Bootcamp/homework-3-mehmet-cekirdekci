using Application.Models.ProductModels;
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
    public class ProductService_Test
    {
        [Fact]
        public void Add_Return_Response()
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            var productDTOList = GetAllProductDTOs();
            int productDTOListCount = productDTOList.Count;
            ProductCreateDTO productCreateDTO = new ProductCreateDTO
            {
                ProductName = $"{productDTOListCount} Name",
                CategoryId = productDTOListCount,
                SupplierId = productDTOListCount
            };
            productServiceMock.Setup(service => service.Add(It.IsAny<ProductCreateDTO>())).Returns(() => 
            {
                ProductDTO productDTO = new ProductDTO
                {
                    ProductId = productDTOListCount,
                    ProductName = productCreateDTO.ProductName,
                    CategoryId = productCreateDTO.CategoryId,
                    SupplierId = productCreateDTO.SupplierId
                };
                productDTOList.Add(productDTO);
                return new SuccessResponse(Messages.ProductAdded);
            });
            IProductService productService = productServiceMock.Object;


            //Act
            productService.Add(productCreateDTO);

            //Assert
            Assert.True(productDTOListCount < productDTOList.Count);
            Assert.NotNull(productCreateDTO);
            Assert.True(!string.IsNullOrWhiteSpace(productCreateDTO.ProductName));

        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            ProductCreateDTO productCreateDTO = new ProductCreateDTO();
            productServiceMock.Setup(service => service.Add(It.IsAny<ProductCreateDTO>())).Callback(() =>
            {
                if (productCreateDTO.ProductName == null)
                {
                    throw new ApplicationException("Product dont added.");
                }
                else
                {
                    return;
                }
            });
            IProductService productService = productServiceMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => productService.Add(productCreateDTO));
        }

        [Fact]
        public void Delete_Return_Response()
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            var productDTOList = GetAllProductDTOs();
            int productDTOCount = productDTOList.Count;
            productServiceMock.Setup(service => service.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var productDTO = productDTOList.FirstOrDefault(x => x.ProductId == id);
                productDTOList.Remove(productDTO);
            });
            IProductService productService = productServiceMock.Object;

            //Act
            productService.Delete(2);

            //Assert
            Assert.True(productDTOCount > productDTOList.Count);
        }

        [Fact]
        public void GetAll_Return_Response()
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            var productDTOList = GetAllProductDTOs();
            productServiceMock.Setup(service => service.GetAll()).Returns(() => 
            {
                return new SuccessDataResponse<List<ProductDTO>>(productDTOList);
            });
            IProductService productService = productServiceMock.Object;
            var productDTO = productDTOList[0];

            //Act
            var result = productService.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(productDTOList.Count, result.Data.Count);
            Assert.True(!string.IsNullOrWhiteSpace(productDTO.ProductId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(productDTO.ProductName));
        }

        [Fact]
        public void GetById_Return_Response()
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            var productDTOList = GetAllProductDTOs();
            productServiceMock.Setup(service => service.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var productDTO = productDTOList.FirstOrDefault(x => x.ProductId == id);
                return new SuccessDataResponse<ProductDTO>(productDTO);
            });
            IProductService productService = productServiceMock.Object;

            //Act
            var result = productService.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.ProductId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.ProductName));
        }

        [Theory]
        [InlineData(2)]
        public void Update_Return_Response(int id)
        {
            //Arrange
            var productServiceMock = new Mock<IProductService>();
            var productDTOList = GetAllProductDTOs();
            ProductUpdateDTO productUpdateDTO = new ProductUpdateDTO
            {
                ProductId = id,
                ProductName = $"{id} Name",
                CategoryId = id + 1,
                SupplierId = id + 2
            };
            productServiceMock.Setup(service => service.Update(It.IsAny<ProductUpdateDTO>(), It.IsAny<int>())).Returns(() => 
            {
                return new SuccessResponse(Messages.ProductUpdated);
            });
            ProductDTO productDTO = new ProductDTO
            {
                ProductId = productUpdateDTO.ProductId,
                ProductName = productUpdateDTO.ProductName,
                CategoryId = productUpdateDTO.CategoryId,
                SupplierId = productUpdateDTO.SupplierId
            };
            var updatedProduct = productDTOList.FirstOrDefault(x => x.ProductId == productUpdateDTO.ProductId);
            updatedProduct = productDTO;
            IProductService productService = productServiceMock.Object;

            //Act
            productService.Update(productUpdateDTO,2);

            //Assert
            Assert.Same(productDTO, updatedProduct);
        }


        //The productDTOs list which i created to this unit test.
        private List<ProductDTO> GetAllProductDTOs()
        {
            List<ProductDTO> productDTOs = new List<ProductDTO>();
            for (int i = 0; i < 5; i++)
            {
                ProductDTO productDTO = new ProductDTO();
                productDTO.ProductId = i;
                productDTO.ProductName = $"{i} Name";
                productDTO.CategoryId = i + 1;
                productDTO.SupplierId = i + 1;

                productDTOs.Add(productDTO);
            }
            return productDTOs;
        }
    }
}
