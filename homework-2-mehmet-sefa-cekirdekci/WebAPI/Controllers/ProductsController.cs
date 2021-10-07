using Application.Models.ProductModels;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        private IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _productService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Data);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductCreateDTO productCreateDTO)
        {
            var product = _mapper.Map<ProductCreateDTO>(productCreateDTO);
            var result = _productService.Add(product);

            if (result.Success)
            {
                return Created("Product Added", product);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductUpdateDTO productUpdateDTO)
        {
            var product = _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            else
            {
                product.Data.SupplierId = productUpdateDTO.SupplierId;
                product.Data.CategoryId = productUpdateDTO.CategoryId;
                product.Data.ProductName = productUpdateDTO.ProductName;
                
                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);

            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
