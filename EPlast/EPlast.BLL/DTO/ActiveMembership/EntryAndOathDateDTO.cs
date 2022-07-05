using System;
using EPlast.Resources;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class EntryAndOathDatesDto
    {
        private DateTime _dateOath;
        private DateTime _dateEntry;

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

    }
}
