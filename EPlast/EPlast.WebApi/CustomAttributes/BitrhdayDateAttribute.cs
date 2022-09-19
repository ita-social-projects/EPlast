using EPlast.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.WebApi.CustomAttributes
{
    public class BirthdayDateAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            if (date == default) 
                return true;
            
            if (date > AllowedDates.LowLimitDate || date <= DateTime.Now)
                return true;

            return false;

        }
    }
}
