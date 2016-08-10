namespace Sleemon.Common
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    public static class TypeExtensions
    {
        private static readonly Func<MethodInfo, IEnumerable<Type>> ParameterTypeProjection = (Func<MethodInfo, IEnumerable<Type>>)(method => Enumerable.Select<ParameterInfo, Type>((IEnumerable<ParameterInfo>)method.GetParameters(), (Func<ParameterInfo, Type>)(p => p.ParameterType.GetGenericTypeDefinition())));

        public static bool IsNullableType(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, params Type[] parameterTypes)
        {
            return Enumerable.SingleOrDefault<MethodInfo>(Enumerable.Where<MethodInfo>(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>)type.GetMethods(), (Func<MethodInfo, bool>)(method => method.Name == name)), (Func<MethodInfo, bool>)(method => Enumerable.SequenceEqual<Type>((IEnumerable<Type>)parameterTypes, TypeExtensions.ParameterTypeProjection(method)))));
        }
    }
}
