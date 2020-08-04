using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class PlastDegree
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<UserPlastDegree> UserPlastDegrees { get; set; }
        public override bool Equals(object obj)
        {
            PlastDegree plastDegree = obj as PlastDegree;
            if (plastDegree != null)
            {
                return Id == plastDegree.Id && Name == plastDegree.Name;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Id;
        }
        }
}
