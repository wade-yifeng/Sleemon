namespace Sleemon.Common
{
    using System;
    using Microsoft.SqlServer.Types;

    [CLSCompliant(true)]
    public interface IHierarchyEntity
    {
        int UniqueId { get; set; }

        SqlHierarchyId HierarchyId { get; }

        SqlHierarchyId ParentHierarchyId { get; set; }
    }
}
