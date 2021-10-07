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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NorthwindContext _northwindContext;

        public CategoryRepository(NorthwindContext northwindContext)
        {
            _northwindContext = northwindContext;
        }
        public void Add(Category category)
        {
            var addedCategory = _northwindContext.Add(category);

            if (addedCategory == null)
            {
                throw new ApplicationException("Category could not added.");
            }
            else
            {
                _northwindContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            Category category = GetById(id);

            _northwindContext.Remove(category);

            _northwindContext.SaveChanges();
        }

        public Category GetById(int id)
        {
            var category = _northwindContext.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                throw new ApplicationException("Category not found");
            }
            else
            {
                return category;
            }
        }

        public List<Category> GetList()
        {
            var categories = _northwindContext.Categories.ToList();

            if (categories == null)
            {
                throw new ApplicationException("Categories not found");
            }
            else
            {
                return categories;
            }
            
        }

        public void Update(Category category)
        {
            var updatedCategory = _northwindContext.Update(category);

            if (updatedCategory == null)
            {
                throw new ApplicationException("Category could not updated.");
            }
            else
            {
                _northwindContext.SaveChanges();
            }
        }
    }
}
