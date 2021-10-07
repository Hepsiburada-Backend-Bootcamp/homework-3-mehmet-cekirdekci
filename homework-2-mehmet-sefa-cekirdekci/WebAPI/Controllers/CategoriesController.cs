using Application.Models.CategoryModels;
using Application.Services;
using AutoMapper;
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
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _categoryService.GetAll();

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
            var result = _categoryService.GetById(id);

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
        public IActionResult Add([FromBody] CategoryCreateDTO categoryCreateDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var category = _mapper.Map<CategoryCreateDTO>(categoryCreateDTO);
                _categoryService.Add(category);
                return Created("Category Added", category);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            else
            {
                category.Data.CategoryName = categoryUpdateDTO.CategoryName;

                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);

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
