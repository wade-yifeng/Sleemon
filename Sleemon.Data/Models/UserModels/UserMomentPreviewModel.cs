namespace Sleemon.Data
{
    using System;

    public class UserMomentPreviewModel
    {
        public string Moment { get; set; }

        public DateTime PostTime { get; set; }

        public byte Category { get; set; }

        public int? RefId { get; set; }
    }
}
