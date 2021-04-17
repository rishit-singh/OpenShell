using System; 
using System.Collections;
using System.Collections.Generic;

namespace OpenShell.Errors
{
	public class Error : Exception  //  Basic error 
    {
        public string Message;  //  Default exception message.
    
        public void Log()
        {
            Console.WriteLine($"Error: {this.Message }");
        }

        public static void Log(string message)
        {
            Console.WriteLine($"Error: {message}");    
        }

        public static void Log(Exception e)
        {
            Console.WriteLine($"Error: { e.Message }");
        }

        public Error()
        {
            this.Message = "Unknown error occured";             
        }

        public Error(string message)
        {
            this.Message = message; 
        }
    }
}

