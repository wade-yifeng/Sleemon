namespace Sleemon.Common
{
    using System;

    /// <summary>
    /// Provides range checking for numeric parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RangeCheckAttribute : Attribute
    {
        /// <summary>
        /// holds the low range value
        /// </summary>
        private readonly int lowRange;

        /// <summary>
        /// holds the high range value
        /// </summary>
        private readonly int highRange;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeCheckAttribute"/> class.
        /// </summary>
        /// <param name="lowRange">the low value of the allowed range</param>
        /// <param name="highRange">the high value of the allowed range</param>
        public RangeCheckAttribute(int lowRange, int highRange)
        {
            this.lowRange = lowRange;
            this.highRange = highRange;
        }

        /// <summary>
        /// Gets the low value of the allowed range
        /// </summary>
        public int LowRange
        {
            get
            {
                return this.lowRange;
            }
        }

        /// <summary>
        /// Gets the high value of the allowed range
        /// </summary>
        public int HighRange
        {
            get
            {
                return this.highRange;
            }
        }
    }
}
