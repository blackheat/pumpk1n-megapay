using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Products;
using pumpk1n_backend.Models.ReturnModels.Inventories;
using pumpk1n_backend.Models.TransferModels.Inventories;

namespace pumpk1n_backend.Services.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public InventoryService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryReturnModel> ImportProduct(InventoryImportModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var item = _mapper.Map<InventoryImportModel, ProductInventory>(model);

                    _context.ProductInventories.Add(item);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<ProductInventory, InventoryReturnModel>(item);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<CustomList<InventoryReturnModel>> GetInventory(int startAt, int count)
        {
            var items = await _context.ProductInventories.Skip(startAt).Take(count)
                .OrderByDescending(i => i.ImportedDate).ToListAsync();
            var itemReturnModels = _mapper.Map<List<ProductInventory>, CustomList<InventoryReturnModel>>(items);
            return itemReturnModels;
        }
    }
}