using AutoMapper;
using EPlast.BLL.Queries.Distinction;
using EPlast.DataAccess.Entities.UserEntities;
using EPlast.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPlast.BLL.Handlers.DistinctionHandlers
{
    public class GetUsersDistinctionsForTableHandler : IRequestHandler<GetUsersDistinctionsForTableQuery, Tuple<IEnumerable<UserDistinctionsTableObject>, int>>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GetUsersDistinctionsForTableHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Tuple<IEnumerable<UserDistinctionsTableObject>, int>> Handle(GetUsersDistinctionsForTableQuery request, CancellationToken cancellationToken)
        {
            var filter = GetFilter(request.TableSettings.SearchedData);
            var order = GetOrder(request.TableSettings.SortByOrder);
            var selector = GetSelector();
            var include = GetInclude();
            var tuple = await _repositoryWrapper.UserDistinction.GetRangeAsync(filter, selector, order, include, request.TableSettings.Page, request.TableSettings.PageSize);
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
                    "ascend" => x => x.OrderBy(GetOrderByUserName()),
                    "descend" => x => x.OrderByDescending(GetOrderByUserName()),
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
        private Expression<Func<UserDistinction, object>> GetOrderByUserName()
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
