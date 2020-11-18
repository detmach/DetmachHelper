using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Detmach
{
    public static class NumberHelper
    {
        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}
