using System ;
using System.Collections.Generic ;
using System.Globalization ;
using System.Linq ;
using System.Xml.Linq ;
using System.Xml.XPath ;

namespace Dunn.TreeTrim.EmailPlugin
{
    public class StaticSettings : IStaticSettings
    {
        readonly List< string > _attachments ;
        readonly List< string > _recipients ;

        public StaticSettings( )
        {
            XDocument document = SettingsHelper.LoadAndValidate( @"EmailPlugin.settings" ) ;
            _attachments = new List< string >( ) ;
            _recipients = new List< string >( ) ;

            populateSenderAndReceiver( document ) ;
            populateEmailContent( document ) ;
            populateSmtpSettings( document ) ;
            populateAnyAdditionalAttachments( document ) ;
        }

        public IEnumerable<string> AdditionalAttachments
        {
            get
            {
                return _attachments;
            }
        }
        public string Body { get; private set ; }
        public IEnumerable<string> RecipientsEmailAddresses
        {
            get
            {
                return _recipients;
            }
        }
        public string SendersEmailAddress { get; private set ; }
        public string SendersEmailDisplayName { get; private set ; }
        public string SmtpPassword { get; private set; }
        public string SmtpServerAddress { get; private set; }
        public string SmtpUsername { get; private set; }
        public string Subject { get; private set ; }
        public int TimeoutInSeconds{ get ; private set ;}

        static string valueOrEmpty( XNode node, string xpath )
        {
            XElement selectedElement = node.XPathSelectElement( xpath ) ;
            if( selectedElement == null )
            {
                return string.Empty ;
            }

            return selectedElement.Value ;
        }

        void populateAnyAdditionalAttachments( XNode document )
        {
            _attachments.AddRange( new List< string >(
                                   from XElement a in document.XPathSelectElements(
                                       @"Settings/AdditionalAttachments/Attachment" )
                                   select a.Value
                               ) ) ;
        }

        void populateEmailContent( XNode node )
        {
            Subject = valueOrEmpty(node,@"Settings/Subject");
            Body = valueOrEmpty(node,@"Settings/Body");
        }

        void populateSenderAndReceiver( XNode node )
        {
            SendersEmailDisplayName = valueOrEmpty( node, @"Settings/Sender/DisplayName" ) ;
            SendersEmailAddress = valueOrEmpty(node,@"Settings/Sender/EmailAddress");
            
            _recipients.AddRange(
                new List< string >( from XElement e in node.XPathSelectElements( @"Settings/Recipients/Recipient" )
                select e.Value ) ) ;
        }

        void populateSmtpSettings( XNode node )
        {
            SmtpServerAddress = valueOrEmpty(node,@"Settings/Smtp/ServerAddress");
            SmtpUsername = valueOrEmpty(node, @"Settings/Smtp/Credentials/Username");
            SmtpPassword = valueOrEmpty(node, @"Settings/Smtp/Credentials/Password");
            XElement element = node.XPathSelectElement( @"Settings/Smtp/TimeoutInSeconds" ) ;
            TimeoutInSeconds = element == null 
                ? 60 
                : Convert.ToInt32( element.Value, CultureInfo.InvariantCulture ) ;
        }
    }
}