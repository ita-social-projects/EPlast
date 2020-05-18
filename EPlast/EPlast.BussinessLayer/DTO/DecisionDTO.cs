using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace EPlast.BussinessLayer
{
    public class DecisionDto
    {
        public Decesion Decesion { get; set; }
        public IFormFile File { get; set; }
        public string Filename { get; set; }
    }
}