using System;

namespace Common.Helper.FormatHelper
{
    public class TrimFormatter : IFormatProvider, ICustomFormatter
    {
        // The GetFormat method of the IFormatProvider interface.
        // This must return an object that provides formatting services for the specified type.
        public object GetFormat(Type format)
        {
            if (format == typeof(ICustomFormatter)) return this;
            else return null;
        }

        // The Format method of the ICustomFormatter interface.
        // This must format the specified value according to the specified format settings.
        public string Format(string format, object arg, IFormatProvider provider)
        {
            string formatValue = arg.ToString();

            return formatValue.TrimEnd();
        }
    }
}