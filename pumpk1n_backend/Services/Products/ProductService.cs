using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Exceptions.Products;
using pumpk1n_backend.Models;
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

        public ProductService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductReturnModel> AddProduct(ProductInsertModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = _mapper.Map<ProductInsertModel, Product>(model);
                    _context.Products.Add(product);
                    
                    await _context.SaveChangesAsync();
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

                    _mapper.Map(model, product);

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
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

                    _context.Products.Remove(product);
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

        public async Task<ProductReturnModel> GetProduct(long productId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                    
                    if (product == null)
                        throw new ProductNotFoundException();

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
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

        public async Task<CustomList<ProductReturnModel>> GetProducts(int startAt, int count, string name = "")
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase)).Skip(startAt)
                .Take(count).ToListAsync();
            var productReturnModels = _mapper.Map<List<Product>, CustomList<ProductReturnModel>>(products);
            productReturnModels.StartAt = startAt;
            productReturnModels.EndAt = startAt + products.Count;
            productReturnModels.Total = products.Count;
            productReturnModels.IsListPartial = true;

            return productReturnModels;
        }
    }
}