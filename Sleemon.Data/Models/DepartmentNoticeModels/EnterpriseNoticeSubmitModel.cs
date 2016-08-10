namespace Sleemon.Data
{
    using System;

    public class EnterpriseNoticeSubmitModel : EnterpriseNoticeBaseModel
    {
        public string Summary { get; set; }

        public short Category { get; set; }

        public override string AvatarPath { get; set; }

        public string Context { get; set; }

        public Int32 LastUpdateUser { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public bool IsActive { get; set; }

        public ResultBase Result { get; set; }
    }
}
