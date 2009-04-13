using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Mail ;
using System.Text ;

namespace Dunn.TreeTrim.EmailPlugin
{
    public class SmtpServer : IEmailServer
    {
        string _address ;
        string _password ;
        int _timeoutInSeconds ;
        string _username ;

        public void Send( IEmailMessage emailMessage )
        {
            var smtp = new SmtpClient
                           {
                               Credentials = new NetworkCredential( _username, _password ),
                               Host = _address,
                               Timeout = _timeoutInSeconds*1000 
                           } ;

            using ( var mailMessage = new MailMessage
                                          {
                                              From = emailMessage.From, 
                                              Body = emailMessage.Body, 
                                              Subject = emailMessage.Subject,
                                          } )
            {
                mailMessage.To.Add( recipientsToCommaSeparatedString( emailMessage.Recipients ) );

                foreach ( string eachAttachmentPath in emailMessage.Attachments )
                {
                    mailMessage.Attachments.Add( new Attachment( eachAttachmentPath ) );    
                }

                smtp.Send( mailMessage );
            }
        }

        public void Initialise( 
            string address, 
            string logOnName, 
            string logOnPassword, 
            int timeoutInSeconds )
        {
            _address = address ;
            _username = logOnName ;
            _password = logOnPassword ;
            _timeoutInSeconds = timeoutInSeconds ;
        }

        static string recipientsToCommaSeparatedString( IEnumerable< string > recipients )
        {
            var builder = new StringBuilder( );
            foreach( string eachRecipient in recipients )
            {
                builder.Append( eachRecipient ).Append( ',' );
            }

            return builder.ToString( );
        }
    }
}