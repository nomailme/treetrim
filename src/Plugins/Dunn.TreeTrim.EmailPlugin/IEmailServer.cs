namespace Dunn.TreeTrim.EmailPlugin
{
    public interface IEmailServer
    {
        void Send( IEmailMessage emailMessage ) ;
        
        void Initialise( 
            string address, 
            string logOnName, 
            string logOnPassword, 
            int timeoutInSeconds ) ;
    }
}