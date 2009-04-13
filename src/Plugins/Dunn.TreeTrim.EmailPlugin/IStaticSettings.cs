using System.Collections.Generic ;

namespace Dunn.TreeTrim.EmailPlugin
{
    public interface IStaticSettings 
    {
        IEnumerable<string> AdditionalAttachments { get ; }
        string Body { get ; }
        IEnumerable<string> RecipientsEmailAddresses { get ; }
        string SendersEmailAddress { get ; }
        string SendersEmailDisplayName { get ; }
        string SmtpPassword { get ; }
        string SmtpServerAddress { get ; }
        string SmtpUsername { get ; }
        string Subject { get ; }
        int TimeoutInSeconds { get ; }
    }
}