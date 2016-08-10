namespace Sleemon.Data
{
    using System;

    using Newtonsoft.Json;

    public class UserDailySignInModel : UserBaseModel
    {
        public int StatusCode { get; set; }

        public DateTime SignInDate { get; set; }

        public int Point { get; set; }

        public string Message { get; set; }
    }
}
