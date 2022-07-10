using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventCreationDto
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public string EventName { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public string Description { get; set; }
        public string Questions { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        private DateTime _eventDateStart;
        public DateTime EventDateStart { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public DateTime EventDateEnd
        {
            get { return _eventDateStart; }
            set
            {
                if (value > EventDateStart)
                    _eventDateStart = value;
            }
        }
        [Required(ErrorMessage = "This field can't be empty")]
        public string Eventlocation { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public int EventTypeID { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public int EventCategoryID { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public int EventStatusID { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public string FormOfHolding { get; set; }
        [Required(ErrorMessage = "This field can't be empty")]
        public string ForWhom { get; set; }
        [Required]
        [Range(2, 200,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int NumberOfPartisipants { get; set; }
    }
}
