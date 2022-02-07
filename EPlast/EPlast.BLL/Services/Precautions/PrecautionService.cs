﻿using AutoMapper;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EPlast.BLL.DTO.PrecautionsDTO;

namespace EPlast.BLL.Services
{
    public class PrecautionService : IPrecautionService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;

        public PrecautionService(IMapper mapper, IRepositoryWrapper repoWrapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _userManager = userManager;
        }
        //
        public async Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = _mapper.Map<PrecautionDTO, Precaution>(precautionDTO);
            await _repoWrapper.Precaution.CreateAsync(Precaution);
            await _repoWrapper.SaveAsync();
        }
        //
        public async Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = await _repoWrapper.Precaution.GetFirstAsync(x => x.Id == precautionDTO.Id);
            Precaution.Name = precautionDTO.Name;
            _repoWrapper.Precaution.Update(Precaution);
            await _repoWrapper.SaveAsync();
        }
        //
        public async Task DeletePrecautionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = (await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            if (Precaution == null)
                throw new ArgumentNullException($"Precaution with {id} not found");
            _repoWrapper.Precaution.Delete(Precaution);
            await _repoWrapper.SaveAsync();
        }
        //
        public async Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync()
        {
            return _mapper.Map<IEnumerable<Precaution>, IEnumerable<PrecautionDTO>>(await _repoWrapper.Precaution.GetAllAsync());
        }
        //
        public async Task<PrecautionDTO> GetPrecautionAsync(int id)
        {
            var Precaution = _mapper.Map<PrecautionDTO>(await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            return Precaution;
        }
        //
        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }
        //
        public async Task<Tuple<IEnumerable<UserPrecautionsTableObject>, int>> GetUsersPrecautionsForTableAsync(PrecautionTableSettings tableSettings)
        {
            var filter = GetFilter(tableSettings.SearchedData, tableSettings.StatusFilter, tableSettings.PrecautionNameFilter, tableSettings.DateFilter);
            var order = GetOrder(tableSettings.SortByOrder);
            var selector = GetSelector();
            var include = GetInclude();
            var tuple = await _repoWrapper.UserPrecaution.GetRangeAsync(filter, selector, order, include, tableSettings.Page, tableSettings.PageSize);
            var precautions = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<UserPrecautionsTableObject>, int>(_mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionsTableObject>>(precautions), rows);
        }
        private Expression<Func<UserPrecaution, bool>> GetFilter(string searchedData, IEnumerable<string> statusFilter, IEnumerable<string> precautionNameFilter, IEnumerable<string> dateFilter)
        {            
            var searchedDataEmty = string.IsNullOrEmpty(searchedData);
            var getDate = searchedDataEmty ? "" : String.Join("-", searchedData.Split(".").Reverse());
            Expression<Func<UserPrecaution, bool>> searchedDataExpr = (searchedDataEmty) switch
            {
                true => x => true,
                false => x => x.Number.ToString().Contains(searchedData) || x.Status.Contains(searchedData)
                || (x.User.FirstName + " " + x.User.LastName).Contains(searchedData)                
                || x.Date.Date.ToString().Contains(getDate) || x.EndDate.Date.ToString().Contains(getDate)
                || x.Reporter.Contains(searchedData) || x.Reason.Contains(searchedData)
                || x.Precaution.Name.Contains(searchedData)
            };

            Expression<Func<UserPrecaution, bool>> nameFilterExpr = (precautionNameFilter == null) switch
            {
                true => x => true,
                false => x => precautionNameFilter.Contains(x.Precaution.Name),
            };

            Expression<Func<UserPrecaution, bool>> dateFilterExpr = (dateFilter == null) switch
            {
                true => x => true,
                false => x => dateFilter.Contains(x.Date.Year.ToString()),
            };

            Expression <Func<UserPrecaution, bool>> statusFilterExpr = (statusFilter == null) switch
            {
                true => x => true,
                false => x => statusFilter.Contains(x.Status),
            };

            Expression<Func<UserPrecaution, bool>> searchAndStatusFilter = Combine(searchedDataExpr, statusFilterExpr);
            Expression<Func<UserPrecaution, bool>> filtersWithDateFilter = Combine(searchAndStatusFilter, dateFilterExpr);
            Expression<Func<UserPrecaution, bool>> allFilterByTable = Combine(filtersWithDateFilter, nameFilterExpr);

            return allFilterByTable;
        }
        private Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>> GetInclude()
        {
            Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>> expr = x => x.Include(i => i.User).Include(c => c.Precaution);
            return expr;
        }
        private Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>> GetOrder(IEnumerable<string> sortByOrder)
        {
            Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>> expr = (sortByOrder.First()) switch
            {
                "number" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByNumber()),
                    "descend" => x => x.OrderByDescending(GetOrderByNumber()),
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
                    "ascend" => x => x.OrderBy(GetOrderByEndDate()),
                    "descend" => x => x.OrderByDescending(GetOrderByEndDate()),
                    _ => x => x
                }
            };
            return expr;
        }        
        private Expression<Func<UserPrecaution, UserPrecaution>> GetSelector()
        {
            Expression<Func<UserPrecaution, UserPrecaution>> expr = x => x;
            return expr;
        }
        private static Expression<T> Combine<T>(Expression<T> firstExpression, Expression<T> secondExpression)
        {
            if (firstExpression is null)
                return secondExpression;

            if (secondExpression is null)
                return firstExpression;

            var invokedExpression = Expression.Invoke(
                secondExpression,
                firstExpression.Parameters);

            var combinedExpression = Expression.AndAlso(firstExpression.Body, invokedExpression);

            return Expression.Lambda<T>(combinedExpression, firstExpression.Parameters);
        }
        private Expression<Func<UserPrecaution, object>> GetOrderByNumber()
        {
            Expression<Func<UserPrecaution, object>> expr = x => x.Number;
            return expr;
        }
        private Expression<Func<UserPrecaution, object>> GetOrderByName()
        {
            Expression<Func<UserPrecaution, object>> expr = x => x.User.FirstName + " " + x.User.LastName;
            return expr;
        }
        private Expression<Func<UserPrecaution, object>> GetOrderByEndDate()
        {
            Expression<Func<UserPrecaution, object>> expr = x => x.EndDate;
            return expr;
        }
    }
}
