using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Net.Mail ;

namespace Dunn.TreeTrim.EmailPlugin
{
    public class EmailMessage : IEmailMessage
    {
        readonly List< string > _attachmentPaths ;
        readonly List< string > _recipients ;

        public EmailMessage( )
        {
            _recipients = new List< string >();
            _attachmentPaths = new List< string >();
        }

        #region IEmailMessage Members

        public void AddRecipient( string recipientsEmailAddress )
        {
            _recipients.Add( recipientsEmailAddress ) ;
        }

        public MailAddress From { get ; set ;}

        public string Body { get ; set ; }

        public string Subject { get ; set ;}

        public void AddAttachment( string path )
        {
            _attachmentPaths.Add( path );
        }

        public IEnumerable<string> Recipients
        {
            get
            {
                return _recipients ;
            }
        }

        public IEnumerable< string > Attachments
        {
            get
            {
                return new ReadOnlyCollection< string >( _attachmentPaths ) ;
            }
        }

        #endregion
    }
}