using AutoMapper;
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
using System.Globalization;

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

        public async Task AddPrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = _mapper.Map<PrecautionDTO, Precaution>(precautionDTO);
            await _repoWrapper.Precaution.CreateAsync(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task ChangePrecautionAsync(PrecautionDTO precautionDTO, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = await _repoWrapper.Precaution.GetFirstAsync(x => x.Id == precautionDTO.Id);
            Precaution.Name = precautionDTO.Name;
            _repoWrapper.Precaution.Update(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task DeletePrecautionAsync(int id, User user)
        {
            await CheckIfAdminAsync(user);
            var Precaution = (await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            if (Precaution == null)
                throw new ArgumentNullException($"Precaution with {id} not found");
            _repoWrapper.Precaution.Delete(Precaution);
            await _repoWrapper.SaveAsync();
        }

        public async Task<IEnumerable<PrecautionDTO>> GetAllPrecautionAsync()
        {
            return _mapper.Map<IEnumerable<Precaution>, IEnumerable<PrecautionDTO>>(await _repoWrapper.Precaution.GetAllAsync());
        }

        public async Task<PrecautionDTO> GetPrecautionAsync(int id)
        {
            var Precaution = _mapper.Map<PrecautionDTO>(await _repoWrapper.Precaution.GetFirstAsync(d => d.Id == id));
            return Precaution;
        }

        public async Task CheckIfAdminAsync(User user)
        {
            if (!(await _userManager.GetRolesAsync(user)).Contains(Roles.Admin))
                throw new UnauthorizedAccessException();
        }

        public async Task<Tuple<IEnumerable<UserPrecautionsTableObject>, int>> GetUsersPrecautionsForTableAsync(IEnumerable<string> sortByOrder, IEnumerable<string> statusSorter, IEnumerable<string> precautionNameSorter, IEnumerable<string> dateSorter, string searchedData, int page, int pageSize)
        {
            var filter = GetFilter(searchedData, statusSorter, precautionNameSorter, dateSorter);
            var order = GetOrder(sortByOrder);
            var selector = GetSelector();
            var include = GetInclude();
            var tuple = await _repoWrapper.UserPrecaution.GetRangeAsync(filter, selector, order, include, page, pageSize);
            var precautions = tuple.Item1;
            var rows = tuple.Item2;

            return new Tuple<IEnumerable<UserPrecautionsTableObject>, int>(_mapper.Map<IEnumerable<UserPrecaution>, IEnumerable<UserPrecautionsTableObject>>(precautions), rows);
        }

        private Expression<Func<UserPrecaution, bool>> GetFilter(string searchedData, IEnumerable<string> statusSorter, IEnumerable<string> precautionNameSorter, IEnumerable<string> dateSorter)
        {
            var searchedDataEmty = string.IsNullOrEmpty(searchedData);
            Expression<Func<UserPrecaution, bool>> expr = (searchedDataEmty) switch
            {
                true => x => true,
                false => x => x.Number.ToString().Contains(searchedData) || x.Status.Contains(searchedData)
                || (x.User.FirstName + " " + x.User.LastName).Contains(searchedData)
                //|| (x.Date.Day.ToString()+"."+ x.Date.Month.ToString() + "." +x.Date.Year.ToString()).Contains(searchedData)
                //|| (x.EndDate.Day.ToString() + "." + x.EndDate.Month.ToString() + "." + x.EndDate.Year.ToString()).Contains(searchedData) 

                || x.Date.ToString("dd.MM.yyyy").Contains(searchedData) || x.EndDate.ToString("dd.MM.yyyy").Contains(searchedData)

                //|| x.Date.ToString().Contains(searchedData) || x.EndDate.ToString().Contains(searchedData)
                || x.Reporter.Contains(searchedData) || x.Reason.Contains(searchedData)
                || x.Precaution.Name.Contains(searchedData)
                //"dd.MM.yyyy"
            };
            return expr;
        }
        private Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>> GetInclude()
        {
            Func<IQueryable<UserPrecaution>, IIncludableQueryable<UserPrecaution, object>> expr = x => x.Include(i => i.User).Include(c => c.Precaution);
            return expr;
        }
        /*private Expression<Func<UserPrecaution, object>> GetOrder(IEnumerable<string> sortByOrder)
        {
            Expression<Func<UserPrecaution, object>> expr = x => x.Id;
            return expr;
        }*/
        private Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>> GetOrder(IEnumerable<string> sortByOrder)
        {
            Func<IQueryable<UserPrecaution>, IQueryable<UserPrecaution>> expr;
            if (sortByOrder.Contains("number"))
            {
                if (sortByOrder.Contains("ascend"))
                    expr = x => x.OrderBy(x => x.Number);
                else if (sortByOrder.Contains("descend"))
                    expr = x => x.OrderByDescending(x => x.Number);
                else
                    expr = x => x;
            }
            else if (sortByOrder.Contains("userName"))
            {
                if (sortByOrder.Contains("ascend"))
                    expr = x => x.OrderBy(x => x.User.FirstName + " " + x.User.LastName);
                else if (sortByOrder.Contains("descend"))
                    expr = x => x.OrderByDescending(x => x.User.FirstName + " " + x.User.LastName);
                else
                    expr = x => x;
            }
            else
            {
                if (sortByOrder.Contains("ascend"))
                    expr = x => x.OrderBy(x => x.EndDate);
                else if (sortByOrder.Contains("descend"))
                    expr = x => x.OrderByDescending(x => x.EndDate);
                else
                    expr = x => x;
            }
            return expr;
        }
        private Expression<Func<UserPrecaution, UserPrecaution>> GetSelector()
        {

            Expression<Func<UserPrecaution, UserPrecaution>> expr = x => x;
            return expr;
        }
    }
}
