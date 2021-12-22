using EPlast.Resources;
using System;

namespace EPlast.BLL.DTO.ActiveMembership
{
    public class EntryAndOathDateDTO
    {
        private DateTime _dateOath;
        private DateTime _entryDate;

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

        public DateTime EntryDate
        {
            get => _entryDate;
            set
            {
                if (value == default)
                {
                    _entryDate = value;
                    return;
                }
                if (value < AllowedDates.LowLimitDate)
                    throw new ArgumentException($"The entry date cannot be earlier than {AllowedDates.LowLimitDate}");
                _entryDate = value;
            }
        }

    }
}
