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
	public delegate string sAppFunction(string[] x);	//	App function delegate that accepts raw and unformatted string as a parameter.
	public delegate string aAppFunction(ApplicationArgument x);//	App function delegate that excepts formatted ApplicatioArgument as parameter.

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

	public struct ApplicationArgument
	{
		public Command.Flag[] Flags,
							SingleFlags;
		
		public string[] RawValues; 	//	Stores all the raw values 
		
		public ApplicationArgument(Command.Flag[] flags, Command.Flag[] singleFlags, string[] rawValues)
		{
			this.Flags = flags;
			this.SingleFlags = singleFlags;
			this.RawValues = rawValues;
		}

		public ApplicationArgument(Command.Flag[] flags, Command.Flag[] singleFlags)
		{
			this.Flags = flags;
			this.SingleFlags = singleFlags;
			this.RawValues = null;
		}

		public ApplicationArgument(Command.Flag[] flags)
		{
			this.Flags = flags;
			this.SingleFlags = null;
			this.RawValues = null;
		}
	}

	[Serializable]
	public class Application : IApplication
	{
		public AppConfiguration Configuration;	//	Stores the app config for current instance

		public aAppFunction ApplicationFunction;	//	delegate to the main app function

		/// <summary>
		/// Returns the flag containing the provided flag string.
		/// </summary>
		/// <returns> Returns the flag with the provided flagstring. </returns>
		private Command.Flag GetFlag(string flagString)
		{
			int indexTemp;

			return ((indexTemp = ArrayTools.BinarySearch(flagString, this.Configuration.AppCommand.Flags)) == -1) ? new Command.Flag() : this.Configuration.AppCommand.Flags[indexTemp];
		}

		/// <summary>
		/// Creates an array of flags in such an order that flags with values appear first in the array following the ones
		/// without values and the raw values.
		/// </summary>
		/// <returns></returns>
		public ApplicationArgument GetApplicationArgument()
		{
			ApplicationArgument argument;

			Stack<Command.Flag> FlagStack = new Stack<Command.Flag>(),
				SingleFlags = new Stack<Command.Flag>();
			
			Stack<string> RawValueStack = new Stack<string>();

			Command.Flag FlagTemp;	// = new Command.Flag();

			try
			{
				for (int x = 0; x < this.Configuration.AppCommand.Parameters.Length; x++)
					if (this.Configuration.AppCommand.Parameters[x][0] == '-')
						if ((FlagTemp = this.GetFlag(this.Configuration.AppCommand.Parameters[x])).IsNull())	//	Flag doesnt exist  
							continue;
						else if (FlagTemp.FlagString != null && FlagTemp.Value == null)
							SingleFlags.Push(FlagTemp);
						else if (FlagTemp.FlagString != null && FlagTemp.Value != null)
							RawValueStack.Push(FlagTemp.Value);
						else
							throw new InvalidFlagException();
			}
			catch (InvalidFlagException e)
			{
				Program.Debug.Log(e.Message);

				return new ApplicationArgument();	//	returns a null instance
			}

			argument = new ApplicationArgument(ArrayTools.InvertArray<Command.Flag>(FlagStack.ToArray()), SingleFlags.ToArray(),  RawValueStack.ToArray());

			return argument;
		}

		/// <summary>
		/// Formats raw paramter strings to ApplicationArgument
		/// </summary>
		/// <param name="parameters"> Parameter string array. </param>
		/// <returns> Generated ApplicationArgument. </returns>
		public ApplicationArgument GetApplicationArgument(string[] parameters)
		{
			ApplicationArgument applicationArgument = new ApplicationArgument();

			Command.Flag FlagTemp = new Command.Flag();

			Stack<Command.Flag> FlagStack = new Stack<Command.Flag>(),
								SingleFlagStack = new Stack<Command.Flag>(); 

			Stack<string> RawValueStack = new Stack<string>();


			if (parameters.Length == 0)
				return applicationArgument;

			try
			{
				for (int x = 0; x < parameters.Length; x++)
					if (parameters[x] != null)
						if (parameters[x][0] == '-' || $"{parameters[x][0]}{parameters[x][1]}" == "--")		//	Checks for the flag prefixes.
							if (!(FlagTemp = this.GetFlag(StringTools.GetSubString(parameters[x], 1, parameters[x].Length))).IsEqual(new Command.Flag()))	//	Flag null check.
								if (FlagTemp.IsSingle())
									SingleFlagStack.Push(FlagTemp);
								else
									FlagStack.Push(FlagTemp);
							else
								throw new UnkownFlagException(FlagTemp.FlagString);

						else
							RawValueStack.Push(parameters[x]);
			}
			catch (UnkownFlagException e)
			{
				e.Log();

				return new ApplicationArgument();
			}

			applicationArgument = new ApplicationArgument(ArrayTools.InvertArray<Command.Flag>(FlagStack.ToArray()), ArrayTools.InvertArray<Command.Flag>(SingleFlagStack.ToArray()), ArrayTools.InvertArray<string>(RawValueStack.ToArray()));
			
			return applicationArgument;
		}

		public ApplicationArgument GetApplicationArgument(Command.Flag[] flags)
		{
			ApplicationArgument argument;

			Stack<Command.Flag> FlagStack = new Stack<Command.Flag>(),
				SingleFlags = new Stack<Command.Flag>();
			
			Stack<string> RawValueStack = new Stack<string>();

			Command.Flag FlagTemp;	// = new Command.Flag();

			try
			{
				for (int x = 0; x < flags.Length; x++)
					if (flags[x].IsNull())	//	Flag doesnt exist  
						continue;
					else if (flags[x].FlagString != null && flags[x].Value != null )
					{
						FlagStack.Push(flags[x]);
					}
					else if (flags[x].FlagString != null && flags[x].Value == null)
						SingleFlags.Push(flags[x]);
					else if (flags[x].FlagString == null && flags[x].Value != null)
						RawValueStack.Push(flags[x].Value);
					else
					{
						Program.Debug.Log($"FlagString: {flags[x].FlagString}\nValue: {flags[x].Value}\n");
						throw new InvalidFlagException();
					}
			}
			catch (Exception e)
			{
				Program.Debug.Log(e.Message);

				return new ApplicationArgument();	//	returns a null instance
			}

			argument = new ApplicationArgument(FlagStack.ToArray(), SingleFlags.ToArray(), RawValueStack.ToArray());

			return argument;
		}

		public string Run()	//	Starts the app function
		{
			this.ApplicationFunction(this.GetApplicationArgument());

			return null;
		}

		public Application()
		{	
			this.Configuration = new AppConfiguration();
			this.ApplicationFunction = new aAppFunction((ApplicationArgument x) => null);
		}

		public Application(AppConfiguration configuration, aAppFunction appFunction)
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
		public static bool SerializeApplications()
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
		/// <returns> Deserialization success state represented by a bool. </returns>
		public static bool DeserializeApplicaions()
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