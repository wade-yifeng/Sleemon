namespace Sleemon.Common
{
    using System.Linq;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;

    using Microsoft.Practices.Unity.InterceptionExtension;

    public class ParameterValidationCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null || getNext == null)
            {
                return null;
            }

            string validationError;
            if (!ValidateArguments(input.MethodBase.Name, input.Arguments, out validationError))
            {
                return
                    input.CreateExceptionMethodReturn(
                        new InvalidArgumentException(string.Format("{0};StatusCode:{1}", validationError,
                            (int) StatusCode.InvalidArgument)));
            }

            return getNext().Invoke(input, getNext);
        }

        private static bool ValidateArguments(string methodName, IParameterCollection parameterCollection, out string validationError)
        {
            for (int i = 0; i < parameterCollection.Count; i++)
            {
                var info = parameterCollection.GetParameterInfo(i);
                var parameterValue = parameterCollection[i];

                foreach (var attr in info.GetCustomAttributes(true))
                {
                    if (attr is NotNullAttribute
                        && !ValidateNotNullArgument(methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }

                    if (attr is NotNullOrWhiteSpaceAttribute
                        && !ValidateNotNullOrWhiteSpaceArgument(methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }

                    if (attr is NotZeroAttribute
                        && !ValidateNotZeroArgument(methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }

                    var rangeCheckAttribute = attr as RangeCheckAttribute;
                    if (rangeCheckAttribute != null
                        && !ValidateRangeCheckArgument(rangeCheckAttribute, methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }

                    if (attr is NotNullCollectionMembersAttribute
                        && !ValidateNotNullCollectionMembersArument(methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }

                    var notNullMemberAttribute = attr as NotNullMemberAttribute;
                    if (notNullMemberAttribute != null
                        && !ValidateNotNullMemberArgument(notNullMemberAttribute, methodName, out validationError, parameterValue, info))
                    {
                        return false;
                    }
                }
            }

            validationError = null;
            return true;
        }

        private static bool ValidateNotNullMemberArgument(NotNullMemberAttribute notNullMemberAttribute, string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            validationError = string.Empty;

            if (parameterValue == null)
            {
                validationError = string.Format(
                    CultureInfo.InvariantCulture,
                    "In method {0}, the argument \"{1}\" is null",
                    methodName,
                    info.Name);
                return false;
            }

            var memberInfo = info.ParameterType.GetMember(notNullMemberAttribute.MemberName);
            if (memberInfo.Length == 0)
            {
                return false;
            }

            if (memberInfo[0].MemberType == MemberTypes.Property)
            {
                PropertyInfo pi = parameterValue.GetType().GetProperty(notNullMemberAttribute.MemberName);
                if (pi.GetValue(parameterValue, null) == null)
                {
                    validationError = string.Format(
                        CultureInfo.InvariantCulture,
                        "In method {0}, the argument \"{1}\" has a member \"{2}\" that is null",
                        methodName,
                        info.Name,
                        notNullMemberAttribute.MemberName);
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateNotNullCollectionMembersArument(string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            var coll = parameterValue as IEnumerable;
            if (coll != null)
            {
                if (coll.Cast<object>().Any(item => item == null))
                {
                    validationError = string.Format(
                        CultureInfo.InvariantCulture,
                        "In method {0}, the argument \"{1}\" has collection members that are null",
                        methodName,
                        info.Name);
                    return false;
                }
            }

            validationError = null;
            return true;
        }

        private static bool ValidateRangeCheckArgument(RangeCheckAttribute attr, string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            bool inRange = true;
            if (parameterValue is int)
            {
                inRange = (int)parameterValue >= attr.LowRange && (int)parameterValue <= attr.HighRange;
            }
            else if (parameterValue is short)
            {
                inRange = (short)parameterValue >= attr.LowRange && (short)parameterValue <= attr.HighRange;
            }
            else if (parameterValue is long)
            {
                inRange = (long)parameterValue >= attr.LowRange && (long)parameterValue <= attr.HighRange;
            }

            if (!inRange)
            {
                validationError = string.Format(
                    CultureInfo.InvariantCulture,
                    "In method {0}, the argument \"{1}\" value of \"{2}\"is outside the valid range of ({3}:{4})",
                    methodName,
                    info.Name,
                    parameterValue,
                    attr.LowRange,
                    attr.HighRange);
                return false;
            }

            validationError = null;
            return true;
        }

        private static bool ValidateNotZeroArgument(string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            bool isZero = false;
            if (parameterValue is int)
            {
                isZero = (int)parameterValue == 0;
            }
            else if (parameterValue is short)
            {
                isZero = (short)parameterValue == 0;
            }
            else if (parameterValue is long)
            {
                isZero = (long)parameterValue == 0;
            }

            if (isZero)
            {
                validationError = string.Format(
                    CultureInfo.InvariantCulture,
                    "In method {0}, the argument \"{1}\" cannot be zero",
                    methodName,
                    info.Name);
                return false;
            }

            validationError = null;
            return true;
        }

        private static bool ValidateNotNullOrWhiteSpaceArgument(string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            if (info.ParameterType == typeof(string) && string.IsNullOrWhiteSpace(parameterValue as string))
            {
                validationError = string.Format(
                    CultureInfo.InvariantCulture,
                    "In method {0}, the argument \"{1}\" cannot be null or empty",
                    methodName,
                    info.Name);
                return false;
            }

            validationError = string.Empty;
            return true;
        }

        private static bool ValidateNotNullArgument(string methodName, out string validationError, object parameterValue, ParameterInfo info)
        {
            if (parameterValue == null)
            {
                validationError = string.Format(
                    CultureInfo.InvariantCulture,
                    "In method {0}, the argument \"{1}\" cannot be null",
                    methodName,
                    info.Name);
                return false;
            }

            validationError = null;
            return true;
        }
    }
}
