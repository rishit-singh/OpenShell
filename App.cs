using System;
using System.Collections; 
using System.Collections.Generic; 
using System.IO;
using System.Runtime.Serialization;
using OpenShell.Serialization;
using OpenShell.Errors;
using OpenShell.Tools;

namespace OpenShell
{	
	public delegate void AppFunction();
	public delegate string sAppFunction(string[] x);

	public interface IApplication
	{
		string Run();
	}

	public struct AppConfiguration	//	Stores the app configuration
	{
		public string AppName;	//	Name of the app 
		
		public Command AppCommand; 

		public AppConfiguration(string appName, Command appCommand)
		{
			this.AppName = appName;
			this.AppCommand = appCommand;
		}
	}

	[Serializable]
	public class Application : IApplication
	{
		public AppConfiguration Configuration;	//	Stores the app config for current instance

		public sAppFunction ApplicationFunction;	//	delegate to the main app function
	
		public string Run()	//	Starts the app function
		{
			this.ApplicationFunction(this.Configuration.AppCommand.Parameters);

			return null;
		}

		public Application()
		{	
			this.Configuration = new AppConfiguration();
			this.ApplicationFunction = new sAppFunction((string[] x) => null);
		}

		public Application(AppConfiguration configuration, sAppFunction appFunction)
		{	
			this.Configuration = configuration;
			this.ApplicationFunction = appFunction;
		}
	}

	/// <summary>
	/// Data to be set and accessed during the execution of the program
	/// </summary>
	public class ExecutionData
	{	
		public static string ApplicationBinaryPath = "../Data/";
		
		/// <summary>
		/// Serializes the Application instances in the Applications stack into binary files.
		/// </summary>
		/// <returns> Serialization state represented by a bool. </returns>
		public static bool SerializeApplicaions()
		{
			Application[] StackArray = ExecutionData.Applications.ToArray();

			for (int x = 0; x < StackArray.Length; x++)
				try	
				{
					Serializer.SerializeToBinary<Application>(StackArray[x], $"{ExecutionData.ApplicationBinaryPath}{StackArray[x].Configuration.AppName}.osh");
				}
				catch (SerializationException e)
				{
					Errors.Error.Log(e);
				}

			return false; 
		}

		/// <summary>
		/// Deserializes all the applications from the default directory and pushed them to the Applications stack.
		/// </summary>
		public static bool DeserializeApplicaions()
		/// <returns> Deserialization success state represented by a bool. </returns>
		{
			string[] ApplicationBinaryFiles = ArrayTools.IEnumerableToArray<string>(Directory.EnumerateFiles(ExecutionData.ApplicationBinaryPath));
			if (ApplicationBinaryFiles.Length == 0)
				return false;

			for (int x = 0; x < ApplicationBinaryFiles.Length; x++)
				ExecutionData.Applications.Push(Serializer.DeserializeBinary<Application>(ApplicationBinaryFiles[x]));

			return true; 
		}

		public static Stack<Application> Applications = new Stack<Application>();
	} 
}