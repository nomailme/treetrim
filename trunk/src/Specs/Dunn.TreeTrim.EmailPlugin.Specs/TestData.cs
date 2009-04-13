using System ;

namespace Dunn.TreeTrim.EmailPlugin.Specs
{
    static class TestData
    {
        public static string PathFromLastPlugin
        {
            get
            {
                return @"c:\file.zip" ;
            }
        }

        public static string AdditionalAttachment1
        {
            get
            {
                return AttachmentsDirectory + @"\1.txt";
            }
                
        }
        public static string AdditionalAttachment2
        {
            get
            {
                return AttachmentsDirectory + @"\2.txt";
            }
        }

        public static string SmtpServerAddress
        {
            get
            {
                return @"1.2.3.4";
            }
        }

        public static string SmtpUsername
        {
            get
            {
                return @"Joe Bloggs" ;
            }
        }

        public static string SmtpPassword
        {
            get { return @"password123" ; }
        }

        public static int TimeoutInSeconds
        {
            get
            {
                return 666 ;
            }
        }

        public static string RecipientEmailAddress1
        {
            get
            {
                return @"1@a.com" ;
            }
        }

        public static string RecipientEmailAddress2
        {
            get
            {
                return @"2@b.com" ;
            }
        }

        public static string SendersEmailAddress
        {
            get { return @"sender@sender.com" ; }
        }

        public static string SendersDisplayName 
        {
            get
            {
                return @"My Name" ;
            }
        }

        public static string Body
        {
            get { return @"this is the body" ; }
        }

        public static string Subject
        {
            get
            {
                return @"this is the subject" ;
            }
        }

        public static string AttachmentsDirectory
        {
            get
            {
                return @"c:\temp" ;
            }
        }
    }
}