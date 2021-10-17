using System;
using EPlast.Resources;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class UserMembershipDatesDTO
    {
        private DateTime _dateEntry;
        private DateTime _dateOath;
        private DateTime _dateEnd;

        public string UserId { get; set; }
        public int Id { get; set; }

        public DateTime DateEntry
        {
            get => _dateEntry;
            set
            {
                if (value == default)
                {
                    _dateEntry = value;
                    return;
                }
                if (value < AllowedDates.LowLimitDate)
                    throw new ArgumentException($"The entry date cannot be earlier than {AllowedDates.LowLimitDate}");
                _dateEntry = value;
            }
        }

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
                if (value < AllowedDates.LowLimitDate || value < _dateEntry)
                    throw new ArgumentException($"The oath date cannot be earlier than {AllowedDates.LowLimitDate}, " +
                                                $"and remember entry date < oath date < end date ");
                _dateOath = value;
            }
        }

        public DateTime DateEnd
        {
            get => _dateEnd;
            set
            {
                if (value == default)
                {
                    _dateEnd = value;
                    return;
                }
                if (value < AllowedDates.LowLimitDate || value < _dateOath || value < _dateEntry)
                    throw new ArgumentException($"The end date cannot be earlier than {AllowedDates.LowLimitDate}, " +
                                                $"and remember entry date < oath date < end date ");
                _dateEnd = value;
            }
        }

       
    }
}
