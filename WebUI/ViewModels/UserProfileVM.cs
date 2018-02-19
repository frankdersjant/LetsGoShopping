using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class UserProfileVM
    {
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}