using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services
{
    public class CityService : ICityService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;

        public CityService(IRepositoryWrapper repoWrapper, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
        }

        public IEnumerable<DataAccessCity.City> GetAll()
        {
            return _repoWrapper.City.FindAll();
        }
        public IEnumerable<CityDTO> GetAllDTO()
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(GetAll());
        }

        public CityDTO GetById(int cityId)
        {
            var city = _repoWrapper.City
                   .FindByCondition(q => q.ID == cityId)
                   .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                   .Include(k => k.CityAdministration)
                      .ThenInclude(a => a.User)
                   .Include(m => m.CityMembers)
                      .ThenInclude(u => u.User)
                   .Include(l => l.CityDocuments)
                       .ThenInclude(d => d.CityDocumentType)
                   .FirstOrDefault();
            return _mapper.Map<DataAccessCity.City, CityDTO>(city);
        }

        public CityProfileDTO CityProfile(int cityId)
        {
            var city = GetById(cityId);

            var cityHead = city?.CityAdministration
                    ?.FirstOrDefault(a => a.EndDate == null && a.AdminType.AdminTypeName == "Голова Станиці");

            var cityAdmins = city?.CityAdministration
                .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                .ToList();

            var members = city?.CityMembers.Where(m => m.EndDate == null && m.StartDate != null).Take(6).ToList();
            var followers = city?.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).Take(6).ToList();

            var cityDoc = city?.CityDocuments.Take(4).ToList();
            return new CityProfileDTO { City = city, CityHead = cityHead, Members = members, Followers = followers, CityAdmins = cityAdmins, CityDoc = cityDoc };
        }
        public CityProfileDTO CityMembers(int cityId)
        {
            var city = GetById(cityId);

            var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate != null).ToList();
            return new CityProfileDTO { City = city, Members = members };
        }
        public CityProfileDTO CityFollowers(int cityId)
        {
            var city = GetById(cityId);

            var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();
            return new CityProfileDTO { City = city, Followers = followers };
        }
        public CityProfileDTO CityAdmins(int cityId)
        {
            var city = GetById(cityId);

            var cityAdmins = city.CityAdministration
                                     .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                                     .ToList();
            return new CityProfileDTO { City = city, CityAdmins = cityAdmins };
        }
        public CityProfileDTO CityDocuments(int cityId)
        {
            var city = GetById(cityId);

            var cityDoc = city.CityDocuments.ToList();
            return new CityProfileDTO { City = city, CityDoc = cityDoc };
        }
        public CityProfileDTO Edit(int cityId)
        {
            var city = GetById(cityId);

            var cityAdmins = city.CityAdministration.Where(a => a.EndDate == null).ToList();
            var members = city.CityMembers.Where(p => cityAdmins.All(a => a.UserId != p.UserId)).Where(m => m.EndDate == null && m.StartDate != null).ToList();
            var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();

            return new CityProfileDTO { City = city, CityAdmins = cityAdmins, Members = members, Followers = followers };
        }
        public void Edit(CityProfileDTO model, IFormFile file)
        {
            var city = model.City;
            UploadPhoto(city, file);
            _repoWrapper.City.Update(_mapper.Map<CityDTO, DataAccessCity.City>(model.City));
            _repoWrapper.Save();
        }
        public int Create(CityProfileDTO model, IFormFile file)
        {
            var city = model.City;
            UploadPhoto(city, file);
            var modelToCreate = _mapper.Map<CityDTO, DataAccessCity.City>(model.City);
            _repoWrapper.City.Create(modelToCreate);
            _repoWrapper.Save();
            return modelToCreate.ID;
        }
        private void UploadPhoto( CityDTO city, IFormFile file)
        {
            var cityId = city.ID;
            var oldImageName = _repoWrapper.City.FindByCondition(i => i.ID == cityId).FirstOrDefault()?.Logo;
            if (file != null && file.Length > 0)
            {
                using (var img = Image.FromStream(file.OpenReadStream()))
                {
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Cities");
                    if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                    {
                        var oldPath = Path.Combine(uploads, oldImageName);
                        if (File.Exists(oldPath))
                        {
                            File.Delete(oldPath);
                        }
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    city.Logo = fileName;
                }
            }
            else
            {
                city.Logo = oldImageName ?? "333493fe-9c81-489f-bce3-5d1ba35a8c36.jpg";
            }
        }
    }
}
