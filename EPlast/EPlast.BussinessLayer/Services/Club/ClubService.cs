using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace EPlast.BussinessLayer.Services.Club
{
    public class ClubService : IClubService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;

        public ClubService(IRepositoryWrapper repoWrapper, IMapper mapper, IHostingEnvironment env)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _env = env;
        }
        private void UpdateOrCreateAnImage(ClubDTO club, IFormFile file, string oldImageName = null)
        {
            if (file != null && file.Length > 0)
            {
                var img = Image.FromStream(file.OpenReadStream());
                var uploads = Path.Combine(_env.WebRootPath, "images\\Club");

                if (!string.IsNullOrEmpty(oldImageName))
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
                club.Logo = fileName;
            }
            else
            {
                club.Logo = oldImageName;
            }
        }
        public void Create(ClubDTO club, IFormFile file)
        {
            UpdateOrCreateAnImage(club, file);
            var modelToCreate = _mapper.Map<ClubDTO, DataAccess.Entities.Club>(club);
            _repoWrapper.Club.Create(modelToCreate);
            _repoWrapper.Save();
        }

        public IEnumerable<DataAccess.Entities.Club> GetAllClubs()
        {
            return _repoWrapper.Club.FindAll();
        }

        public IEnumerable<ClubDTO> GetAllDTO()
        {
            return _mapper.Map<IEnumerable<DataAccess.Entities.Club>, IEnumerable<ClubDTO>>(GetAllClubs());
        }

        public DataAccess.Entities.Club GetById(int id)
        {
            var club = _repoWrapper.Club
                   .FindByCondition(q => q.ID == id)
                   .FirstOrDefault();
            return club;
        }

        public ClubDTO GetByIdDTO(int id)
        {
            return _mapper.Map<DataAccess.Entities.Club, ClubDTO>(GetById(id));

        }

        public DataAccess.Entities.Club GetByIdWithDetails(int id)
        {
            var club = _repoWrapper.Club
                   .FindByCondition(q => q.ID == id)
                   .Include(c => c.ClubAdministration)
                   .ThenInclude(t => t.AdminType)
                   .Include(n => n.ClubAdministration)
                   .ThenInclude(t => t.ClubMembers)
                   .ThenInclude(us => us.User)
                   .Include(m => m.ClubMembers)
                   .ThenInclude(u => u.User)
                   .FirstOrDefault();
            return club;
        }

        public ClubDTO GetByIdWithDetailsDTO(int id)
        {
            return _mapper.Map<DataAccess.Entities.Club, ClubDTO>(GetByIdWithDetails(id));
        }

        public void Update(ClubDTO club, IFormFile file)
        {
            var oldImageName = GetById(club.ID).Logo;
            UpdateOrCreateAnImage(club, file, oldImageName);
            var modelToCreate = _mapper.Map<ClubDTO, DataAccess.Entities.Club>(club);
            _repoWrapper.Club.Update(modelToCreate);
            _repoWrapper.Save();
        }
    }
}
