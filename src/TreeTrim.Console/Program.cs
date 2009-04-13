using System;
using System.Collections.Generic;
using System.Globalization ;
using System.IO;
using System.Linq;
using System.Text ;
using Dunn.TreeTrim;

namespace TreeTrim.Console
{
    class Program
    {
        static void Main(string[ ] args)
        {
            try
            {
                if( args.Length == 0 )
                {
                    throw new InvalidOperationException(
                        "No solution folder path was provided. Please provide a folder path as the first parameter to this executable." );
                }

                string path = args[ 0 ];

                if( !Directory.Exists( path ) )
                {
                    string message = string.Format( "Solution folder path '{0}' doesn't exist.", path );
                    throw new InvalidOperationException( message );
                }

                var commandLineArgs = new List< string >(
                    from string arg in args
                    select arg.ToLower( CultureInfo.CurrentCulture ) ) ;
                
                commandLineArgs.RemoveAt( 0 );

                IDiscoverPlugins pluginDiscoverer = new DiscoverPluginsInAssemblyDirectory( );

                Trimmer.TrimTree(
                    new TaskCollection( pluginDiscoverer.DiscoveredPlugins, commandLineArgs ),
                    path );
            }
            catch( Exception exception )
            {
                System.Console.WriteLine( getExceptionMessage(exception));
            }
        }

        static string getExceptionMessage( Exception exception )
        {
            var message = new StringBuilder();
            do
            {
                message.AppendLine( exception.Message )
                    .AppendLine( exception.StackTrace )
                    .AppendLine( @"--------" )
                    .AppendLine( ).AppendLine( );
                
                exception = exception.InnerException ;
                
            } while ( exception != null ) ;

            return message.ToString( ) ;
        }
    }
}
