using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
        List<Category> GetList();
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}
