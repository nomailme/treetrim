using System.Collections.Generic ;
using System.Net.Mail ;

namespace Dunn.TreeTrim.EmailPlugin
{
    public interface IEmailMessage 
    {
        IEnumerable<string> Attachments
        { 
            get ;
        }

        string Body { get ; set ; }
        MailAddress From { get ; set ; }

        IEnumerable<string> Recipients
        {
            get;
        }

        string Subject { get ; set ; }
        void AddAttachment( string path ) ;
        void AddRecipient( string recipientsEmailAddress ) ;
    }
}