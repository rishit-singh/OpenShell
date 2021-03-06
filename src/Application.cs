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

	/// <summary>
	/// Stores the app configuration.
	/// </summary>
	public struct AppConfiguration	
	{
		public string AppName;	//	Name of the app.
		
		public Command AppCommand;	//	Command instance of the app.

		public AppConfiguration(string appName, Command appCommand)
		{
			this.AppName = appName;
			this.AppCommand = appCommand;
		}
	}

	/// <summary>
	/// Argument of an Application function.
	/// </summary>
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
	/// <summary>
	///	Stores the Application function and its configuration. 
	/// </summary>
	[Serializable]
	public class Application : IApplication
	{
		public AppConfiguration Configuration;	//	Stores the app config for current instance

		public aAppFunction ApplicationFunction;	//	delegate to the main app function

		/// <summary>
		/// Returns the flag containing the provided flag string.
		/// </summary>
		/// <returns> Returns the flag with the provided flagstring. </returns>
		public Command.Flag GetFlag(string flagString)
		{
			string newFlagString = StringTools.OmitCharOccurances('-', (flagString[1] == '-') ? 2 : 1, flagString);
			
			try
			{
				return ((Command.Flag)this.Configuration.AppCommand.FlagHash[newFlagString]);
			}
			catch (NullReferenceException e)
			{
				new UnkownFlagException(flagString).Log();
			}

			return new Command.Flag();
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

			Command.Flag FlagTemp;

			for (int x = 0; x < this.Configuration.AppCommand.Parameters.Length; x++)
			{
				try
				{
					if (this.Configuration.AppCommand.Parameters[x][0] == '-')
					{
						FlagTemp = this.GetFlag(this.Configuration.AppCommand.Parameters[x]);
						
						if (FlagTemp.IsNull())	//	Flag doesnt exist  
							throw new UnkownFlagException(this.Configuration.AppCommand.Parameters[x]);	
						
						else if (FlagTemp.FlagString != null && FlagTemp.Value == null)
							SingleFlags.Push(FlagTemp);

						else
							throw new InvalidFlagException();
					}
					else
					{
						RawValueStack.Push(this.Configuration.AppCommand.Parameters[x]);
					}
				}
				catch (Error e)
				{
					e.Log();

					return new ApplicationArgument();	//	returns a null instance
				}
			}
			
			argument = new ApplicationArgument(ArrayTools.InvertArray<Command.Flag>(FlagStack.ToArray()), ArrayTools.InvertArray<Command.Flag>(SingleFlags.ToArray()),  ArrayTools.InvertArray<string>(RawValueStack.ToArray()));

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

			if (parameters.Length == 0)
				return applicationArgument;

			Stack<string> RawValueStack = new Stack<string>();

			try
			{
				for (int x = 0; x < parameters.Length; x++)
					if (parameters[x][0] == '-')		//	Checks for the flag prefixes.
						if (!(FlagTemp = this.GetFlag(parameters[x])).IsNull())	//	Flag null check.
							if (FlagTemp.IsSingle())
								SingleFlagStack.Push(FlagTemp);
							
							else
							{
								FlagStack.Push(FlagTemp);
							
								if (x < (parameters.Length - 1))							
									if (FlagTemp.Value == parameters[x + 1]) 
									{	
										x++;

										continue;
									}
							}
								
						else
							throw new UnkownFlagException(parameters[x]);

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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
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
						FlagStack.Push(flags[x]);
					
					else if (flags[x].FlagString != null && flags[x].Value == null)
						SingleFlags.Push(flags[x]);
					
					else if (flags[x].FlagString == null && flags[x].Value != null)
						RawValueStack.Push(flags[x].Value);
				
					else
						throw new InvalidFlagException();
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
			return this.ApplicationFunction(this.GetApplicationArgument());
		}

		public string Run(Command command)	//	Starts the app function
		{		
			if (command.CommandString != this.Configuration.AppCommand.CommandString)
				return null;

			ArrayTools.PrintArray<string>(command.Parameters);

			return this.ApplicationFunction(this.GetApplicationArgument(command.Parameters));
		}

		public Application()
		{	
			this.Configuration = new AppConfiguration();
			this.ApplicationFunction = new aAppFunction((ApplicationArgument x) => null);	
		}

		public Application(aAppFunction appFunction)
		{	
			this.ApplicationFunction = appFunction;
			this.Configuration = new AppConfiguration();
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