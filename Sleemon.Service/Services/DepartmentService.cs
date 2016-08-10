namespace Sleemon.Service
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;

    public class DepartmentService : IDepartmentService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public DepartmentService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public SyncResult SyncDepartment(IEnumerable<DepartmentSyncModel> departments)
        {
            var enumerable = departments.ToList();

            var result = new SyncResult()
            {
                Quantity = enumerable.Count,
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };

            this._invoicingEntities.spSyncDepartment(Utilities.GetXElementFromObject(enumerable).ToString());

            return result;
        }

        public IList<DepartmentModel> GetAllActivedDepartment()
        {
            return this._invoicingEntities.Database.SqlQuery<DepartmentModel>(@"
                SELECT P.[UniqueId]   AS [Id]
	                  ,P.[ParentId]   AS [ParentId]
	                  ,P.[Name]
                      ,P.[Order]
                      ,p.[Level]
                FROM [dbo].[Department] P
                WHERE P.[IsActive] = 1").ToList();
        }
    }
}
