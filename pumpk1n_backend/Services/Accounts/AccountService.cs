using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pumpk1n_backend.Enumerations;
using pumpk1n_backend.Exceptions.Accounts;
using pumpk1n_backend.Helpers.Accounts;
using pumpk1n_backend.Models;
using pumpk1n_backend.Models.DatabaseContexts;
using pumpk1n_backend.Models.Entities.Accounts;
using pumpk1n_backend.Models.ReturnModels.Accounts;
using pumpk1n_backend.Models.TransferModels.Accounts;
using pumpk1n_backend.Utilities;

namespace pumpk1n_backend.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _context;
        private readonly IAccountHelper _accountHelper;
        private readonly IMapper _mapper;

        public AccountService(DatabaseContext context, IMapper mapper, IAccountHelper accountHelper)
        {
            _context = context;
            _mapper = mapper;
            _accountHelper = accountHelper;
        }

        #region Authentication

        public async Task RegisterAccount(UserRegisterModel model, UserType userType)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u =>
                        u.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase));
                    
                    if (user != null)
                        throw new UserAlreadyExistsException();

                    var currentDate = DateTime.UtcNow;
                    
                    user = _mapper.Map<UserRegisterModel, User>(model);
                    user.Nonce = StringUtilities.GenerateRandomString();
                    user.HashedPassword = StringUtilities.HashPassword(model.Password, user.Nonce);
                    user.RegisteredDate = currentDate;
                    user.UpdatedDate = currentDate;
                    user.UserType = userType;

                    _context.Users.Add(user);
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

        public async Task<UserBearerTokenModel> UserLogin(UserLoginModel model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email.Equals(model.Email, StringComparison.InvariantCultureIgnoreCase));
                
                if (user == null)
                    throw new InvalidCredentialsException();

                var hashedPassword = StringUtilities.HashPassword(model.Password, user.Nonce);
                if (!hashedPassword.Equals(user.HashedPassword))
                    throw new InvalidCredentialsException();

                var token = _accountHelper.JwtGenerator(user.Id, user.FullName, 0, user.UserType);
                var userTokenModel = new UserBearerTokenModel
                {
                    Token = token
                };

                return userTokenModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Account Details

        public async Task<UserInformationModel> GetUserDetails(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userInformationModel = _mapper.Map<User, UserInformationModel>(user);
            return userInformationModel;
        }

        public async Task<CustomList<UserInformationModel>> GetUsers(int page, int count, UserAccountFilterModel filterModel)
        {
            var startAt = (page - 1) * count;
            var users = await _context.Users
                .Where(u => u.FullName.Contains(filterModel.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            u.Email.Contains(filterModel.Email, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(u => u.RegisteredDate).Skip(startAt).Take(count).ToListAsync();
            
            var totalCount = await _context.Users
                .Where(u => u.FullName.Contains(filterModel.Name, StringComparison.InvariantCultureIgnoreCase) &&
                            u.Email.Contains(filterModel.Email, StringComparison.InvariantCultureIgnoreCase))
                .OrderByDescending(u => u.RegisteredDate).CountAsync();

            var totalPages = totalCount / count + (totalCount / count > 0 ? totalCount % count : 1);
            
            var userReturnModels = _mapper.Map<List<User>, CustomList<UserInformationModel>>(users);
            userReturnModels.CurrentPage = page;
            userReturnModels.TotalPages = totalPages;
            userReturnModels.TotalItems = totalCount;
            userReturnModels.IsListPartial = true;

            return userReturnModels;
        }

        #endregion
    }
}