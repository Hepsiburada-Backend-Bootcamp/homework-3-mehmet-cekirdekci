using Application.Models.CategoryModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api_Test.CategoriesController
{
    public class CategoriesController_Test : IClassFixture<ApiFactory>
    {
        private readonly WebApplicationFactory<TestStartup> _factory;

        public CategoriesController_Test(ApiFactory apiFactory)
        {
            _factory = apiFactory;
        }

        [Fact]
        public async Task Post_Should_Return_Fail_With_Error_Response_When_Insert_CategoryName_Is_Empty()
        {
            //Arrange
            var category = new CategoryDTO { CategoryName = string.Empty };

            var json = JsonSerializer.Serialize(category);

            var client = _factory.CreateClient();

            var content = new StringContent(json, Encoding.UTF8, "application/json");


            //Act
            var response = await client.PostAsync("api/v1/categories", content);

            var actualStatusCode = response.StatusCode;

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, actualStatusCode);
        }
    }
}
