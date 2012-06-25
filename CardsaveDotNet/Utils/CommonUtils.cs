using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardsaveDotNet.Utils
{
    public static class CommonUtils
    {
        public static bool AreNullOrEmpty(params string[] stringsToValidate) {
            bool result = false;
            Array.ForEach(stringsToValidate, str => {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }
    }
}
