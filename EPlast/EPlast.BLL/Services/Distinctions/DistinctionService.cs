using AutoMapper;
using EPlast.BLL.DTO.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPlast.BLL
{
    public class DistinctionService : IDistinctionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public DistinctionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }

        public async Task AddDistinctionAsync(DistinctionDTO distinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = _mapper.Map<DistinctionDTO, Distinction>(distinctionDTO);
            await _repoWrapper.Distinction.CreateAsync(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangeDistinctionAsync(DistinctionDTO distinctionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = await _repoWrapper.Distinction.GetFirstAsync(x => x.Id == distinctionDTO.Id);
            distinction.Name = distinctionDTO.Name;
            _repoWrapper.Distinction.Update(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeleteDistinctionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var distinction = (await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
            if (distinction == null)
                throw new ArgumentNullException($"Distinction with {id} not found");
            _repoWrapper.Distinction.Delete(distinction);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<DistinctionDTO>> GetAllDistinctionAsync()
        {
            return _mapper.Map<IEnumerable<Distinction>, IEnumerable<DistinctionDTO>>(await _repoWrapper.Distinction.GetAllAsync());
        }
        
        public async Task<DistinctionDTO> GetDistinctionAsync(int id)
        {
            var distinction = _mapper.Map<DistinctionDTO>(await _repoWrapper.Distinction.GetFirstAsync(d => d.Id == id));
            return distinction;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }

        public async Task<Tuple<IEnumerable<UserDistinctionsTableObject>, int>> GetUsersDistinctionsForTableAsync(DistictionTableSettings tableSettings)
        {
            var filter = GetFilter(tableSettings.SearchedData);
            var order = GetOrder(tableSettings.SortByOrder);
            var selector = GetSelector();
            var include = GetInclude();
            var tuple = await _repoWrapper.UserDistinction.GetRangeAsync(filter, selector, order, include, tableSettings.Page, tableSettings.PageSize);
            var distinctions = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<UserDistinctionsTableObject>, int>(_mapper.Map<IEnumerable<UserDistinction>, IEnumerable<UserDistinctionsTableObject>>(distinctions), rows);
        }
        private Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>> GetInclude()
        {
            Func<IQueryable<UserDistinction>, IIncludableQueryable<UserDistinction, object>> expr = x => x.Include(i => i.User).Include(c => c.Distinction);
            return expr;
        }
        private Expression<Func<UserDistinction, bool>> GetFilter(string searchedData) 
        { 
            var searchedDataEmty = string.IsNullOrEmpty(searchedData);
            var getDate = searchedDataEmty ? "" : String.Join("-", searchedData.Split(".").Reverse());
            Expression<Func<UserDistinction, bool>> searchedDataExpr = (searchedDataEmty) switch
            {
                true => x => true,
                false => x => x.Number.ToString().Contains(searchedData) || x.Distinction.Name.Contains(searchedData)
                || (x.User.FirstName + " " + x.User.LastName).Contains(searchedData)
                || x.Date.Date.ToString().Contains(getDate)
                || x.Reporter.Contains(searchedData) || x.Reason.Contains(searchedData)                
            };
            return searchedDataExpr;
        }
        private Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>> GetOrder(IEnumerable<string> sortByOrder)
        {
            Func<IQueryable<UserDistinction>, IQueryable<UserDistinction>> expr = (sortByOrder.First()) switch
            {
                "number" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByNumber()),
                    "descend" => x => x.OrderByDescending(GetOrderByNumber()),
                    _ => x => x
                },

                "distinctionName" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByDistinctionName()),
                    "descend" => x => x.OrderByDescending(GetOrderByDistinctionName()),
                    _ => x => x
                },

                "userName" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByName()),
                    "descend" => x => x.OrderByDescending(GetOrderByName()),
                    _ => x => x
                },

                _ => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByDate()),
                    "descend" => x => x.OrderByDescending(GetOrderByDate()),
                    _ => x => x
                }
            };
            return expr;
        }
        private Expression<Func<UserDistinction, UserDistinction>> GetSelector()
        {
            Expression<Func<UserDistinction, UserDistinction>> expr = x => x;
            return expr;
        }
        private Expression<Func<UserDistinction, object>> GetOrderByNumber()
        {
            Expression<Func<UserDistinction, object>> expr = x => x.Number;
            return expr;
        }
        private Expression<Func<UserDistinction, object>> GetOrderByDistinctionName()
        {
            Expression<Func<UserDistinction, object>> expr = x => x.Distinction.Name;
            return expr;
        }
        private Expression<Func<UserDistinction, object>> GetOrderByName()
        {
            Expression<Func<UserDistinction, object>> expr = x => x.User.FirstName + " " + x.User.LastName;
            return expr;
        }
        private Expression<Func<UserDistinction, object>> GetOrderByDate()
        {
            Expression<Func<UserDistinction, object>> expr = x => x.Date;
            return expr;
        }
    }
}
