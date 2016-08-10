namespace Sleemon.Data
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.Data.Linq.Mapping;
    using System.Collections.Generic;

    using Sleemon.Common;

    public abstract class Entity
    {
        public virtual string GenerateInsertQuery(IEnumerable<CustomColumnInfo> additionalReturnColumns, string outTableVariable = null)
        {
            string table;
            List<string> columns;
            List<string> values;
            GenerateInsertColumnsAndValues(this, out table, out columns, out values);

            return string.Format(@"{4}
INSERT INTO {0} ({1})
OUTPUT INSERTED.*{3}{5}
VALUES ({2})",
                table,
                string.Join(",", columns),
                string.Join(",", values),
                string.Join(@"", additionalReturnColumns.Select(col => string.Format(@",{0}", col.GetSelection(@"INSERTED")))),
                string.IsNullOrEmpty(outTableVariable) ? string.Empty : string.Concat(GetTableCreationQuery(this.GetType().Name, outTableVariable, additionalReturnColumns), "\r\n"),
                string.IsNullOrEmpty(outTableVariable) ? string.Empty : string.Format(@" INTO @{0}", outTableVariable));
        }

        public static string GenerateInsertQuery<T>(IEnumerable<T> entities, IEnumerable<CustomColumnInfo> additionalReturnColumns, string outTableVariable = null)
            where T : Entity
        {
            var queryBuilder = new StringBuilder();

            string table;
            List<string> columns = null;
            List<string> values;
            foreach (var entity in entities)
            {
                GenerateInsertColumnsAndValues(entity, out table, out columns, out values);

                if (queryBuilder.Length == 0)
                {
                    queryBuilder.AppendFormat(@"{3}
INSERT {0} ({1})
OUTPUT INSERTED.*{2}{4}
SELECT * FROM (
    VALUES",
                        table,
                        string.Join(@",", columns),
                        string.Join(@"", additionalReturnColumns.Select(col => string.Format(@",{0}", col.GetSelection(@"INSERTED")))),
                        string.IsNullOrEmpty(outTableVariable) ? string.Empty : GetTableCreationQuery(typeof(T).Name, outTableVariable, additionalReturnColumns),
                        string.IsNullOrEmpty(outTableVariable) ? string.Empty : string.Format(@" INTO @{0}", outTableVariable));
                }

                queryBuilder.AppendFormat(@"
    ({0}),",
                    string.Join(",", values));
            }

            if (columns != null)
            {
                queryBuilder.Remove(queryBuilder.Length - 1, 1);
                queryBuilder.AppendFormat(@"
) AS [WaitingForInsert] ({0})",
                    string.Join(@",", columns));
            }

            return queryBuilder.ToString();
        }

        private static IEnumerable<PropertyInfo> GetCanBeInsertedProperties(Type type)
        {
            foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                var columnAttribute = pi.GetCustomAttribute<ColumnAttribute>();
                if ((columnAttribute == null)
                    || (columnAttribute.IsDbGenerated == true)
                    || ((columnAttribute.Expression ?? string.Empty).Equals(@"COMPUTED")))
                {
                    continue;
                }

                yield return pi;
            }
        }

        private class TableCreation
        {
            public string ColumnsDefined { get; set; }
        }
        private static string GetTableCreationQuery(string tableName, string tableVariableName, IEnumerable<CustomColumnInfo> additionalReturnColumns)
        {
            using (var context = new SleemonEntities())
            {
                var creations = context.Database.SqlQuery<TableCreation>(string.Format(@"
SELECT STUFF(REPLACE(REPLACE(
(
	SELECT
		CONCAT(
			'[', [name], '] ',
			TYPE_NAME([user_type_id]),
			CASE WHEN [system_type_id] NOT IN (34,35,36,40,41,42,43,48,52,56,58,59,60,61,62,98,99,104,108,122,127,240,189,241) THEN CONCAT('(', CASE WHEN [system_type_id] IN (231,239) THEN [max_length] / 2 ELSE [max_length] END, ')') ELSE '' END,
			CASE WHEN [is_nullable] = 1 THEN ' NULL' ELSE '' END
		) AS [ColumnDefined]
	FROM sys.all_columns AS [Columns]
	WHERE [object_id] = OBJECT_ID('{0}')
	FOR XML AUTO
), '<Columns ColumnDefined=""', ','), '""/>', ''), 1, 1, '') AS [ColumnsDefined]",
                    tableName)).ToList();
                if (creations.Count > 0)
                {
                    return string.Format(@"DECLARE @{0} TABLE ({1}{2})",
                        tableVariableName,
                        creations[0].ColumnsDefined,
                        string.Join("", additionalReturnColumns.Select(col => string.Format(@",{0}", col.Creation))));
                }
            }
            return null;
        }

        private static void GenerateInsertColumnsAndValues<T>(T entity, out string tableName, out List<string> columns, out List<string> values)
            where T : Entity
        {
            var type = entity.GetType();

            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if ((tableAttribute == null)
                || string.IsNullOrEmpty(tableAttribute.Name))
            {
                tableName = string.Format(@"[dbo].[{0}]", type.Name);
            }
            else
            {
                tableName = tableAttribute.Name;
            }

            columns = new List<string>();
            values = new List<string>();

            foreach (var pi in GetCanBeInsertedProperties(type))
            {
                var columnAttribute = pi.GetCustomAttribute<ColumnAttribute>();
                var value = pi.GetValue(entity);

                columns.Add(columnAttribute.Name.GetColumnName());
                if (value == null)
                {
                    if (columnAttribute.CanBeNull)
                    {
                        values.Add(@"NULL");
                    }
                    else
                    {
                        ////TODO: needs to get the default value
                        values.Add(@"''");
                    }
                }
                else
                {
                    switch (columnAttribute.DbType)
                    {
                        case "Boolean":
                            values.Add(string.Format(@"{0}", ((bool)value) ? 1 : 0));
                            break;
                        case "Int32":
                        case "Double":
                            values.Add(string.Format(@"{0}", value.ToString()));
                            break;
                        default:
                            values.Add(string.Format(@"'{0}'", value.ToString()));
                            break;
                    }
                }
            }
        }
    }
}
