using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Region
{
    public class RegionDTO
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Назва округи не має перевищувати 50 символів.")]
        public string RegionName { get; set; }
        [MaxLength(1000, ErrorMessage = "Опис округи не має перевищувати 1000 символів.")]
        public string Description { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<RegionDocumentDTO> Documents { get; set; }
        [Required, MaxLength(18, ErrorMessage = "Контактний номер округи не може перевищувати 12 символів.")]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Місто розташування округи не може перевищувати 50 символів.")]
        public string City { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Електронна адреса округи не може перевищувати 50 символів.")]
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Вулиця розташування округи не може перевищувати 50 символів.")]
        public string Street { get; set; }
        public bool CanCreate { get; set; }
        [Required, MaxLength(5, ErrorMessage = "Номер будинку розташування округи не може перевищувати 5 символів.")]
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        [Required, Range(10000, 99999, ErrorMessage = "Поштовий індекс округи має мати 5 цифр.")]
        public int PostIndex { get; set; }
        public RegionsStatusTypeDTO Status { get; set; }
    }
}
