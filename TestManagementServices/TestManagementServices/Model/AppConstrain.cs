using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestManagementServices.Model
{
    public class AppConstrain
    {
        public const bool includeLowercase = true;
        public const bool includeUppercase = true;
        public const bool includeNumeric = true;
        public const bool includeSpecial = true;
        public const bool includeSpaces = false;
        public const int lengthOfPassword = 8;
    }
}
