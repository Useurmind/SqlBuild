using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuild.Model
{
    public class KeyedModel : IModel
    {
        public string Key { get; set; }
    }
}
