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
    public class SupplierRepository_Test
    {
        [Fact]
        public void GetList_Return_SupplierList()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var supplierList = GetAllSuppliers();
            supplierRepositoryMock.Setup(repository => repository.GetList()).Returns(supplierList);
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;
            var supplier = supplierList[0];

            //Act
            var result = supplierRepository.GetList();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(supplierList.Count, result.Count);
            Assert.True(!string.IsNullOrWhiteSpace(supplier.SupplierId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(supplier.ContactName));
            Assert.True(!string.IsNullOrWhiteSpace(supplier.CompanyName));
        }

        [Fact]
        public void GetList_ThrowException()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            List<Supplier> supplierList = new();
            supplierRepositoryMock.Setup(repository => repository.GetList()).Returns(() =>
            {
                if (supplierList.Count == 0)
                {
                    throw new ApplicationException("Supplier not found");
                }
                else
                {
                    return supplierList;
                }
            });
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;


            //Assert
            Assert.Throws<ApplicationException>(() => supplierRepository.GetList());


        }

        [Fact]
        public void GetById_Return_Supplier()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var supplierList = GetAllSuppliers();
            supplierRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var supplier = supplierList.FirstOrDefault(x => x.SupplierId == id);
                return supplier;
            });
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Act
            var result = supplierRepository.GetById(3);

            //Assert
            Assert.NotNull(result);
            Assert.True(!string.IsNullOrWhiteSpace(result.SupplierId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(result.ContactName));
            Assert.True(!string.IsNullOrWhiteSpace(result.CompanyName));
        }

        [Fact]
        public void GetById_ThrowException()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var supplierList = GetAllSuppliers();
            supplierRepositoryMock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns((int id) =>
            {
                var supplier = supplierList.FirstOrDefault(x => x.SupplierId == id);

                if (supplier == null)
                {
                    throw new ApplicationException("not found");
                }
                else
                {
                    return supplier;
                }
            });
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;



            //Assert
            Assert.Throws<ApplicationException>(() => supplierRepository.GetById(supplierList.Count + 1));

        }

        [Fact]
        public void Add()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var supplierList = GetAllSuppliers();
            int supplierListCount = supplierList.Count;
            supplierRepositoryMock.Setup(repository => repository.Add(It.IsAny<Supplier>()));
            Supplier supplier = new Supplier
            {
                SupplierId = supplierListCount,
                CompanyName = $"{supplierListCount} Name",
                ContactName = $"{supplierListCount} Name"
            };
            supplierList.Add(supplier);
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Act
            supplierRepository.Add(supplier);

            //Assert
            Assert.True(supplierListCount < supplierList.Count);
            Assert.NotNull(supplier);
            Assert.True(!string.IsNullOrWhiteSpace(supplier.SupplierId.ToString()));
            Assert.True(!string.IsNullOrWhiteSpace(supplier.ContactName));
            Assert.True(!string.IsNullOrWhiteSpace(supplier.CompanyName));

        }

        [Fact]
        public void Add_ThrowException()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();

            Supplier supplier = new Supplier();
            supplierRepositoryMock.Setup(repository => repository.Add(It.IsAny<Supplier>())).Callback(() =>
            {
                if (supplier.CompanyName == null)
                {
                    throw new ApplicationException("Supplier could not added.");
                }
                else
                {
                    return;
                }
            });
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => supplierRepository.Add(supplier));

        }

        [Fact]
        public void Delete()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var list = GetAllSuppliers();
            int supplierCount = list.Count;
            supplierRepositoryMock.Setup(repository => repository.Delete(It.IsAny<int>())).Callback((int id) =>
            {

                var supplier = list.FirstOrDefault(x => x.SupplierId == id);
                list.Remove(supplier);


            });

            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Act
            supplierRepository.Delete(2);

            //Assert
            Assert.True(supplierCount > list.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Update(int id)
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            var supplierList = GetAllSuppliers();
            supplierRepositoryMock.Setup(repository => repository.Update(It.IsAny<Supplier>()));
            Supplier supplier = new Supplier
            {
                SupplierId = id + 1,
                CompanyName = $"{id} Name",
                ContactName = $"{id} Name"
            };
            var updatedSupplier = supplierList.FirstOrDefault(x => x.SupplierId == supplier.SupplierId);
            updatedSupplier = supplier;
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Act
            supplierRepository.Update(supplier);

            //Assert
            Assert.Same(supplier, updatedSupplier);

        }

        [Fact]
        public void Update_ThrowException()
        {
            //Arrange
            var supplierRepositoryMock = new Mock<ISupplierRepository>();
            Supplier supplier = new Supplier();
            supplierRepositoryMock.Setup(repository => repository.Update(It.IsAny<Supplier>())).Callback(() =>
            {
                if (supplier.ContactName == null)
                {
                    throw new ApplicationException("Supplier could not be updated.");
                }
                else
                {
                    return;
                }
            });
            ISupplierRepository supplierRepository = supplierRepositoryMock.Object;

            //Assert
            Assert.Throws<ApplicationException>(() => supplierRepository.Update(supplier));
        }

        private List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();
            for (int i = 0; i < 5; i++)
            {
                Supplier supplier = new Supplier();
                supplier.SupplierId = i;
                supplier.ContactName = $"{i} Name";
                supplier.CompanyName = $"{i} Name";
                suppliers.Add(supplier);
            }
            return suppliers;
        }
    }
}
