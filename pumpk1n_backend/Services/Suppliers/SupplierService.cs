using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Exceptions.Others;
using pumpk1n_backend.Exceptions.Suppliers;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Suppliers;
using pumpk1n_backend.Models.ReturnModels.Suppliers;
using pumpk1n_backend.Models.TransferModels.Suppliers;

namespace pumpk1n_backend.Services.Suppliers
{
    public class SupplierService : ISupplierService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public SupplierService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SupplierReturnModel> AddSupplier(SupplierInsertModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var supplier = _mapper.Map<SupplierInsertModel, Supplier>(model);
                    supplier.AddedDate = DateTime.UtcNow;
                    _context.Suppliers.Add(supplier);
                    
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<Supplier, SupplierReturnModel>(supplier);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<SupplierReturnModel> UpdateSupplier(SupplierInsertModel model, long supplierId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
                    
                    if (supplier == null)
                        throw new SupplierNotFoundException();

                    _mapper.Map(model, supplier);

                    _context.Suppliers.Update(supplier);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<Supplier, SupplierReturnModel>(supplier);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteSupplier(long supplierId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
                    
                    if (supplier == null)
                        throw new SupplierNotFoundException();

                    _context.Suppliers.Remove(supplier);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<SupplierReturnModel> GetSupplier(long supplierId)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
            
            if (supplier == null)
                throw new SupplierNotFoundException();
            
            return _mapper.Map<Supplier, SupplierReturnModel>(supplier);
        }

        public async Task<CustomList<SupplierReturnModel>> GetSuppliers(int page, int count, string name = "")
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();
            
            var startAt = (page - 1) * count;
            var suppliers = await _context.Suppliers
                .Where(s => s.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(s => s.AddedDate)
                .Skip(startAt)
                .Take(count).ToListAsync();
            
            var totalCount = await _context.Suppliers
                .Where(s => s.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).CountAsync();
            var totalPages = totalCount / count + (totalCount % count > 0 ? 1 : 0);
            
            var supplierReturnModels = _mapper.Map<List<Supplier>, CustomList<SupplierReturnModel>>(suppliers);
            supplierReturnModels.CurrentPage = page;
            supplierReturnModels.TotalPages = totalPages;
            supplierReturnModels.TotalItems = totalCount;
            supplierReturnModels.IsListPartial = true;

            return supplierReturnModels;
        }
    }
}