using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Organization
    {
        public int ID { get; set; }

        public string OrganizationName { get; set; }
    }
}