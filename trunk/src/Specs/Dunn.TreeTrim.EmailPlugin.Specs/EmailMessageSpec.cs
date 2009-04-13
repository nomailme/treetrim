using System ;
using System.Collections.Generic ;
using System.Linq ;
using Xunit ;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
    public class EmailMessageSpec
    {
        public class When_storing_data : ContextSpecification
        {
            EmailMessage _emailMessage ;
            
            public override void Because()
            {
            }

            public override void EstablishContext()
            {
                _emailMessage = new EmailMessage(  );
                _emailMessage.AddAttachment( @"c:\file1.txt" );
                _emailMessage.AddAttachment( @"c:\file2.txt" );

                _emailMessage.AddRecipient( @"1@a.com" );
                _emailMessage.AddRecipient( @"2@b.com" );
            }

            [Fact]
            public void it_should_store_attachments()
            {
                IEnumerable< string > attachments = _emailMessage.Recipients ;
                Assert.Equal( 2, attachments.Count( ) ) ;
                Assert.Equal( @"1@a.com",  attachments.ElementAt( 0 ));
                Assert.Equal( @"2@b.com",  attachments.ElementAt( 1 ));
            }

            [Fact]
            public void it_should_store_recipients()
            {
                IEnumerable< string > recipients = _emailMessage.Attachments ;
                Assert.Equal( 2, recipients.Count( ) ) ;
                Assert.Equal( @"c:\file1.txt", recipients.ElementAt( 0 ) );
                Assert.Equal( @"c:\file2.txt", recipients.ElementAt( 1 ) );
            }
        }
    }
}