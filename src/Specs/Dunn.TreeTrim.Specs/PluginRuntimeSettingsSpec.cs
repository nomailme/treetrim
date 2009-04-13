using Xunit ;

namespace Dunn.TreeTrim.Specs
{
    public class PluginRuntimeSettingsSpec
    {
        public class when_storing_valid_values
        {
            [Fact]
            public void should_not_care_about_case_when_using_contains()
            {
                var runtimeSettings = new PluginRuntimeSettings(@"Key1:Value1+KEY2:Value2");
                Assert.True(runtimeSettings.ContainsPropertyNamed(@"key1"));
                Assert.True(runtimeSettings.ContainsPropertyNamed(@"KEY2"));
            }

            [Fact]
            public void should_return_value()
            {
                var runtimeSettings = new PluginRuntimeSettings(@"Key1:Value1+KEY2:Value2");
                Assert.Equal(@"Value1", runtimeSettings[@"key1"]);
                Assert.Equal(@"Value2", runtimeSettings[@"key2"]);
            }

            [Fact]
            public void should_store_keys_with_no_value()
            {
                var runtimeSettings = new PluginRuntimeSettings(@"justdoit+KEY2:Value2");
                Assert.True(runtimeSettings.ContainsPropertyNamed(@"justdoit"));
                Assert.Equal(string.Empty, runtimeSettings[@"justdoit"]);
                Assert.True(runtimeSettings.ContainsPropertyNamed(@"KEY2"));
            }
        }
    }
}