using AutoMapper;
using EPlast.BLL.DTO.EducatorsStaff;
using EPlast.BLL.Interfaces.EducatorsStaff;
using EPlast.DataAccess.Entities;
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

namespace EPlast.BLL.Services.EducatorsStaff
{
    public class EducatorsStaffService : IEducatorsStaffService
    {

        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;


        public EducatorsStaffService(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;

        }

        public async Task<EducatorsStaffDTO> CreateKadra(EducatorsStaffDTO kadrasDTO)
        {
            var user = await _userManager.FindByIdAsync(kadrasDTO.UserId);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.RegisteredUser) || roles.Contains(Roles.Supporter))
            {
                throw new ArgumentException("Can't add with the restricted roles");
            }

            var userExists = await UserHasSuchStaffEdit(kadrasDTO.UserId, kadrasDTO.KadraVykhovnykivTypeId);

            if (userExists)
            {
                throw new ArgumentException("User has such staff");
            }

            var newKV = _mapper.Map<EducatorsStaffDTO, DataAccess.Entities.EducatorsStaff.EducatorsStaff>(kadrasDTO);
            await _repositoryWrapper.KVs.CreateAsync(newKV);
            await _repositoryWrapper.SaveAsync();
            return _mapper.Map<DataAccess.Entities.EducatorsStaff.EducatorsStaff, EducatorsStaffDTO>(newKV);
        }

        public async Task DeleteKadra(int kadra_id)
        {

            var deletedKadra = (await _repositoryWrapper.KVs.GetFirstAsync(d => d.ID == kadra_id));

            _repositoryWrapper.KVs.Delete(deletedKadra);
            await _repositoryWrapper.SaveAsync();



        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetAllKVsAsync()
        {
            return _mapper.Map<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(
                 await _repositoryWrapper.KVs.GetAllAsync());

        }

        public async Task<EducatorsStaffDTO> GetKadraById(int KadraID)
        {
            var KV = _mapper.Map<DataAccess.Entities.EducatorsStaff.EducatorsStaff, EducatorsStaffDTO>(await _repositoryWrapper.KVs.GetFirstAsync(c => c.ID == KadraID,
                include:
                source => source.Include(c => c.User)));
            return KV;
        }

        public async Task<EducatorsStaffDTO> GetKadraByRegisterNumber(int KadrasRegisterNumber)
        {
            var KV = _mapper.Map<DataAccess.Entities.EducatorsStaff.EducatorsStaff, EducatorsStaffDTO>(await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(c => c.NumberInRegister == KadrasRegisterNumber));
            return KV;
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetKVsOfGivenUser(string UserId)
        {
            var Kadras = _mapper.Map<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.UserId == UserId));
            return Kadras;
        }

        public async Task<IEnumerable<EducatorsStaffDTO>> GetKVsWithKVType(int kvTypeId)
        {

            var KVs = _mapper.Map<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IEnumerable<EducatorsStaffDTO>>(await _repositoryWrapper.KVs.GetAllAsync(c => c.KadraVykhovnykivType.ID == kvTypeId,
                include:
                source => source.Include(c => c.User)));
            return KVs;
        }

        public async Task<bool> DoesUserHaveSuchStaff(string UserId, int kadraId)
        {
            var edustaff = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.KadraVykhovnykivTypeId == kadraId && x.UserId == UserId));
            return edustaff != null;

        }

        public async Task UpdateKadra(EducatorsStaffDTO kadrasDTO)
        {
            bool existRegisterNumber = await StaffWithRegisternumberExistsEdit(kadrasDTO.KadraVykhovnykivTypeId, kadrasDTO.NumberInRegister);
            if (existRegisterNumber)
            {
                throw new ArgumentException("Number in register already exists");
            }
            var editedKadra = await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == kadrasDTO.ID);

            editedKadra.NumberInRegister = kadrasDTO.NumberInRegister;
            editedKadra.DateOfGranting = kadrasDTO.DateOfGranting;

            _repositoryWrapper.KVs.Update(editedKadra);
            await _repositoryWrapper.SaveAsync();

        }

        public async Task<bool> StaffWithRegisternumberExists(int registerNumber)
        {
            var staffwithnum = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.NumberInRegister == registerNumber));
            return staffwithnum != null;
        }




        public async Task<bool> StaffWithRegisternumberExistsEdit(int kadraId, int numberInRegister)
        {
            var staffwithnum = (await _repositoryWrapper.KVs.GetFirstOrDefaultAsync(x => x.NumberInRegister == numberInRegister && x.ID != kadraId));
            return staffwithnum != null;
        }


        public async Task<bool> UserHasSuchStaffEdit(string UserId, int kadraId)
        {
            var edustaff = (await _repositoryWrapper.KVs.GetAllAsync(x => x.KadraVykhovnykivTypeId == kadraId && x.UserId == UserId));

            return edustaff.Any();
        }

        public async Task<string> GetUserByEduStaff(int EduStaffId)
        {
            var eduStaff = (await _repositoryWrapper.KVs.GetFirstAsync(x => x.ID == EduStaffId));
            return eduStaff.UserId;
        }

        /// <inheritdoc />
        public IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaffTableObject> GetEducatorsStaffTableObject(int kadraType, string searchedData,
            int page, int pageSize)
        {
            return _repositoryWrapper.KVs.GetEducatorsStaff(kadraType, searchedData, page, pageSize);
        }
        public async Task<Tuple<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaffTableObject>, int>> GetEducatorsStaffTableAsync
               (int kadraType, IEnumerable<string> sortByOrder, string searchedData, int page, int pageSize)
        {
            var filter = GetFilter(kadraType, searchedData);
            var order = GetOrder(sortByOrder);
            var include = GetInclude();
            var selector = GetSelector();
            var tuple = await _repositoryWrapper.KVs.GetRangeAsync(filter, selector, order, include, page, pageSize);
            var kadras = tuple.Item1;
            var rows = tuple.Item2;
            return new Tuple<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaffTableObject>, int>(_mapper.Map<IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IEnumerable<DataAccess.Entities.EducatorsStaff.EducatorsStaffTableObject>>(kadras), rows);
        }

        private Func<IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IIncludableQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetInclude()
        {
            Func<IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IIncludableQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> expr = x => x.Include(i => i.User).Include(i => i.KadraVykhovnykivType);
            return expr;
        }

        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, bool>> GetFilter(int kadraType, string searchedData)
        {
            var searchedDataEmty = string.IsNullOrEmpty(searchedData);
            var getDate = searchedDataEmty ? "" : String.Join("-", searchedData.Split(".").Reverse());
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, bool>> searchedDataExpr = (searchedDataEmty) switch
            {
                true => x => true,
                false => x =>
                x.NumberInRegister.ToString().Contains(searchedData)
                || (x.User.FirstName + " " + x.User.LastName).Contains(searchedData)
                || x.DateOfGranting.ToString().Contains(getDate)
            };

            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, bool>> kadraTypeExpr = (kadraType > 0) switch
            {
                true => x => x.KadraVykhovnykivTypeId.Equals(kadraType),
                false => x => false
            };

            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, bool>> allFilterByTable = Combine(searchedDataExpr, kadraTypeExpr);
            return allFilterByTable;
        }

        public static Expression<T> Combine<T>(Expression<T> firstExpression, Expression<T> secondExpression)
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

        private Func<IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>> GetOrder(IEnumerable<string> sortByOrder)
        {
            Func<IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>, IQueryable<DataAccess.Entities.EducatorsStaff.EducatorsStaff>> expr = (sortByOrder.First()) switch
            {
                "id" => (sortByOrder.Last()) switch
                {
                    "ascend" =>  x => x.OrderBy(GetOrderByID()),
                    "descend" => x => x.OrderByDescending(GetOrderByID()),
                    _ => x => x
                },
                "userName" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByUserName()),
                    "descend" =>x => x.OrderByDescending(GetOrderByUserName()),
                    _ => x => x
                },
                "numberInRegister" => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByNumberInRegister()),
                    "descend" => x => x.OrderByDescending(GetOrderByNumberInRegister()),
                    _ => x => x
                },
                _ => (sortByOrder.Last()) switch
                {
                    "ascend" => x => x.OrderBy(GetOrderByDateOfGranting()),
                    "descend" => x => x.OrderByDescending(GetOrderByDateOfGranting()),
                    _ => x => x
                }
            };
            return expr;
        }
        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, DataAccess.Entities.EducatorsStaff.EducatorsStaff>> GetSelector()
        {
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, DataAccess.Entities.EducatorsStaff.EducatorsStaff>> expr = x => x;
            return expr;
        }

        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByNumberInRegister()
        {
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> expr = x => x.NumberInRegister;
            return expr;
        }

        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByUserName()
        {
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> expr = x => x.User.FirstName + " " + x.User.LastName;
            return expr;
        }

        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByDateOfGranting()
        {
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> expr = x => x.DateOfGranting;
            return expr;
        }
        private Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> GetOrderByID()
        {
            Expression<Func<DataAccess.Entities.EducatorsStaff.EducatorsStaff, object>> expr = x => x.ID;
            return expr;
        }
    }
}