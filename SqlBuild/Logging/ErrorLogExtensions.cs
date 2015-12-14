using System.Linq;

namespace SqlBuild.Logging
{
    public static class ErrorLogExtensions
    {
        public static void WriteReferencedElementNotFound<TParent, TReference>(this IErrorLog errorLog, string parentKey, string referenceKey)
        {
            errorLog.WriteError(string.Format(Errors.ReferencedElementNotFound, typeof(TParent).Name, parentKey,
                typeof(TReference).Name, referenceKey));
        }

    }
}