using System.Collections.Generic;
using System.Linq;

namespace SqlBuild.Logging
{
    public class ErrorLog : IErrorLog
    {
        private IList<string> errors;

        public ErrorLog()
        {
            this.errors = new List<string>();
        }

        public void WriteError(string error)
        {
            this.errors.Add(error);
        }
    }
}
