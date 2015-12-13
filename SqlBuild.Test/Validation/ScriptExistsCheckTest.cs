using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;
using SqlBuild.Validation;

using Xunit;

namespace SqlBuild.Test.Validation
{
    public class ScriptExistsCheckTest
    {
        [Fact]
        public void does_not_throw_for_existing_script()
        {
            new ScriptExistsCheck(new SqlScript()
                                      {
                                          ItemSpec = @"TestData\existing_script.sql",
                                          Identity = @"TestData\existing_script.sql"
                                      });
        }

        [Fact]
        public void does_throw_for_non_existing_script()
        {
            Assert.Throws<Exception>(() => new ScriptExistsCheck(new SqlScript()
                                                                     {
                                                                         ItemSpec = @"TestData\non_existing_script.sql",
                                                                         Identity = @"TestData\non_existing_script.sql"
                                                                     }));
        }
    }
}
