using EPlast.WebApi.Models.User;

namespace EPlast.WebApi.Models.Approver
{
    public class ApproverViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public UserInfoViewModel User { get; set; }
    }
}
