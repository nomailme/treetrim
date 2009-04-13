using System ;
using System.Collections.Generic ;
using System.Net.Mail ;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
    public class StubEmailMessage : IEmailMessage
    {
        readonly List< string > _attachments ;
        readonly List<string> _recipients;

        public StubEmailMessage( )
        {
            _attachments = new List< string >();
            _recipients = new List< string >();
        }

        #region IEmailMessage Members

        public IEnumerable<string> Attachments
        {
            get
            {
                return _attachments ;
            }
        }

        public string Body
        {
            get;
            set;
        }

        public MailAddress From
        {
            get;
            set;
        }

        public IEnumerable<string> Recipients
        {
            get
            {
                return _recipients;
            }
        }

        public string Subject
        {
            get;
            set;
        }

        public void AddAttachment(string path)
        {
            _attachments.Add( path ) ;
        }

        public void AddRecipient(string recipientsEmailAddress)
        {
            _recipients.Add( recipientsEmailAddress );
        }

        #endregion
    }
}