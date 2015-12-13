using System;
using System.Linq;
using System.Linq.Expressions;

using SqlBuild.Utility;

namespace SqlBuild.Model
{
    public static class ModelExtensions
    {
        public static string GetMetadataName<TModel, TProperty>(Expression<Func<TModel, TProperty>> propertyExpression)
        {
            string propertyName = Property.Get(propertyExpression);
            return GetMetadataName(propertyName);
        }

        public static string GetMetadataName(this IModel model, string propertyName)
        {
            return GetMetadataName(propertyName);
        }

        public static string GetMetadataName(string propertyName)
        {
            if (propertyName.EndsWith(Constants.KeyPropertyPostfix))
            {
                return propertyName.Remove(propertyName.Length - 3);
            }

            return propertyName;
        }
    }
}