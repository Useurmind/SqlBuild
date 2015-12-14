using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Utility
{
    [Serializable]
    public class SqlBuildException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SqlBuildException()
        {
        }

        public SqlBuildException(string message)
            : base(message)
        {
        }

        public SqlBuildException(string message, params object[] parameters)
            : base(string.Format(message, parameters))
        {
        }

        public SqlBuildException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SqlBuildException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
