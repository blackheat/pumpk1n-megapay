using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Orders;
using pumpk1n_backend.Exceptions.Others;
using pumpk1n_backend.Exceptions.Products;
using pumpk1n_backend.Exceptions.Tokens;
using pumpk1n_backend.Helpers.Tokens;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.ReturnModels.Orders;
using pumpk1n_backend.Models.TransferModels.Orders;
using pumpk1n_backend.Models.TransferModels.Tokens;

namespace pumpk1n_backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;

        public OrderService(DatabaseContext context, IMapper mapper, ITokenHelper tokenHelper)
        {
            _context = context;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<OrderReturnModel> Checkout(long userId, CheckoutTransferModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .FirstOrDefaultAsync(o =>
                            o.CustomerId == userId && o.AddedDate > o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                            o.AddedDate > o.CancelledDate);
                    
                    if (order == null)
                    {
                        order = new Order
                        {
                            AddedDate = DateTime.UtcNow,
                            CustomerId = userId
                        };

                        _context.Orders.Add(order);
                        _context.SaveChanges();
                        
                        order = await _context.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                            .FirstOrDefaultAsync(o =>
                                o.CustomerId == userId && o.AddedDate > o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                                o.AddedDate > o.CancelledDate);
                    }

                    foreach (var item in model.Items)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    
                        if (product == null)
                            throw new ProductNotFoundException();
                        
                        if (product.Deprecated)
                            throw new ProductIsDeprecatedException();
                        
                        if (product.OutOfStock)
                            throw new ProductIsOutOfStockException();

                        var orderItem = _mapper.Map<OrderItemTransferModel, OrderItem>(item);
                        orderItem.OrderId = order.Id;
                        orderItem.SinglePrice = product.Price;

                        _context.OrderItems.Add(orderItem);
                    }
                    
                    var totalPrice = order.OrderItems.Sum(oi => oi.SinglePrice * oi.Quantity);
                    if (totalPrice > order.Customer.Balance)
                        throw new InsufficientBalanceException();

                    order.CustomerName = model.Name;
                    order.Address = model.Address;
                    order.Notes = model.Notes;
                    order.CheckedOutDate = DateTime.UtcNow;
                    
                    // Updating balance
                    var currentDateTime = DateTime.UtcNow;
                    var tokenTransactionInsertModel = new TokenTransactionInsertModel
                    {
                        Amount = totalPrice,
                        Notes = $"Payment for order with ID : {order.Id}"
                    };
                    _tokenHelper.AddTokenTransaction(userId, currentDateTime,
                        currentDateTime, tokenTransactionInsertModel, TokenTransactionType.Subtract);
                    
                    _context.Orders.Update(order);
                    _context.Users.Update(order.Customer);
                    await _context.SaveChangesAsync();
                    
                    transaction.Commit();

                    var populatedCart = await _context.Orders
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == order.Id);
                    
                    return _mapper.Map<Order, OrderReturnModel>(populatedCart);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<OrderReturnModel> ConfirmOrder(long orderId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .FirstOrDefaultAsync(o =>
                            o.Id == orderId && o.AddedDate < o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                            o.AddedDate > o.CancelledDate);
                    
                    if (order == null)
                        throw new OrderNotFoundException();
                    
                    order.ConfirmedDate = DateTime.UtcNow;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<Order, OrderReturnModel>(order);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public async Task<OrderReturnModel> CancelOrder(long orderId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .FirstOrDefaultAsync(o =>
                            o.Id == orderId && o.AddedDate < o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                            o.AddedDate > o.CancelledDate);
                    
                    if (order == null)
                        throw new OrderNotFoundException();
                    
                    order.CancelledDate = DateTime.UtcNow;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<Order, OrderReturnModel>(order);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<CustomList<OrderReturnModel>> GetUserOrders(long userId, int page, int count)
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();

            var startAt = (page - 1) * count;
            
            var orders = await _context.Orders.Skip(startAt).Take(count)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.AddedDate)
                .ThenByDescending(o => o.CheckedOutDate)
                .ThenBy(o => o.ConfirmedDate)
                .ThenByDescending(o => o.CancelledDate)
                .Where(o => o.CustomerId == userId)
                .ToListAsync();
            
            var totalCount = await _context.Orders
                .Where(o =>
                    o.CustomerId == userId && o.AddedDate < o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                    o.AddedDate > o.CancelledDate)
                .CountAsync();
            var totalPages = totalCount / count + (totalCount % count > 0 ? 1 : 0);

            var orderReturnModels = _mapper.Map<List<Order>, CustomList<OrderReturnModel>>(orders);
            orderReturnModels.CurrentPage = page;
            orderReturnModels.TotalItems = totalCount;
            orderReturnModels.TotalPages = totalPages;
            orderReturnModels.IsListPartial = true;

            return orderReturnModels;
        }
        
        public async Task<CustomList<OrderReturnModel>> GetOrders(int page, int count)
        {
            if (page <= 0 || count <= 0)
                throw new InvalidPaginationDataException();

            var startAt = (page - 1) * count;
            
            var orders = await _context.Orders.Skip(startAt).Take(count)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.AddedDate)
                .ThenByDescending(o => o.CheckedOutDate)
                .ThenBy(o => o.ConfirmedDate)
                .ThenByDescending(o => o.CancelledDate)
                .ToListAsync();
            
            var totalCount = await _context.Orders
                .Where(o =>
                    o.AddedDate < o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                    o.AddedDate > o.CancelledDate)
                .CountAsync();
            var totalPages = totalCount / count + (totalCount % count > 0 ? 1 : 0);

            var orderReturnModels = _mapper.Map<List<Order>, CustomList<OrderReturnModel>>(orders);
            orderReturnModels.CurrentPage = page;
            orderReturnModels.TotalItems = totalCount;
            orderReturnModels.TotalPages = totalPages;
            orderReturnModels.IsListPartial = true;

            return orderReturnModels;
        }

        public async Task<OrderReturnModel> GetOrder(long orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            
            if (order == null)
                throw new OrderNotFoundException();

            var orderReturnModel = _mapper.Map<Order, OrderReturnModel>(order);
            return orderReturnModel;
        }

        public async Task<OrderReturnModel> GetUserOrder(long userId, long orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == userId);
            
            if (order == null)
                throw new OrderNotFoundException();

            var orderReturnModel = _mapper.Map<Order, OrderReturnModel>(order);
            return orderReturnModel;
        }
    }
}