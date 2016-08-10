namespace Sleemon.Common
{
    using System;

    public class LogHelper<TSendType> where TSendType : class
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(TSendType));

        private static readonly string titleName = typeof(TSendType).Name;

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">内容</param>
        public static void WriteDebug(string message)
        {
            log.Debug(titleName + ":" + message);
        }

        /// <summary>
        /// 记录一般日志
        /// </summary>
        /// <param name="message">内容</param>
        public static void WriteInfo(string message)
        {
            log.Info(titleName + ":" + message);
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">内容</param>
        public static void WriteWarn(string message)
        {
            log.Warn(titleName + ":" + message);
        }
        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">日志内容</param>
        public static void WriteWarn(string title, string msg)
        {
            log.Warn(title + ":" + msg);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void WriteException(string message)
        {
            log.Error(titleName + ":" + message);
        }

        public static void WriteException(Exception ex)
        {
            log.Error(titleName, ex);
        }
    }
}
