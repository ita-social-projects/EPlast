using EPlast.BussinessLayer.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.Interfaces.UserProfiles
{
    public interface IGenderService
    {
       IEnumerable<GenderDTO> GetAll();
    }
}
