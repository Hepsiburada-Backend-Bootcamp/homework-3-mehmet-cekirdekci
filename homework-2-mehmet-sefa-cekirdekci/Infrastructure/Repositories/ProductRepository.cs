using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NorthwindContext _northwindContext;
        
        public ProductRepository(NorthwindContext northwindContext)
        {
            _northwindContext = northwindContext;
        }

        public void Add(Product product)
        {
            var addedProduct = _northwindContext.Add(product);


            if (addedProduct == null)
            {
                throw new ApplicationException("Product could not added.");
            }
            else
            {
                _northwindContext.SaveChanges();
            }
        }


        public Product GetById(int id)
        {
            var product = _northwindContext.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                throw new ApplicationException("Product not found");
            }
            else
            {
                return product;
            }
        }

        public List<Product> GetList()
        {
            var products = _northwindContext.Products.ToList();

            if (products == null)
            {
                throw new ApplicationException("Products not found");
            }
            else
            {
                return products;
            } 
        }

        public void Update(Product product)
        {
            var updatedProduct = _northwindContext.Update(product);

            if (updatedProduct == null)
            {
                throw new ApplicationException("Product could not updated.");
            }
            else
            {
                _northwindContext.SaveChanges();
                
            }
        }

        public void Delete(int id)
        {
            Product product = GetById(id);

            _northwindContext.Remove(product);

            _northwindContext.SaveChanges();
        }
            
    }
}
