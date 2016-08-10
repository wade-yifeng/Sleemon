namespace Sleemon.Data
{
    using System;
    using System.Linq.Expressions;

    using Sleemon.Common;

    public class TrainingSearchContext
    {
        public string Subject { get; set; }

        public string Location { get; set; }

        public DateTime? StartFrom { get; set; }

        public DateTime? EndTo { get; set; }

        public bool? IsPublic { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public Expression<Func<Training, bool>> GenerateSearchConditions()
        {
            Expression<Func<Training, bool>> searchConditions = p => p.IsActive;

            if (!string.IsNullOrEmpty(this.Subject))
            {
                searchConditions = searchConditions.And(p => p.Subject.Contains(this.Subject));
            }

            if (!string.IsNullOrEmpty(this.Location))
            {
                searchConditions = searchConditions.And(p => p.Location.Contains(this.Location));
            }

            if (this.StartFrom.HasValue)
            {
                searchConditions = searchConditions.And(p => p.StartFrom >= this.StartFrom.Value);
            }

            if (this.EndTo.HasValue)
            {
                searchConditions = searchConditions.And(p => p.EndTo <= this.EndTo.Value);
            }

            return searchConditions;
        }
    }
}
