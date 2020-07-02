using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services
{
    public class CityService : ICityService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CityService(IRepositoryWrapper repoWrapper, IMapper mapper, IWebHostEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<DataAccessCity.City>> GetAllAsync()
        {
            var cities = await _repoWrapper.City.GetAllAsync(
                    include: source => source
                       .Include(c => c.CityAdministration)
                           .ThenInclude(t => t.AdminType)
                       .Include(k => k.CityAdministration)
                           .ThenInclude(a => a.User)
                       .Include(m => m.CityMembers)
                           .ThenInclude(u => u.User)
                       .Include(l => l.CityDocuments)
                           .ThenInclude(d => d.CityDocumentType)
                       .Include(r => r.Region));

            return cities;
        }

        public async Task<IEnumerable<CityDTO>> GetAllDTOAsync()
        {
            return _mapper.Map<IEnumerable<DataAccessCity.City>, IEnumerable<CityDTO>>(await GetAllAsync());
        }

        public async Task<CityDTO> GetByIdAsync(int cityId)
        {
            var city = await _repoWrapper.City.GetFirstOrDefaultAsync(
                    predicate: c => c.ID == cityId,
                    include : source => source
                        .Include(c => c.CityAdministration)
                            .ThenInclude(t => t.AdminType)
                        .Include(k => k.CityAdministration)
                            .ThenInclude(a => a.User)
                        .Include(m => m.CityMembers)
                            .ThenInclude(u => u.User)
                        .Include(l => l.CityDocuments)
                            .ThenInclude(d => d.CityDocumentType)
                        .Include(r => r.Region));

            return _mapper.Map<DataAccessCity.City, CityDTO>(city);
        }

        public async Task<CityProfileDTO> GetCityProfileAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var cityHead = city.CityAdministration?
                .FirstOrDefault(a => a.EndDate == null && a.AdminType.AdminTypeName == "Голова Станиці");
            var cityAdmins = city.CityAdministration
                .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                .ToList();
            var members = city.CityMembers
                .Where(m => m.EndDate == null && m.StartDate != null)
                .Take(6)
                .ToList();
            var followers = city.CityMembers
                .Where(m => m.EndDate == null && m.StartDate == null)
                .Take(6)
                .ToList();
            var cityDoc = city.CityDocuments.Take(4).ToList();

            return new CityProfileDTO { City = city, CityHead = cityHead, Members = members, Followers = followers, CityAdmins = cityAdmins, CityDoc = cityDoc };
        }

        public async Task<CityProfileDTO> GetCityMembersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var members = city.CityMembers
                .Where(m => m.EndDate == null && m.StartDate != null)
                .ToList();

            return new CityProfileDTO { City = city, Members = members };
        }

        public async Task<CityProfileDTO> GetCityFollowersAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var followers = city.CityMembers
                .Where(m => m.EndDate == null && m.StartDate == null)
                .ToList();

            return new CityProfileDTO { City = city, Followers = followers };
        }

        public async Task<CityProfileDTO> GetCityAdminsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var cityAdmins = city.CityAdministration
                .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                .ToList();

            return new CityProfileDTO { City = city, CityAdmins = cityAdmins };
        }

        public async Task<CityProfileDTO> GetCityDocumentsAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var cityDoc = city.CityDocuments.ToList();

            return new CityProfileDTO { City = city, CityDoc = cityDoc };
        }

        public async Task<CityProfileDTO> EditAsync(int cityId)
        {
            var city = await GetByIdAsync(cityId);
            if (city == null)
            {
                return null;
            }
            var cityAdmins = city.CityAdministration
                .Where(a => a.EndDate == null)
                .ToList();
            var members = city.CityMembers
                .Where(p => cityAdmins.All(a => a.UserId != p.UserId))
                .Where(m => m.EndDate == null && m.StartDate != null)
                .ToList();
            var followers = city.CityMembers
                .Where(m => m.EndDate == null && m.StartDate == null)
                .ToList();

            return new CityProfileDTO { City = city, CityAdmins = cityAdmins, Members = members, Followers = followers };
        }

        public async Task EditAsync(CityProfileDTO model, IFormFile file)
        {
            var city = await CreateCityAsync(model, file);

            _repoWrapper.City.Update(city);
            await _repoWrapper.SaveAsync();
        }

        public async Task<int> CreateAsync(CityProfileDTO model, IFormFile file)
        {
            var city = await CreateCityAsync(model, file);

            await _repoWrapper.City.CreateAsync(city);
            await _repoWrapper.SaveAsync();

            return city.ID;
        }

        private async Task<DataAccessCity.City> CreateCityAsync(CityProfileDTO model, IFormFile file)
        {
            var cityDto = model.City;
            await UploadPhotoAsync(cityDto, file);

            var city = _mapper.Map<CityDTO, DataAccessCity.City>(cityDto);
            var region = await _repoWrapper.Region.GetFirstOrDefaultAsync(r => r.RegionName == city.Region.RegionName);

            if (region == null)
            {
                region = new DataAccessCity.Region();
                region.RegionName = city.Region.RegionName;

                await _repoWrapper.Region.CreateAsync(region);
                await _repoWrapper.SaveAsync();
            }

            city.RegionId = region.ID;
            city.Region = region;

            return city;
        }

        private async Task UploadPhotoAsync(CityDTO city, IFormFile file)
        {
            var cityId = city.ID;
            var oldImageName = (await _repoWrapper.City.GetFirstOrDefaultAsync(
                predicate: i => i.ID == cityId))
                ?.Logo;
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