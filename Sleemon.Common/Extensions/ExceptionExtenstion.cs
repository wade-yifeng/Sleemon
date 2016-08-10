namespace Sleemon.Common
{
    using System;
    using System.Text;

    public static class ExceptionExtenstion
    {
        public static string BuildMessage(this Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                sb.AppendLine(ex.InnerException.BuildMessage());
            }

            return sb.ToString();
        }
    }
}
