namespace Sleemon.Data
{
    public class UserViewModel : UserBaseModel
    {
        public string Avatar { get; set; }

        public string Mobile { get; set; }

        public byte Grade { get; set; }

        public int Point { get; set; }

        public int ProductAbility { get; set; }

        public int SalesAbility { get; set; }

        public int ExhibitAbility { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsSuperAdmin { get; set; }

        public string Position { get; set; }

        public string Email { get; set; }
    }
}
