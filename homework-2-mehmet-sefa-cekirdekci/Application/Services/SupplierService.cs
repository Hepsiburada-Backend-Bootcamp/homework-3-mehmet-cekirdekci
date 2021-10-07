using Application.Models.SupplierModels;
using Application.Responses;
using Application.ServiceMessages;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public IResponse Add(SupplierCreateDTO supplierCreateDTO)
        {
            var supplier = _mapper.Map<Supplier>(supplierCreateDTO);

            if (supplier == null)
            {
                return new ErrorResponse(Messages.SupplierDontAdded);
            }
            _supplierRepository.Add(supplier);

            return new SuccessResponse(Messages.SupplierAdded);
        }

        public IResponse Delete(int supplierId)
        {
            _supplierRepository.Delete(supplierId);

            return new SuccessResponse(Messages.SupplierDeleted);
        }

        public IDataResponse<List<SupplierDTO>> GetAll()
        {
            var suppliers = _supplierRepository.GetList();
            var dtos = _mapper.Map<List<SupplierDTO>>(suppliers);
            return new SuccessDataResponse<List<SupplierDTO>>(dtos);
        }

        public IDataResponse<SupplierDTO> GetById(int supplierId)
        {
            var supplier = _supplierRepository.GetById(supplierId);
            var mappedSupplier = _mapper.Map<SupplierDTO>(supplier);

            return new SuccessDataResponse<SupplierDTO>(mappedSupplier);
        }

        public IResponse Update(SupplierUpdateDTO supplierUpdateDTO, int id)
        {
            var mappedSupplier = _mapper.Map<Supplier>(supplierUpdateDTO);
            var updatedSupplier = _supplierRepository.GetById(id);

            updatedSupplier.SupplierId = mappedSupplier.SupplierId;
            updatedSupplier.CompanyName = mappedSupplier.CompanyName;
            updatedSupplier.ContactName = mappedSupplier.ContactName;
            _supplierRepository.Update(updatedSupplier);

            return new SuccessResponse(Messages.SupplierUpdated);
        }
    }
}
