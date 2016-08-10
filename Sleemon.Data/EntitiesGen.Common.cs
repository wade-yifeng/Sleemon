namespace Sleemon.Data
{
    using System.Linq;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;

    public static class ObjectContextExtensions

    {
        #region Public Methods and Operators

        public static void DiscardChanges(this ObjectContext dbContext)
        {
            // delete added objects that did not get saved
            foreach (var entry in dbContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added))
            {
                if (entry.Entity != null)
                {
                    dbContext.DeleteObject(entry.Entity);
                }
            }

            // Refetch modified objects from database
            var entities =
                dbContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted)
                    .Where(entry => entry.Entity != null)
                    .ToList();
            if (entities.Count > 0)
            {
                dbContext.Refresh(RefreshMode.StoreWins, entities);
            }

            dbContext.AcceptAllChanges();
        }

        #endregion
    }
}
