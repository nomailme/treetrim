using System;
using System.ComponentModel.Composition;
using System.Globalization ;
using System.Net.Mail;

namespace Dunn.TreeTrim.EmailPlugin
{
    [Export(typeof(IPlugin))]
    public class Plugin : IPlugin
    {
        IPluginRuntimeSettings _pluginSettings;
        readonly IStaticSettings _staticSettings;
        string _workingPath;
        readonly IEmailServer _emailServer ;
        readonly IEmailMessage _emailMessage ;
        readonly IDisk _disk ;

        public Plugin( )
            :this(new SmtpServer(  ), new EmailMessage(  ), new StaticSettings(  ), new Disk(  )) 
        {
        }

        public Plugin( 
            IEmailServer emailServer, 
            IEmailMessage emailMessage, 
            IStaticSettings staticSettings,
            IDisk disk)
        {
            _emailServer = emailServer ;
            _emailMessage = emailMessage ;
            _staticSettings = staticSettings ;
            _disk = disk ;
        }

        #region IPlugin Members

        public void Run(IPluginRuntimeSettings settings, IPlugin lastPlugin)
        {
            _workingPath = lastPlugin.WorkingPath ;

            _pluginSettings = settings ;

            populateSender( ) ;
            populateRecipients( ) ;
            populateEmailContent( ) ;
            includeAttachmentFromPreviousPlugin( lastPlugin) ;
            includeAnyAdditionalAttachments(  ) ;

            send(  ) ;
        }

        public void Cleanup()
        {
        }

        public string WorkingPath
        {
            get { return _workingPath; }
        }

        public string Moniker
        {
            get { return @"email"; }
        }

        #endregion

        static string stringOrEmpty(Func<string> func)
        {
            string v = func();
            if (string.IsNullOrEmpty(v))
            {
                return string.Empty;
            }

            return v;
        }

        void includeAnyAdditionalAttachments( )
        {
            if( _pluginSettings.ContainsPropertyNamed( @"additionalAttachments" ) )
            {
                string[ ] indidualAttachmentPaths = _pluginSettings[@"additionalAttachments"].Split( ';' );
                foreach( string eachAttachmentPath in indidualAttachmentPaths )
                {
                    _emailMessage.AddAttachment( eachAttachmentPath );
                }
            }
            else
            {
                foreach ( string eachAttachment in _staticSettings.AdditionalAttachments )
                {
                    _emailMessage.AddAttachment( eachAttachment );
                }
            }
        }

        void includeAttachmentFromPreviousPlugin( IPlugin lastPlugin)
        {
            if( _pluginSettings.ContainsPropertyNamed( @"excludeOutputFromPreviousTask" ) )
            {
                return ;
            }

            string lastWorkingPath = lastPlugin.WorkingPath ;

            if( _disk.IsFile( lastWorkingPath ))
            {
                _emailMessage.AddAttachment(lastWorkingPath );
            }
            else
            {

                foreach ( string eachFilePath in _disk.GetFilesInDirectoryRecursively(lastWorkingPath) )
                {
                    _emailMessage.AddAttachment(eachFilePath ) ;
                }
            }
        }

        void populateEmailContent( )
        {
            _emailMessage.Body = resolve(@"body", () => stringOrEmpty(() => _staticSettings.Body)) ;
            _emailMessage.Subject = resolve(@"subject", () => stringOrEmpty(() => _staticSettings.Subject)) ;
        }

        void populateRecipients( )
        {
            if(_pluginSettings.ContainsPropertyNamed( @"recipients" ))
            {
                string[] recipients = _pluginSettings[ @"recipients" ].Split( ',' ) ;
                
                foreach( string eachRecipientEmailAddress in recipients )
                {
                    _emailMessage.AddRecipient( eachRecipientEmailAddress );
                }
            }
            else
            {
                foreach ( string eachRecipientEmailAddress in _staticSettings.RecipientsEmailAddresses )
                {
                    _emailMessage.AddRecipient( eachRecipientEmailAddress );
                }
            }
        }

        void populateSender( )
        {
            _emailMessage.From = resolveSender() ;
        }


        string resolve(string fieldName, Func<string> funcForDefaultValue)
        {
            return _pluginSettings.ContainsPropertyNamed(fieldName)
                ? _pluginSettings[fieldName]
                : funcForDefaultValue();
        }

        MailAddress resolveSender()
        {
            string fromEmailAddress = resolve(@"sendersEmail", () => _staticSettings.SendersEmailAddress);
            string fromEmailDisplayName = resolve(@"sendersName", () => _staticSettings.SendersEmailDisplayName);

            if (string.IsNullOrEmpty(fromEmailDisplayName))
            {
                fromEmailDisplayName = fromEmailAddress;
            }

            return new MailAddress(fromEmailAddress, fromEmailDisplayName);
        }

        void send( )
        {
            int timeout  ;
            if(_pluginSettings.ContainsPropertyNamed( @"timeout" ))
            {
                timeout = Convert.ToInt32(
                    _pluginSettings[ @"timeout" ],
                    CultureInfo.InvariantCulture ) ;
            }
            else
            {
                timeout = _staticSettings.TimeoutInSeconds ;
            }

            _emailServer.Initialise(
                resolve( @"serverAddress", ( ) => _staticSettings.SmtpServerAddress ),
                resolve( @"userName", ( ) => _staticSettings.SmtpUsername ),
                resolve( @"password", ( ) => _staticSettings.SmtpPassword ),
                timeout ) ;


            _emailServer.Send( _emailMessage );
        }
    }
}
