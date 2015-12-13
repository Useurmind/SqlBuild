using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlBuild.Model;

using Xunit;

namespace SqlBuild.Test.Model
{
    public class ModelExtensionsTest
    {
        [Fact]
        public void metadata_name_for_content_property_is_property_name()
        {
            string metaDataName = ModelExtensions.GetMetadataName("SomeContentProperty");

            Assert.Equal("SomeContentProperty", metaDataName);
        }

        [Fact]
        public void metadata_name_for_key_property_is_property_name_minus_key_postfix()
        {
            string metaDataName = ModelExtensions.GetMetadataName("OtherObjectKey");

            Assert.Equal("OtherObject", metaDataName);
        }
    }
}
