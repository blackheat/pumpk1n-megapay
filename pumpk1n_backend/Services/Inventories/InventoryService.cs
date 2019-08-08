using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Exceptions.Inventories;
using pumpk1n_backend.Exceptions.Others;
using pumpk1n_backend.Exceptions.Products;
using pumpk1n_backend.Exceptions.Suppliers;
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
        private const string DefaultImageUrl =
            "https://ssl-product-images.www8-hp.com/digmedialib/prodimg/lowres/c05962484.png";

        public InventoryService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<List<InventoryReturnModel>> ImportProducts(InventoryImportModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var productUniqueIdentifiers =
                        model.InventoryItems.Select(ii => ii.ProductUniqueIdentifier).ToList();
                    if (productUniqueIdentifiers.Distinct().Count() != productUniqueIdentifiers.Count)
                        throw new InventoryItemWithSameUniqueIdentifierExistsException();

                    var inventoryItems = new List<ProductInventory>();
                    foreach (var inventoryProductItemModel in model.InventoryItems)
                    {
                        // Check existing SKU
                        var inventoryProductItem = await _context.ProductInventories.FirstOrDefaultAsync(pi =>
                            string.Equals(pi.ProductUniqueIdentifier, inventoryProductItemModel.ProductUniqueIdentifier,
                                StringComparison.InvariantCultureIgnoreCase));
                        if  (inventoryProductItem != null)
                            throw new InventoryItemWithSameUniqueIdentifierExistsException();
                        
                        // Check for existing supplier
                        var supplier =
                            await _context.Suppliers.FirstOrDefaultAsync(s =>
                                s.Id == inventoryProductItemModel.SupplierId);
                        if (supplier == null)
                            throw new SupplierNotFoundException();
                        
                        // Check for existing product
                        var product =
                            await _context.Products.FirstOrDefaultAsync(
                                p => p.Id == inventoryProductItemModel.ProductId);
                        if (product == null)
                            throw new ProductNotFoundException();
                        
                        // Add to inventory
                        var inventoryItem =
                            _mapper.Map<InventoryProductImportModel, ProductInventory>(inventoryProductItemModel);
                        inventoryItem.ImportedDate = model.ImportedDate;
                        inventoryItems.Add(inventoryItem);
                    }
                    
                    _context.ProductInventories.AddRange(inventoryItems);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    var inventoryItemsIds = inventoryItems.Select(ii => ii.Id).ToList();
                    var populatedInventoryItems =
                        await _context.ProductInventories.Where(pi => inventoryItemsIds.Contains(pi.Id)).ToListAsync();
                    var populatedInventoryItemsModels =
                        _mapper.Map<List<ProductInventory>, List<InventoryReturnModel>>(populatedInventoryItems);
                    
                    foreach (var populatedInventoryItemsModel in populatedInventoryItemsModels)
                    {
                        if (populatedInventoryItemsModel.ProductDetails == null) 
                            continue;

                        if (string.IsNullOrEmpty(populatedInventoryItemsModel.ProductDetails.Image) ||
                            string.IsNullOrWhiteSpace(populatedInventoryItemsModel.ProductDetails.Image))
                            populatedInventoryItemsModel.ProductDetails.Image = DefaultImageUrl;
                    }

                    return populatedInventoryItemsModels;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<CustomList<InventoryReturnModel>> GetInventory(int page, int count, InventoryFilterModel filterModel)
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();

            var startAt = (page - 1) * count;

            var items = await _context.ProductInventories.Skip(startAt).Take(count)
                .OrderByDescending(i => i.ImportedDate)
                .Where(i => filterModel.ProductId <= 0 ? i.ProductId > filterModel.ProductId :
                    i.ProductId == filterModel.ProductId &&
                    filterModel.SupplierId <= 0 ? i.SupplierId > filterModel.SupplierId :
                    i.SupplierId == filterModel.SupplierId &&
                    i.Product.Name.Contains(filterModel.ProductName, StringComparison.InvariantCultureIgnoreCase) &&
                    i.Supplier.Name.Contains(filterModel.SupplierName, StringComparison.InvariantCultureIgnoreCase))
                .Include(i => i.Customer)
                .Include(i => i.Supplier)
                .Include(i => i.Product)
                .ToListAsync();
            
            var totalCount = await _context.ProductInventories
                .OrderByDescending(i => i.ImportedDate)
                .Where(i => filterModel.ProductId <= 0 ? i.ProductId > filterModel.ProductId :
                    i.ProductId == filterModel.ProductId &&
                    filterModel.SupplierId <= 0 ? i.SupplierId > filterModel.SupplierId :
                    i.SupplierId == filterModel.SupplierId &&
                    i.Product.Name.Contains(filterModel.ProductName, StringComparison.InvariantCultureIgnoreCase) &&
                    i.Supplier.Name.Contains(filterModel.SupplierName, StringComparison.InvariantCultureIgnoreCase))
                .Include(i => i.Customer)
                .Include(i => i.Supplier)
                .Include(i => i.Product).CountAsync();
            var totalPages = totalCount / count + (totalCount / count > 0 ? totalCount % count : 1);

            var itemReturnModels = _mapper.Map<List<ProductInventory>, CustomList<InventoryReturnModel>>(items);
            foreach (var itemReturnModel in itemReturnModels)
            {
                if (itemReturnModel.ProductDetails == null) 
                    continue;

                if (string.IsNullOrEmpty(itemReturnModel.ProductDetails.Image) ||
                    string.IsNullOrWhiteSpace(itemReturnModel.ProductDetails.Image))
                    itemReturnModel.ProductDetails.Image = DefaultImageUrl;
            }
            itemReturnModels.CurrentPage = page;
            itemReturnModels.TotalItems = totalCount;
            itemReturnModels.IsListPartial = true;
            itemReturnModels.TotalPages = totalPages;
            
            return itemReturnModels;
        }
        
        public async Task<InventoryReturnModel> GetInventoryItem(long id)
        {
            var inventoryItem = await _context.ProductInventories
                .Include(pi => pi.Product)
                .Include(pi => pi.Customer)
                .Include(pi => pi.Supplier)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            
            if (inventoryItem == null)
                throw new InventoryItemNotFoundException();

            var inventoryItemReturnModel = _mapper.Map<ProductInventory, InventoryReturnModel>(inventoryItem);
            if (inventoryItemReturnModel.ProductDetails == null) 
                return inventoryItemReturnModel;
            
            if (string.IsNullOrEmpty(inventoryItemReturnModel.ProductDetails.Image) ||
                string.IsNullOrWhiteSpace(inventoryItemReturnModel.ProductDetails.Image))
                inventoryItemReturnModel.ProductDetails.Image = DefaultImageUrl;

            return inventoryItemReturnModel;
        }

        public async Task<List<InventoryReturnModel>> ExportProducts(InventoryExportModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var inventoryItems = await _context.ProductInventories
                        .Include(pi => pi.Customer)
                        .Include(pi => pi.Product)
                        .Include(pi => pi.Supplier)
                        .Where(pi => model.InventoryItems.Contains(pi.Id)).ToListAsync();

                    var inventoryItemsIds = inventoryItems.Select(ii => ii.Id).Distinct().ToList();
                    foreach (var inventoryItemExportId in inventoryItemsIds)
                    {
                        if (!inventoryItemsIds.Contains(inventoryItemExportId))
                            throw new InventoryItemNotFoundException();
                    }

                    foreach (var inventoryItem in inventoryItems)
                    {
                        if (inventoryItem.CustomerId != null || inventoryItem.ExportedDate >= inventoryItem.ImportedDate) 
                            throw new InventoryItemAlreadyExportedException();
                        
                        inventoryItem.CustomerId = model.CustomerId;
                        inventoryItem.ExportedDate = model.ExportedDate;
                    }
                    
                    _context.ProductInventories.UpdateRange(inventoryItems);
                    await _context.SaveChangesAsync();
                    
                    transaction.Commit();

                    var inventoryItemReturnModels =
                        _mapper.Map<List<ProductInventory>, List<InventoryReturnModel>>(inventoryItems);
                    foreach (var inventoryItemReturnModel in inventoryItemReturnModels)
                    {
                        if (inventoryItemReturnModel.ProductDetails == null) 
                            continue;
                        if (string.IsNullOrEmpty(inventoryItemReturnModel.ProductDetails.Image) ||
                            string.IsNullOrWhiteSpace(inventoryItemReturnModel.ProductDetails.Image))
                            inventoryItemReturnModel.ProductDetails.Image = DefaultImageUrl;
                    }

                    return inventoryItemReturnModels;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}