using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Logging;

namespace SqlBuild.Test
{
    public class ExceptionErrorLog : IErrorLog
    {
        public void WriteError(string error)
        {
            throw new Exception(error);
        }
    }
}
