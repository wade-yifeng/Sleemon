using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmitMapper;
using EmitMapper.MappingConfiguration;


namespace Sleemon.Common
{
    /// <summary>
    /// 实体映射帮助类
    /// </summary>
    public class EmitHelper
    {
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TResult ConvertTo<TSource, TResult>(TSource source) where TResult : new()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TResult>();
            return mapper.Map(source);
        }

        /// <summary>
        /// 列表转换
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TResult> ListConvertTo<TSource, TResult>(List<TSource> list) where TResult : new()
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<TSource>, List<TResult>>();
            return mapper.Map(list);
        }
    }
}
