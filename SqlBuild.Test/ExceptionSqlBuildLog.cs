using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Logging;
using SqlBuild.Utility;

namespace SqlBuild.Test
{
    public class ExceptionSqlBuildLog : ISqlBuildLog
    {
        public ExceptionSqlBuildLog()
        {
            WroteAnyErrors = false;
        }

        public string WriteError(string error)
        {
            WroteAnyErrors = true;
            throw new SqlBuildException(error);
        }

        public bool WroteAnyErrors { get; private set; }


        public string WriteError(string filePath, int lineNumber, int columnNumber, string errorCode, string errorMessage)
        {
            WroteAnyErrors = true;
            throw new SqlBuildException(string.Join(", ", filePath, lineNumber, columnNumber, errorCode, errorMessage));
        }
        
        public void WriteTrace(string message)
        {
        }
    }
}
