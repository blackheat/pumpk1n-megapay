using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Exceptions.Chain;
using pumpk1n_backend.Exceptions.Others;
using pumpk1n_backend.Exceptions.Products;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.ChainTransferModels.Products;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Products;
using pumpk1n_backend.Models.ReturnModels.Products;
using pumpk1n_backend.Models.TransferModels.Products;

namespace pumpk1n_backend.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IProductChainService _productChainService;

        private const string DefaultImageUrl =
            "https://ssl-product-images.www8-hp.com/digmedialib/prodimg/lowres/c05962484.png";

        public ProductService(DatabaseContext context, IMapper mapper, IProductChainService productChainService)
        {
            _context = context;
            _mapper = mapper;
            _productChainService = productChainService;
        }

        public async Task Resync()
        {
            var products = await _context.Products.ToListAsync();
            foreach (var product in products)
            {
                var chainModel = new ChainProductTransferModel
                {
                    Id = product.Id.ToString(),
                    CreatedDate = product.AddedDate.ToBinary().ToString(),
                    Hash = product.ComputeHash().ToString()
                };
                await _productChainService.AddProduct(chainModel);
            }
        }
        
        public async Task<ProductReturnModel> AddProduct(ProductInsertModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = _mapper.Map<ProductInsertModel, Product>(model);
                    product.AddedDate = DateTime.UtcNow;
                    _context.Products.Add(product);

                    await _context.SaveChangesAsync();
                    
                    var chainModel = new ChainProductTransferModel
                    {
                        Id = product.Id.ToString(),
                        CreatedDate = product.AddedDate.ToBinary().ToString(),
                        Hash = product.ComputeHash().ToString()
                    };
                    await _productChainService.AddProduct(chainModel);
                    
                    transaction.Commit();

                    return _mapper.Map<Product, ProductReturnModel>(product);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ProductReturnModel> UpdateProduct(ProductInsertModel model, long productId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                        throw new ProductNotFoundException();
                    
                    var productChainInfo = await _productChainService.GetProduct(product.Id);
                    if (productChainInfo == null)
                        throw new DataNotFoundInChainException();
                    if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                        throw new ChainCodeDataNotInSyncException();
                    
                    _mapper.Map(model, product);

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    
                    var chainModel = new ChainProductTransferModel
                    {
                        Id = product.Id.ToString(),
                        CreatedDate = product.AddedDate.ToBinary().ToString(),
                        Hash = product.ComputeHash().ToString()
                    };
                    await _productChainService.AddProduct(chainModel);
                    
                    transaction.Commit();

                    return _mapper.Map<Product, ProductReturnModel>(product);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteProduct(long productId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                        throw new ProductNotFoundException();
                    
                    var productChainInfo = await _productChainService.GetProduct(product.Id);
                    if (productChainInfo == null)
                        throw new DataNotFoundInChainException();
                    if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                        throw new ChainCodeDataNotInSyncException();

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    
                    await _productChainService.DeleteProduct(productId);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ProductReturnModel> GetProduct(long productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                throw new ProductNotFoundException();
            
            var productChainInfo = await _productChainService.GetProduct(product.Id);
            if (productChainInfo == null)
                throw new DataNotFoundInChainException();
            if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                throw new ChainCodeDataNotInSyncException();

            if (string.IsNullOrEmpty(product.Image) || string.IsNullOrWhiteSpace(product.Image))
                product.Image = DefaultImageUrl;

            return _mapper.Map<Product, ProductReturnModel>(product);
        }

        public async Task<CustomList<ProductReturnModel>> GetProducts(int page, int count, ProductSearchFilterModel filterModel)
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();

            var startAt = (page - 1) * count;

            var products = await _context.Products
                .Where(p => p.Name.Contains(filterModel.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            p.Price >= filterModel.MinPrice && p.Price <= filterModel.MaxPrice)
                .OrderByDescending(p => p.AddedDate)
                .Skip(startAt)
                .Take(count).ToListAsync();

            foreach (var product in products)
            {
                var productChainInfo = await _productChainService.GetProduct(product.Id);
                if (productChainInfo == null)
                    throw new DataNotFoundInChainException();
                if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                    throw new ChainCodeDataNotInSyncException();
                
                if (string.IsNullOrEmpty(product.Image) || string.IsNullOrWhiteSpace(product.Image))
                    product.Image = DefaultImageUrl;
            }

            var totalCount = await _context.Products
                .Where(p => p.Name.Contains(filterModel.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            p.Price >= filterModel.MinPrice && p.Price <= filterModel.MaxPrice)
                .OrderByDescending(p => p.AddedDate).CountAsync();
            var totalPages = totalCount / count + (totalCount % count > 0 ? 1 : 0);
            
            var productReturnModels = _mapper.Map<List<Product>, CustomList<ProductReturnModel>>(products);
            productReturnModels.TotalItems = totalCount;
            productReturnModels.TotalPages = totalPages;
            productReturnModels.CurrentPage = page;
            productReturnModels.IsListPartial = true;

            return productReturnModels;
        }

        public async Task ChangeProductDeprecatedStatus(long productId, bool status)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                    if (product == null)
                        throw new ProductNotFoundException();
                    
                    var productChainInfo = await _productChainService.GetProduct(product.Id);
                    if (productChainInfo == null)
                        throw new DataNotFoundInChainException();
                    if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                        throw new ChainCodeDataNotInSyncException();

                    product.Deprecated = status;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    
                    var chainModel = new ChainProductTransferModel
                    {
                        Id = product.Id.ToString(),
                        CreatedDate = product.AddedDate.ToBinary().ToString(),
                        Hash = product.ComputeHash().ToString()
                    };
                    await _productChainService.AddProduct(chainModel);
                    
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public async Task ChangeProductStockStatus(long productId, bool status)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                    if (product == null)
                        throw new ProductNotFoundException();
                    
                    var productChainInfo = await _productChainService.GetProduct(product.Id);
                    if (productChainInfo == null)
                        throw new DataNotFoundInChainException();
                    if (long.Parse(productChainInfo.Hash) != product.ComputeHash())
                        throw new ChainCodeDataNotInSyncException();

                    product.OutOfStock = !status;

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    
                    var chainModel = new ChainProductTransferModel
                    {
                        Id = product.Id.ToString(),
                        CreatedDate = product.AddedDate.ToBinary().ToString(),
                        Hash = product.ComputeHash().ToString()
                    };
                    await _productChainService.AddProduct(chainModel);
                    
                    transaction.Commit();
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