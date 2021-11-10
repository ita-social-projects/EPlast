using EPlast.Resources;
using System;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserOathDateDTO
    {
        
        private DateTime _dateOath;

        public string UserId { get; set; }
        public int Id { get; set; }


        public DateTime DateOath
        {
            get => _dateOath;
            set
            {
                if (value == default)
                {
                    _dateOath = value;
                    return;
                }
                if (value < AllowedDates.LowLimitDate)
                    throw new ArgumentException($"The oath date cannot be earlier than {AllowedDates.LowLimitDate}");
                _dateOath = value;
            }
        }

    }
}
