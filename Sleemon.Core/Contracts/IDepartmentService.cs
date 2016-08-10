namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IDepartmentService
    {
        SyncResult SyncDepartment(IEnumerable<DepartmentSyncModel> departments);

        IList<DepartmentModel> GetAllActivedDepartment();
    }
}