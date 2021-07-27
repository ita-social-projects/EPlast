using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class RegionFollowers
    {
        public int ID { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required, MaxLength(1024, ErrorMessage = "Appeal can not exceed 1024 characters")]
        public string Appeal { get; set; }

        [Required, MaxLength(50, ErrorMessage = "City name can not exceed 50 characters")]
        public string CityName { get; set; }

        public string CityDescription { get; set; }
        public string Logo { get; set; }

        [Required]
        public int RegionId { get; set; }
        public Region Region { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Street name can not exceed 50 characters")]
        public string Street { get; set; }

        [Required, MaxLength(10, ErrorMessage = "House number can not exceed 10 characters")]
        public string HouseNumber { get; set; }

        [Required, MaxLength(10, ErrorMessage = "Office number can not exceed 10 characters")]
        public string OfficeNumber { get; set; }

        [Required, MaxLength(10, ErrorMessage = "Post index can not exceed 10 characters")]
        public string PostIndex { get; set; }

        public string СityURL { get; set; }

        [MaxLength(50, ErrorMessage = "Email can not exceed 50 characters")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "Phone number can not exceed 20 characters")]
        public string PhoneNumber { get; set; }
    }
}
