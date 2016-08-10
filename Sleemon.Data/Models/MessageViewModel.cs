namespace Sleemon.Data
{
    using Newtonsoft.Json;
    using System.Configuration;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public Receiver Receivers { get; set; }

        public News News { get; set; }
    }

    public class Receiver
    {
        public string ToUsers { get; set; }

        public string ToDepts { get; set; }
    }

    public class News
    {
        protected readonly string STATIC_RESOURCES_DOMAIN = ConfigurationManager.AppSettings["STATIC_RESOURCES_DOMAIN"];

        public string Title { get; set; }

        public string Description { get; set; }

        public int Url { get; set; }

        private string _picUrl;

        public string PicUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_picUrl))
                {
                    _picUrl = STATIC_RESOURCES_DOMAIN + _picUrl;
                }
                return _picUrl;
            }
            set
            {
                _picUrl = value;
            }
        }
    }
}
