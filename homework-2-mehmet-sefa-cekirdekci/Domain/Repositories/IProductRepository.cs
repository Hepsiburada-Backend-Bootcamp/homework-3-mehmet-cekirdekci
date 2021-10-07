using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Product GetById(int id);
        List<Product> GetList();
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        
    }
}
