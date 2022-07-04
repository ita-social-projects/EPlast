using System.Collections.Generic;

namespace EPlast.Resources
{
    public static class FunctionalityWithSpecificAccessForEvents
    {
        public static List<string> CanWhenUserIsAdmin => new List<string>() { "EditEvent", "DeleteEvent", "AddPhotos", "SeeUserTable", "ApproveParticipant" };
        public static List<string> CannotWhenEventIsApproved => new List<string>() { "EditEvent", "DeleteEvent" };
    }
}
