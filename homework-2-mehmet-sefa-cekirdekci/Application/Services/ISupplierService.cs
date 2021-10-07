using Application.Models.SupplierModels;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ISupplierService
    {
        IDataResponse<SupplierDTO> GetById(int supplierId);
        IDataResponse<List<SupplierDTO>> GetAll();
        IResponse Add(SupplierCreateDTO supplierCreateDTO);
        IResponse Update(SupplierUpdateDTO supplierUpdateDTO, int id);
        IResponse Delete(int supplierId);
    }
}
