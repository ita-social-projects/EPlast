using AutoMapper;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.PrecautionHandlers
{
    public class GetUsersPrecautionsForTableHandler: IRequestHandler<GetUsersPrecautionsForTableQuery, Tuple<IEnumerable<UserPrecautionsTableObject>, int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetUsersPrecautionsForTableHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Tuple<IEnumerable<UserPrecautionsTableObject>, int>> Handle(GetUsersPrecautionsForTableQuery request, CancellationToken cancellationToken)
        {
            var filter = GetFilter(request.TableSettings.SearchedData, request.TableSettings.StatusFilter, request.TableSettings.PrecautionNameFilter, request.TableSettings.DateFilter);
            var order = GetOrder(request.TableSettings.SortByOrder);
            var selector = GetSelector();
            var include = GetInclude();
            var tuple = await _repositoryWrapper.UserPrecaution.GetRangeAsync(filter, selector, order, include, request.TableSettings.Page, request.TableSettings.PageSize);
            var precautions = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<UserPrecautionsTableObject>, int>(_mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionsTableObject>>(precautions), rows);
        }
        private Expression<Func<UserPrecaution, bool>> GetFilter(string searchedData, IEnumerable<UserPrecautionStatus> statusFilter, IEnumerable<string> precautionNameFilter, IEnumerable<string> dateFilter)
        {
            var searchedDataEmty = string.IsNullOrEmpty(searchedData);
            var getDate = searchedDataEmty ? "" : String.Join("-", searchedData.Split(".").Reverse());
            UserPrecautionStatus? statusSearch = null;
            if (!searchedDataEmty)
                statusSearch = SearchedByStatus(searchedData);

            Expression<Func<UserPrecaution, bool>> searchedDataExpr = (searchedDataEmty) switch
            {
                true => x => true,
                false => x => 
                x.Number.ToString().Contains(searchedData) 
                || (x.User.FirstName + " " + x.User.LastName).Contains(searchedData)
                || x.Date.Date.ToString().Contains(getDate) 
                || x.Date.AddMonths(x.Precaution.MonthsPeriod).Date.ToString().Contains(getDate)
                || x.Reporter.Contains(searchedData) 
                || x.Reason.Contains(searchedData)
                || x.Precaution.Name.Contains(searchedData)
                || x.Status == statusSearch
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

            Expression<Func<UserPrecaution, bool>> statusFilterExpr = (statusFilter == null) switch
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
            Expression<Func<UserPrecaution, object>> expr = x => x.Date.AddMonths(x.Precaution.MonthsPeriod); 
            return expr;
        }
        private UserPrecautionStatus? SearchedByStatus(string searchedData)
        {
            foreach (var s in (UserPrecautionStatus[]) Enum.GetValues(typeof(UserPrecautionStatus)))
            {
                if (s.GetDescription().Contains(searchedData))
                {
                    return s;
                }
            }
            return null;
        }
    }
}
