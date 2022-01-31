using System;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyTableViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AdminType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GovernBodyName { get; set; }
        public bool Status { get; set; }
    }
}
