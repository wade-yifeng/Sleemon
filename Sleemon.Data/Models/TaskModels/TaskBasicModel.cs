namespace Sleemon.Data
{
    using System;

    public class TaskBasicModel
    {
        public int TaskId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte TaskCategory { get; set; }

        public DateTime StartFrom { get; set; }

        public DateTime? EndTo { get; set; }

        public int Point { get; set; }

        public int OverduePoint { get; set; }

        public int ProductAbility { get; set; }

        public int SalesAbility { get; set; }

        public int ExhibitAbility { get; set; }

        public int BelongTo { get; set; }

        public byte Status { get; set; }

        public string LastUpdateUser { get; set; }

        public string LastUpdateUserName { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
