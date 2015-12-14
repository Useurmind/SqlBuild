using System.Linq;

namespace SqlBuild.Logging
{
    public interface IErrorLog
    {
        void WriteError(string error);
    }
}