using System.Collections.Generic;

namespace EPlast.Resources
{
    public static class FunctionalityWithSpecificAccessForEvents
    {
        public static List<string> canWhenUserIsAdmin = new List<string>() { "EditEvent", "DeleteEvent", "AddPhotos", "SeeUserTable", "ApproveParticipant" };
        public static List<string> cannotWhenEventIsApproved = new List<string>() { "EditEvent", "DeleteEvent"};
    }
}
