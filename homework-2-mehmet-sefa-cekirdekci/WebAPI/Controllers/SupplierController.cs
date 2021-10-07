using Application.Models.SupplierModels;
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
    public class SupplierController : ControllerBase
    {
        private ISupplierService _supplierService;
        private IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var result = _supplierService.GetAll();

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
            var result = _supplierService.GetById(id);

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
        public IActionResult Add([FromBody] SupplierCreateDTO supplierCreateDTO)
        {
            var supplier = _mapper.Map<SupplierCreateDTO>(supplierCreateDTO);
            var result = _supplierService.Add(supplier);

            if (result.Success)
            {
                return Created("Supplier Added", supplier);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SupplierUpdateDTO supplierUpdateDTO)
        {
            var supplier = _supplierService.GetById(id);

            if (supplier == null)
            {
                return NotFound();
            }

            else
            {
                supplier.Data.CompanyName = supplierUpdateDTO.CompanyName;
                supplier.Data.ContactName = supplierUpdateDTO.ContactName;

                return Ok();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _supplierService.Delete(id);

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
