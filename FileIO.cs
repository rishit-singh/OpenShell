using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OpenShell.IO
{
	public class FileIO
	{
		public static bool PushBuffer(string buffer, string filePath)
		{
			StreamWriter sw = null;
			
			try 
			{
				sw = new StreamWriter(File.Open("", FileMode.Open));
				
				sw.WriteAsync(buffer); 
			}
			catch (FileNotFoundException)
			{
				
			}

			sw.FlushAsync();

			return false;
		}
	}
}

