using System ;
using System.Collections.Generic ;
using System.Linq ;
using Xunit ;

namespace Dunn.TreeTrim.Specs
{
    public class TaskCollectionSpecs
    {
        public class When_used_correctly : ContextSpecification
        {
            TaskCollection _tasks;

            public override void EstablishContext()
            {
                _tasks = new TaskCollection(
                    new StubPluginDiscoverer( @"gzip", @"email" ).DiscoveredPlugins,
                    new List<string>
                    {
                        @"-gzip:",
                        @"-email:something:with:colons"
                    } );
            }

            public override void Because()
            {

            }

            [Fact]
            public void it_should_have_the_correct_amount_of_tasks()
            {
                Assert.Equal( 2, _tasks.Count( ) );
            }

            [Fact]
            public void it_should_store_all_arguments()
            {
                ITask gzipItem = _tasks.ElementAt( 0 );
                Assert.Equal( @"gzip", gzipItem.Moniker );
                Assert.Equal( string.Empty, gzipItem.SettingsValue );

                ITask emailItem = _tasks.ElementAt( 1 );
                Assert.Equal( @"email", emailItem.Moniker );
                Assert.Equal( @"something:with:colons", emailItem.SettingsValue );
            }

            [Fact]
            public void it_should_iterate_over_the_tasks()
            {
                IEnumerator<ITask> enumerator = _tasks.GetEnumerator( );
                Assert.True( enumerator.MoveNext( ) );
                Assert.True( enumerator.MoveNext( ) );

                Assert.False( enumerator.MoveNext( ) );
            }

            [Fact]
            public void it_should_handle_values_with_colons()
            {
                ITask task = _tasks.ElementAt( 1 );

                Assert.Equal( @"something:with:colons", task.SettingsValue );
            }

        }

        public class When_a_task_has_complex_arguments : ContextSpecification
        {
            TaskCollection _tasks;

            public override void EstablishContext()
            {
                _tasks = new TaskCollection(
                    new StubPluginDiscoverer( @"email" ).DiscoveredPlugins,
                    new List<string>
                        {
                            @"-email:from:steve@dunnhq.com+to:joe@bloggs.com+subject:""Emailing {{FileName}}""+body:""This is the body""+otherAttachments:""c:\temp\disclaimer.txt;c:\temp\copyright.txt"""
                        }
                    );
            }

            public override void Because()
            {

            }

            [Fact]
            public void it_should_store_the_moniker()
            {
                ITask task = _tasks.First( );
                Assert.Equal( @"email", task.Moniker );
            }

            [Fact]
            public void it_should_store_the_whole_settings_value()
            {
                ITask task = _tasks.First( );

                Assert.Equal(
                    @"from:steve@dunnhq.com+to:joe@bloggs.com+subject:""Emailing {{FileName}}""+body:""This is the body""+otherAttachments:""c:\temp\disclaimer.txt;c:\temp\copyright.txt""",
                    task.SettingsValue );
            }

            [Fact]
            public void it_should_store_the_parameters()
            {
                ITask task = _tasks.First( );

                Assert.Equal( @"steve@dunnhq.com", task[ @"from" ] );
                Assert.Equal( @"joe@bloggs.com", task[ @"to" ] );
                Assert.Equal( @"""Emailing {{FileName}}""", task[ @"subject" ] );
                Assert.Equal( @"""This is the body""", task[ @"body" ] );
                Assert.Equal( @"""c:\temp\disclaimer.txt;c:\temp\copyright.txt""", task[ @"otherAttachments" ] );
            }
        }
    }
}