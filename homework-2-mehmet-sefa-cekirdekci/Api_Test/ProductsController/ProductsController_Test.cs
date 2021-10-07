using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api_Test.ProductsController
{
    public class ProductsController_Test : IClassFixture<ApiFactory>
    {
        private readonly WebApplicationFactory<TestStartup> _factory;

        public ProductsController_Test(ApiFactory apiFactory)
        {
            _factory = apiFactory;
        }
    }
}
