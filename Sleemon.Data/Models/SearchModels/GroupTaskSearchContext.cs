namespace Sleemon.Data
{
    using System;
    using System.Linq.Expressions;
    using Sleemon.Common;

    public class GroupTaskSearchContext
    {
        public string Title { get; set; }

        public int RequiredGrade { get; set; }

        public int Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public Expression<Func<GroupTask, bool>> GenerateSearchConditions()
        {
            Expression<Func<GroupTask, bool>> searchConditions = p => p.IsActive;
            
            if (!string.IsNullOrEmpty(this.Title))
            {
                searchConditions = searchConditions.And(p => p.Title.Contains(this.Title));
            }

            if (this.RequiredGrade > 0)
            {
                searchConditions = searchConditions.And(p => p.RequiredGrade == this.RequiredGrade);
            }

            if (this.Status > 0)
            {
                searchConditions = searchConditions.And(p => p.Status == this.Status);
            }

            return searchConditions;
        }
    }
}
