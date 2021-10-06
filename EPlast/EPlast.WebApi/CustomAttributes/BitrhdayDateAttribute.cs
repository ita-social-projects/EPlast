using System;
using System.ComponentModel.DataAnnotations;
using EPlast.Resources;

namespace EPlast.WebApi.Validators
{
    public class BitrhdayDateAttribute: ValidationAttribute
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
