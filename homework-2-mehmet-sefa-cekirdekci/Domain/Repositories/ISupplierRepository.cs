using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ISupplierRepository
    {
        Supplier GetById(int id);
        List<Supplier> GetList();
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int id);
    }
}
