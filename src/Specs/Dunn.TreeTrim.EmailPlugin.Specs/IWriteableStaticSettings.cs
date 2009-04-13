using System ;
using System.Collections.Generic ;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
    public interface IWriteableStaticSettings : IStaticSettings
    {
// ReSharper disable UnusedMember.Global
        new IEnumerable< string > AdditionalAttachments { get ; set ;}
        new string Body { get ; set ;}
        new IEnumerable< string > RecipientsEmailAddresses { get ; set ;}
        new string SendersEmailAddress { get ; set ;}
        new string SendersEmailDisplayName { get ; set ;}
        new string SmtpPassword { get ; set ;}
        new string SmtpServerAddress { get ; set ;}
        new string SmtpUsername { get ; set ;}
        new string Subject { get ; set ;}
        new int TimeoutInSeconds { get ; set ;}
// ReSharper restore UnusedMember.Global
    }
}