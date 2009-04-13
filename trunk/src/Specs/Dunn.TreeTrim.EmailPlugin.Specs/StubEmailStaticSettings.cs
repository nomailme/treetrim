using System ;
using System.Collections.Generic ;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
    public class StubEmailStaticSettings : IStaticSettings
    {
        #region IStaticSettings Members

        public IEnumerable< string > AdditionalAttachments
        {
            get
            {
                return new List< string >( ) ;
            }
        }

        public string Body
        {
            get;
            set;
        }
        public IEnumerable<string> RecipientsEmailAddresses
        {
            get { return new List< string >( ) ;}
        }

        public string SendersEmailAddress
        {
            get;
            set;
        }
        public string SendersEmailDisplayName
        {
            get;
            set;
        }
        public string SmtpPassword
        {
            get;
            set;
        }
        public string SmtpServerAddress
        {
            get;
            set;
        }
        public string SmtpUsername
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }

        public int TimeoutInSeconds
        {
            get; set ;
        }

        #endregion
    }
}