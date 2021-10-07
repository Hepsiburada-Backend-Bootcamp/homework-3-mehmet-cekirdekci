using Application.Models.SupplierModels;
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
    public class SupplierService_Test
    {
        [Fact]
        public void Add_Return_Response()
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            var supplierDTOList = GetAllSupplierDTOs();
            int supplierDTOListCount = supplierDTOList.Count;
            SupplierCreateDTO supplierCreateDTO = new SupplierCreateDTO
            {
                CompanyName = $"{supplierDTOListCount} Name",
                ContactName = $"{supplierDTOListCount} Name"
            };
            supplierServiceMock.Setup(service => service.Add(It.IsAny<SupplierCreateDTO>())).Returns(() =>
            {
                SupplierDTO supplierDTO = new SupplierDTO
                {
                    SupplierId = supplierDTOListCount,
                    ContactName = supplierCreateDTO.ContactName,
                    CompanyName = supplierCreateDTO.CompanyName
                };
                supplierDTOList.Add(supplierDTO);
                return new SuccessResponse(Messages.SupplierAdded);
            });
            ISupplierService supplierService = supplierServiceMock.Object;


            //Act
            supplierService.Add(supplierCreateDTO);

            //Assert
            Assert.True(supplierDTOListCount < supplierDTOList.Count);
            Assert.NotNull(supplierCreateDTO);
            Assert.True(!string.IsNullOrWhiteSpace(supplierCreateDTO.CompanyName));
            Assert.True(!string.IsNullOrWhiteSpace(supplierCreateDTO.ContactName));

        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            SupplierCreateDTO supplierCreateDTO = new SupplierCreateDTO();
            supplierServiceMock.Setup(service => service.Add(It.IsAny<SupplierCreateDTO>())).Callback(() =>
            {
                if (supplierCreateDTO.ContactName == null)
                {
                    throw new ApplicationException("Supplier dont added.");
                }
                else
                {
                    return;
                }
            });
            ISupplierService supplierService = supplierServiceMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => supplierService.Add(supplierCreateDTO));
        }

        [Fact]
        public void Delete_Return_Response()
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            var suppliertDTOList = GetAllSupplierDTOs();
            int supplierDTOCount = suppliertDTOList.Count;
            supplierServiceMock.Setup(service => service.Delete(It.IsAny<int>())).Callback((int id) =>
            {
                var supplierDTO = suppliertDTOList.FirstOrDefault(x => x.SupplierId == id);
                suppliertDTOList.Remove(supplierDTO);
            });
            ISupplierService supplierService = supplierServiceMock.Object;

            //Act
            supplierService.Delete(2);

            //Assert
            Assert.True(supplierDTOCount > suppliertDTOList.Count);
        }

        [Fact]
        public void GetAll_Return_Response()
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            var suppliertDTOList = GetAllSupplierDTOs();
            supplierServiceMock.Setup(service => service.GetAll()).Returns(() =>
            {
                return new SuccessDataResponse<List<SupplierDTO>>(suppliertDTOList);
            });
            ISupplierService supplierService = supplierServiceMock.Object;
            var supplierDTO = suppliertDTOList[0];

            //Act
            var result = supplierService.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(suppliertDTOList.Count, result.Data.Count);
            Assert.True(!string.IsNullOrWhiteSpace(supplierDTO.SupplierId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(supplierDTO.ContactName));
            Assert.True(!string.IsNullOrWhiteSpace(supplierDTO.CompanyName));
        }

        [Fact]
        public void GetById_Return_Response()
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            var suppliertDTOList = GetAllSupplierDTOs();
            supplierServiceMock.Setup(service => service.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var supplierDTO = suppliertDTOList.FirstOrDefault(x => x.SupplierId == id);
                return new SuccessDataResponse<SupplierDTO>(supplierDTO);
            });
            ISupplierService supplierService = supplierServiceMock.Object;

            //Act
            var result = supplierService.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.SupplierId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.ContactName));
            Assert.True(!string.IsNullOrWhiteSpace(result.Data.CompanyName));
        }

        [Theory]
        [InlineData(2)]
        public void Update_Return_Response(int id)
        {
            //Arrange
            var supplierServiceMock = new Mock<ISupplierService>();
            var supplierDTOList = GetAllSupplierDTOs();
            SupplierUpdateDTO supplierUpdateDTO = new SupplierUpdateDTO
            {
                
                SupplierId = id + 1,
                CompanyName = $"{id} Name",
                ContactName = $"{id} Name"
            };
            supplierServiceMock.Setup(service => service.Update(It.IsAny<SupplierUpdateDTO>(), It.IsAny<int>())).Returns(() =>
            {
                return new SuccessResponse(Messages.SupplierAdded);
            });
            SupplierDTO supplierDTO = new SupplierDTO
            {
                SupplierId = supplierUpdateDTO.SupplierId,
                ContactName = supplierUpdateDTO.ContactName,
                CompanyName = supplierUpdateDTO.CompanyName
            };
            var updatedSupplier = supplierDTOList.FirstOrDefault(x => x.SupplierId == supplierUpdateDTO.SupplierId);
            updatedSupplier = supplierDTO;
            ISupplierService supplierService = supplierServiceMock.Object;

            //Act
            supplierService.Update(supplierUpdateDTO, 2);

            //Assert
            Assert.Same(supplierDTO, updatedSupplier);
        }


        //The supplierDTOs list which i created to this unit test.
        private List<SupplierDTO> GetAllSupplierDTOs()
        {
            List<SupplierDTO> supplierDTOs = new List<SupplierDTO>();
            for (int i = 0; i < 5; i++)
            {
                SupplierDTO supplierDTO = new SupplierDTO();
                supplierDTO.SupplierId = i;
                supplierDTO.ContactName = $"{i} Name";
                supplierDTO.CompanyName = $"{i} Name";

                supplierDTOs.Add(supplierDTO);
            }
            return supplierDTOs;
        }
    }
}
