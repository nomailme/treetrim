using System.Collections.Generic;
using System.Linq ;
using Microsoft.Practices.Unity;
using Moq;
using Xunit;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

    public class EmailPluginSpecs
    {
        public class When_passing_parameters : ContextSpecification
        {
            Mock< IEmailServer > _emailServer ;
            Plugin _plugin ;
            IPluginRuntimeSettings _runtimeSettings ;

            public override void Because()
            {
                _plugin.Run(
                    _runtimeSettings,
                    mockLastPlugin( @"c:\file.zip" ) );
            }

            public override void EstablishContext()
            {
                using( var container = new UnityContainer( ) )
                {
                    _emailServer = new Mock< IEmailServer >( ) ;
                    container.RegisterType< IEmailMessage, StubEmailMessage >( )
                        .RegisterType< IStaticSettings, StubEmailStaticSettings >( )
                        .RegisterInstance( mockedDisk( ) )
                        .RegisterInstance( _emailServer.Object ) ;

                    _plugin  = container.Resolve< Plugin >( ) ;

                    _runtimeSettings = stubRuntimeSettings(
                        @"additionalAttachments:c:\temp\1.txt;c:\temp\2.txt+serverAddress:1.2.3.4+userName:Joe Bloggs+password:password123+timeout:666+recipients:1@a.com,2@b.com+sendersEmail:sender@sender.com+sendersName:My Name+body:this is the body+subject:this is the subject" ) ;
                }
            }

            [Fact]
            public void it_should_use_the_additional_attachments_in_the_email()
            {
                _emailServer.Verify(
                    server => server.Send( It.Is< IEmailMessage >( 
                        message => matchCollections( 
                            message.Attachments, 
                            new List<string>
                                {
                                    @"c:\file.zip", @"c:\temp\1.txt", @"c:\temp\2.txt"
                                } ) ) ),
                    Times.Exactly( 1 ) ) ;
            }

            [Fact]
            public void it_should_use_the_body_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is< IEmailMessage >( s => matchIt( s.Body, @"this is the body" ) ) ),
                    Times.Exactly( 1 ) ) ;
            }

            [Fact]
            public void it_should_use_the_recipients_addresses_in_the_email()
            {
                _emailServer.Verify(
                    server => server.Send( It.Is<IEmailMessage>(
                        message => matchCollections(
                            message.Recipients,
                            new List<string> { @"1@a.com",@"2@b.com" } ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_senders_email_address_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is< IEmailMessage >( s => matchIt( s.From.Address, @"sender@sender.com" ) ) ),
                    Times.Exactly( 1 ) ) ;
            }

            [Fact]
            public void it_should_use_the_senders_name_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is< IEmailMessage >( s => matchIt( s.From.DisplayName, @"My Name" ) ) ),
                    Times.Exactly( 1 ) ) ;
            }

            [Fact]
            public void it_should_use_the_server_address_and_logon_credentials()
            {
                _emailServer.Verify( t => t.Initialise( 
                    @"1.2.3.4",
                    @"Joe Bloggs", 
                    @"password123", 
                    666 ) ) ;
            }

            [Fact]
            public void it_should_use_the_subject_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is< IEmailMessage >( s => matchIt( s.Subject, @"this is the subject" ) ) ),
                    Times.Exactly( 1 ) ) ;
            }

            static bool matchCollections<T>( IEnumerable<T> func, IEnumerable<T> expected )
            {
                Assert.Equal(expected.ToArray(  ), func.ToArray(  ) );
                return true ;
            }

            static bool matchIt<T>( T value, T expected )
            {
                Assert.Equal( expected, value );
                return true ;
            }

            static IDisk mockedDisk( )
            {
                var disk = new Mock< IDisk >();
                disk.Setup( t => t.IsFile( @"c:\temp\1.txt" ) ).Returns( true ) ;
                disk.Setup( t => t.IsFile( @"c:\temp\2.txt" ) ).Returns( true ) ;
                disk.Setup( t => t.IsFile( @"c:\file.zip" ) ).Returns( true ) ;
                disk.Setup( t => t.IsFile( @"c:\temp" ) ).Returns( false ) ;
                return disk.Object ;
            }

            static IPlugin mockLastPlugin(string path)
            {
                return new NullPlugin( path );
            }

            static IPluginRuntimeSettings stubRuntimeSettings(string value)
            {
                return new PluginRuntimeSettings( value );
            }
        }

        public class When_using_static_settings : ContextSpecification
        {
            Mock<IEmailServer> _emailServer;
            Plugin _plugin;
            IPluginRuntimeSettings _runtimeSettings;

            public override void Because()
            {
                _plugin.Run(
                    _runtimeSettings,
                    mockLastPlugin( TestData.PathFromLastPlugin ) );
            }

            public override void EstablishContext()
            {
                using( var container = new UnityContainer( ) )
                {
                    var mockStaticSettings = new Mock< IWriteableStaticSettings >( ) ;

                    mockStaticSettings.SetupProperty( t => t.AdditionalAttachments,
                        new List< string > { TestData.AdditionalAttachment1, TestData.AdditionalAttachment2 } ) ;

                    mockStaticSettings.SetupProperty( t => t.SmtpServerAddress, TestData.SmtpServerAddress ) ;
                    mockStaticSettings.SetupProperty( t => t.SmtpUsername, TestData.SmtpUsername ) ;
                    mockStaticSettings.SetupProperty( t => t.SmtpPassword, TestData.SmtpPassword ) ;
                    mockStaticSettings.SetupProperty( t => t.TimeoutInSeconds, TestData.TimeoutInSeconds ) ;
                    mockStaticSettings.SetupProperty( t => t.RecipientsEmailAddresses,
                        new List< string > { TestData.RecipientEmailAddress1, TestData.RecipientEmailAddress2 } ) ;

                    mockStaticSettings.SetupProperty( t => t.SendersEmailAddress, TestData.SendersEmailAddress ) ;
                    mockStaticSettings.SetupProperty( t => t.SendersEmailDisplayName, TestData.SendersDisplayName ) ;
                    mockStaticSettings.SetupProperty( t => t.Body, TestData.Body ) ;
                    mockStaticSettings.SetupProperty( t => t.Subject, TestData.Subject ) ;

                    _emailServer = new Mock<IEmailServer>( );

                    container.RegisterType<IEmailMessage, StubEmailMessage>( )
                        .RegisterInstance<IStaticSettings>( mockStaticSettings.Object )
                        .RegisterInstance( mockedDisk( ) ) 
                        .RegisterInstance( _emailServer.Object ) ;

                    _plugin = container.Resolve< Plugin >( ) ;

                    _runtimeSettings = stubRuntimeSettings( string.Empty ) ;
                }
            }

            [Fact]
            public void it_should_use_the_additional_attachments_in_the_email()
            {
                _emailServer.Verify(
                    server => server.Send( It.Is<IEmailMessage>(
                        message => matchCollections(
                            message.Attachments,
                            new List<string>
                                {
                                    @"c:\file.zip", @"c:\temp\1.txt", @"c:\temp\2.txt"
                                } ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_body_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is<IEmailMessage>( s => matchIt( s.Body, @"this is the body" ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_recipients_addresses_in_the_email()
            {
                _emailServer.Verify(
                    server => server.Send( It.Is<IEmailMessage>(
                        message => matchCollections(
                            message.Recipients,
                            new List<string> { @"1@a.com", @"2@b.com" } ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_senders_email_address_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is<IEmailMessage>( s => matchIt( s.From.Address, @"sender@sender.com" ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_senders_name_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is<IEmailMessage>( s => matchIt( s.From.DisplayName, @"My Name" ) ) ),
                    Times.Exactly( 1 ) );
            }

            [Fact]
            public void it_should_use_the_server_address_and_logon_credentials()
            {
                _emailServer.Verify( t => t.Initialise(
                    @"1.2.3.4",
                    @"Joe Bloggs",
                    @"password123",
                    666 ) );
            }

            [Fact]
            public void it_should_use_the_subject_in_the_email()
            {
                _emailServer.Verify(
                    t => t.Send( It.Is<IEmailMessage>( s => matchIt( s.Subject, @"this is the subject" ) ) ),
                    Times.Exactly( 1 ) );
            }

            static bool matchCollections<T>(IEnumerable<T> func, IEnumerable<T> expected)
            {
                Assert.Equal( expected.ToArray( ), func.ToArray( ) );
                return true;
            }

            static bool matchIt<T>(T value, T expected)
            {
                Assert.Equal( expected, value );
                return true;
            }

            static IDisk mockedDisk()
            {
                var disk = new Mock<IDisk>( );
                disk.Setup( t => t.IsFile( TestData.AttachmentsDirectory ) ).Returns( false );
                disk.Setup( t => t.IsFile( TestData.AdditionalAttachment1 ) ).Returns( true );
                disk.Setup( t => t.IsFile( TestData.AdditionalAttachment2 ) ).Returns( true );

                disk.Setup( t => t.IsFile( TestData.PathFromLastPlugin ) ).Returns( true );
                return disk.Object;
            }

            static IPlugin mockLastPlugin(string path)
            {
                return new NullPlugin( path );
            }

            static IPluginRuntimeSettings stubRuntimeSettings(string value)
            {
                return new PluginRuntimeSettings( value );
            }
        }
    }
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global
}