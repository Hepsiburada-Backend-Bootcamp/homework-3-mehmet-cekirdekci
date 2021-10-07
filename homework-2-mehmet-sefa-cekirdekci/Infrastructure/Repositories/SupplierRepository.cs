using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly NorthwindContext _northwindContext;

        public SupplierRepository(NorthwindContext northwindContext)
        {
            _northwindContext = northwindContext;
        }
        public void Add(Supplier supplier)
        {
            var addedSupplier = _northwindContext.Add(supplier);

            if (addedSupplier == null)
            {
                throw new ApplicationException("Supplier could not added.");
            }
            else
            {
                _northwindContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Supplier supplier = GetById(id);

            _northwindContext.Remove(supplier);

            _northwindContext.SaveChanges();
        }

        public Supplier GetById(int id)
        {
            var supplier = _northwindContext.Suppliers.FirstOrDefault(s => s.SupplierId == id);

            if (supplier == null)
            {
                throw new ApplicationException("Supplier not found");
            }
            else
            {
                return supplier;
            }
        }

        public List<Supplier> GetList()
        {
            var suppliers = _northwindContext.Suppliers.ToList();

            if (suppliers == null)
            {
                throw new ApplicationException("Suppliers not found");
            }
            else
            {
                return suppliers;
            }
        }

        public void Update(Supplier supplier)
        {
            var updatedSupplier = _northwindContext.Update(supplier);

            if (updatedSupplier == null)
            {
                throw new ApplicationException("Supplier could not updated.");
            }
            else
            {
                _northwindContext.SaveChanges();
            }
        }
    }
}
