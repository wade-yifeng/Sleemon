namespace Sleemon.Data
{
    using System.Collections.Generic;

    using Sleemon.Common;

    public class UserProfile : UserBaseModel
    {
        private string _password;

        public string Position { get; set; }

        public string Mobile { get; set; }

        public byte Gender { get; set; }

        public string Email { get; set; }

        public string WeixinId { get; set; }

        public string Avatar { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public List<int> Department { get; set; }

        public int Status { get; set; }

        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(this._password))
                {
                    this._password = EncryptUtility.MD5(this.UserId);
                }

                return this._password;
            }
            set { this._password = EncryptUtility.MD5(this.UserId); }
        }
    }
}
