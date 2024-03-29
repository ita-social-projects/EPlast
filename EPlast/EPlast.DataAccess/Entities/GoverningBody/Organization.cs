﻿using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody
{
    public class Organization
    {
        public int ID { get; set; }

        [MaxLength(255, ErrorMessage = "Назва повинна містити не більше 255 символів")]
        public string OrganizationName { get; set; }

        [MaxLength(1000, ErrorMessage = "Історія станиці не має перевищувати 1024 символів")]
        public string Description { get; set; }

        [StringLength(18, ErrorMessage = "Контактний номер керівного органу повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Logo { get; set; }

        public ICollection<GoverningBodyAdministration> GoverningBodyAdministration { get; set; }
        public ICollection<GoverningBodyAnnouncement> GoverningBodyAnnouncement { get; set; }
        public ICollection<GoverningBodyDocuments> GoverningBodyDocuments { get; set; }
        public ICollection<Sector.Sector> GoverningBodySectors { get; set; }
        public bool IsActive { get; set; }
        public bool IsMainStatus { get; set; }
    }
}
