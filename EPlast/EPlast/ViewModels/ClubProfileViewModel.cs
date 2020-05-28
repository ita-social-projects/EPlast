using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using EPlast.ViewModels.UserInformation.UserProfile;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubProfileViewModel
    {
        public ClubViewModel Club { get; set; }
        public UserViewModel ClubAdmin { get; set; }
        public List<ClubMembersViewModel> Members { get; set; }
        public List<ClubMembersViewModel> Followers { get; set; }
        public ICollection<ClubAdministrationViewModel> ClubAdministration { get; set; }
    }
    /*
    public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Club name cannot exceed 50 characters")]
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        [MaxLength(1024, ErrorMessage = "Club description cannot exceed 1024 characters")]
        public string Description { get; set; }
        public string Logo { get; set; }
        public ICollection<ClubMembersViewModel> ClubMembers { get; set; }
        public ICollection<ClubAdministrationViewModel> ClubAdministration { get; set; }


     public int ID { get; set; }
        public string ClubName { get; set; }
        public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

      public class ClubViewModel_ChooseAClub
    {
    public int ID { get; set; }
    public string ClubName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }

     public class ClubViewModel_Club
    {
    public int ID { get; set; }
    public string ClubName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

        ?public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
    }

    public class ClubViewModel_ClubAdmins
    {
    public int ID { get; set; }
    public string ClubName { get; set; }
        public User ClubAdmin { get; set; }

    public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
    }
     public class ClubViewModel_ClubDescription
    {
    public int ID { get; set; }
    public string ClubURL { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

        ?public ICollection<ClubMembers> ClubMembers { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
    }

     public class ClubViewModel_clubFollowers
    {
        public int ID { get; set; }
        public User ClubAdmin { get; set; }
        public List<ClubMembers> Members { get; set; }
        public List<ClubMembers> Followers { get; set; }
    }
     public class ClubViewModel_clubMembers
    {
        public int ID { get; set; }
        public User ClubAdmin { get; set; }
        public List<ClubMembers> Members { get; set; }
    }

    public class ClubViewModel_CreateClub
    {
    public int ID { get; set; }
    public string ClubURL { get; set; }    
    public string ClubName { get; set; }
        public string Description { get; set; }
    ?public string Logo { get; set; }
    }

     public class ClubViewModel_EditClub
    {
    public int ID { get; set; }
    public string ClubURL { get; set; }    
    public string ClubName { get; set; }
        public string Description { get; set; }
    public string Logo { get; set; }

    public class ClubViewModel_Index
    {
    public int ID { get; set; }   
    public string ClubName { get; set; }
    public string Logo { get; set; }
    }
     */
}
