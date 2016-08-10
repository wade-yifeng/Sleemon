namespace Sleemon.Data
{
    public struct CustomColumnInfo
    {
        public string Name { get; set; }
        public string SqlType { get; set; }
        public string ExpressionTemplate { get; set; }

        public string GetSelection(string targetTable)
        {
            if (string.IsNullOrEmpty(targetTable))
            {
                targetTable = string.Empty;
            }
            else
            {
                targetTable = string.Concat(targetTable, ".");
            }

            if (string.IsNullOrEmpty(this.ExpressionTemplate))
            {
                return string.Format(@"{1}[{0}]", this.Name, targetTable);
            }
            else
            {
                return string.Format(@"{1} AS [{0}]", this.Name, string.Format(this.ExpressionTemplate, targetTable));
            }
        }

        public string Creation
        {
            get
            {
                return string.Format(@"[{0}] {1} NULL", this.Name, this.SqlType);
            }
        }
    }
}
