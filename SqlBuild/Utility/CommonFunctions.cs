using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Utility
{
    public static class CommonFunctions
    {
        public static void CheckNullArgument<TArgument>(string name, [ValidatedNotNull]TArgument argument)
        where TArgument : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
