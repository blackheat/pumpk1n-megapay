using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Exceptions.Orders;
using pumpk1n_backend.Exceptions.Products;
using pumpk1n_backend.Exceptions.Tokens;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Orders;
using pumpk1n_backend.Models.ReturnModels.Orders;
using pumpk1n_backend.Models.TransferModels.Orders;

namespace pumpk1n_backend.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public OrderService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderReturnModel> CreateCart(long userId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var currentCart = await _context.Orders.FirstOrDefaultAsync(o =>
                        o.CustomerId == userId && o.AddedDate > o.CheckedOutDate || o.AddedDate > o.ConfirmedDate ||
                        o.AddedDate > o.CancelledDate);
                    if (currentCart != null)
                        throw new CartAlreadyExistsException();
                    
                    var cart = new Order
                    {
                        AddedDate = DateTime.UtcNow,
                        CustomerId = userId
                    };

                    _context.Orders.Add(cart);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    var populatedCart = await _context.Orders
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == cart.Id);

                    return _mapper.Map<Order, OrderReturnModel>(populatedCart);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<OrderReturnModel> GetCurrentCart(long userId)
        {
            var cart = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o =>
                    o.CustomerId == userId && o.AddedDate > o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                    o.AddedDate > o.CancelledDate);

            return _mapper.Map<Order, OrderReturnModel>(cart);
        }

        public async Task<OrderReturnModel> AddToCart(long userId, IEnumerable<OrderItemTransferModel> models)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cart = await _context.Orders
                        .FirstOrDefaultAsync(o =>
                            o.CustomerId == userId && o.AddedDate > o.CheckedOutDate || o.AddedDate > o.ConfirmedDate ||
                            o.AddedDate > o.CancelledDate);
                    
                    if (cart == null)
                        throw new CartNotFoundException();

                    foreach (var model in models)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);
                    
                        if (product == null)
                            throw new ProductNotFoundException();

                        var orderItem = _mapper.Map<OrderItemTransferModel, OrderItem>(model);
                        orderItem.OrderId = cart.Id;
                        orderItem.SinglePrice = product.Id;

                        _context.OrderItems.Add(orderItem);
                        await _context.SaveChangesAsync();
                    }
                    transaction.Commit();

                    var populatedCart = await _context.Orders
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .Include(o => o.Customer)
                        .FirstOrDefaultAsync(o => o.Id == cart.Id);
                    
                    return _mapper.Map<Order, OrderReturnModel>(populatedCart);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteCartItem(long orderItemId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cartItem = await _context.OrderItems
                        .Include(oi => oi.Order)
                        .FirstOrDefaultAsync(oi =>
                            oi.Id == orderItemId && oi.Order.CancelledDate < oi.Order.AddedDate &&
                            oi.Order.ConfirmedDate < oi.Order.AddedDate &&
                            oi.Order.CheckedOutDate < oi.Order.AddedDate);
                    
                    if (cartItem == null)
                        throw new CartItemNotFoundException();

                    _context.OrderItems.Remove(cartItem);
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
        
        public async Task<OrderItemReturnModel> UpdateCartItemQuantity(long orderItemId, long quantity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cartItem = await _context.OrderItems
                        .Include(oi => oi.Product)
                        .Include(oi => oi.Order)
                        .FirstOrDefaultAsync(oi =>
                            oi.Id == orderItemId && oi.Order.CancelledDate < oi.Order.AddedDate &&
                            oi.Order.ConfirmedDate < oi.Order.AddedDate &&
                            oi.Order.CheckedOutDate < oi.Order.AddedDate);
                    
                    if (cartItem == null)
                        throw new CartItemNotFoundException();
                    
                    cartItem.Quantity = quantity;
                    _context.OrderItems.Update(cartItem);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return _mapper.Map<OrderItem, OrderItemReturnModel>(cartItem);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<OrderReturnModel> Checkout(long userId, CustomerInformationCheckoutTransferModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cart = await _context.Orders
                        .Include(o => o.Customer)
                        .Include(o => o.Customer)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                        .FirstOrDefaultAsync(o =>
                            o.CustomerId == userId && o.AddedDate > o.CheckedOutDate && o.AddedDate > o.ConfirmedDate &&
                            o.AddedDate > o.CancelledDate);
                    
                    if (cart == null)
                        throw new CartNotFoundException();

                    var totalPrice = cart.OrderItems.Sum(oi => oi.SinglePrice * oi.Quantity);
                    if (totalPrice > cart.Customer.Balance)
                        throw new InsufficientBalanceException();

                    cart.CustomerName = model.Name;
                    cart.Address = model.Address;
                    cart.Notes = model.Notes;
                    cart.CheckedOutDate = DateTime.UtcNow;
                    cart.Customer.Balance -= totalPrice;

                    _context.Orders.Update(cart);
                    _context.Users.Update(cart.Customer);
                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return _mapper.Map<Order, OrderReturnModel>(cart);
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
    }
}