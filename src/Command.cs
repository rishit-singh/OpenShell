using System;
using  System.Collections;
using  System.Collections.Generic;
using OpenShell.Tools; 
using OpenShell.Errors;

namespace OpenShell
{

	public struct Command : IComparable<Command>
	{

		/// <summary>
		/// Stores the Flag information.
		/// </summary>

		public struct Flag : IComparable<Flag>
		{
			public string FlagString,
			Value;
			
			private Stack<string> DefaultValueStack;	//	Stores temporary values to be asssigned added to the DefaulValues array later.

			public string[] DefaultValues;	//	Default values of the flag.

			/// <summary>
			/// IComparable<Command> interface implementation.
			/// </summary>
			/// <param name="command"></param>
			/// <returns> Bool represnting if the condition was met. </returns>
			public bool IsEqual(Flag command)
			{
				return (this.FlagString == command.FlagString && this.Value == command.Value);
			}

			/// <summary>
			/// Checks if the current instance follows the null criteria.
			/// </summary>
			/// <returns> Bool reperesnting whether the current instance is null. </returns>
			public bool IsNull()
			{
				return (this.FlagString == null &&
						this.Value == null);
			}

			/// <summary>
			/// Checks if the current instance is a single flag.
			/// </summary>
			/// <returns> Bool representing whether the current instance is a single flag. </returns>
			
			public bool IsSingle()
			{
				return (this.FlagString != null && this.Value == null);
			}
		
			public Flag(string flagString)
			{
				this.FlagString = flagString;
				this.Value = null;

				this.DefaultValueStack = null;
				this.DefaultValues = null;
			}

			public Flag(string flagString, bool raw)
			{
				this.DefaultValueStack = new Stack<string>();
				
				this.FlagString = null;
				this.Value = null;

				if (raw)
					this.Value = flagString;	
				else
					this.FlagString = flagString;

				this.DefaultValueStack.Push(this.Value);

				this.DefaultValues = this.DefaultValueStack.ToArray();
			}

			public Flag(string flagString, string value)
			{
				this.DefaultValueStack = new Stack<string>();
				
				this.FlagString = flagString;
				this.Value = value;	

				this.DefaultValueStack.Push(this.Value);

				this.DefaultValues = this.DefaultValueStack.ToArray();
			}
	
			public Flag(string flagString, string value, string[] defaultValues) 
			{
				this.DefaultValueStack = new Stack<string>();
				
				this.FlagString = flagString;
				this.Value = value;	

				this.DefaultValueStack.Push(this.Value);

				this.DefaultValues = this.DefaultValueStack.ToArray();
			}
		}

		public bool IsEqual(Command command)
		{
			return (this.CommandString == command.CommandString && Tools.ArrayTools.ArrayCmp<string>(this.Aliases, command.Aliases));
		}	

		public string CommandString;	//	Command identifier string. 

		public string[] Aliases;	//	Aliases of the current command instances.

		public string[] Parameters;	//	Parameters of the currentr command instances

		public Hashtable FlagHash;	//	Stores all the flags hashed to their flagstring values.

		public Command.Flag[] Flags;	//	Flags of the current command instance
		/// <summary>
		/// Hashes all the flag instances stored in the Flags array to their FlagString.
		/// </summary>
		private void HashFlags()
		{
			this.FlagHash = new Hashtable();

			for (int x = 0; x < this.Flags.Length; x++)
				this.FlagHash.Add(this.Flags[x].FlagString, this.Flags[x]);		
		}

		public Command(string command)
		{
			this.Flags = null;
			this.FlagHash = new Hashtable();

			if (command == null || command == "")
			{	
				this.CommandString = null;
				this.Flags = null;
				this.Aliases = null;
				this.Parameters = null;
			}
			else
			{
				string[] SplittedString = StringTools.Split(command, ' ');

				this.CommandString = SplittedString[0]; 

				this.Parameters = ArrayTools.OmitElementAtIndex<string>(SplittedString, 0);
				this.Aliases = null;

				this.Flags  = new Command.Flag[] {};

				this.HashFlags();
			}
		}
	
		public Command(string command, Flag[] defaultFlags)
		{
			this.Flags = null;
			this.FlagHash = new Hashtable();

			if (command == null || command == "")
			{
				this.CommandString = null;
				this.Aliases = null;
				this.Parameters = null;
			}
			else
			{
				string[] SplittedString = StringTools.Split(command, ' ');

				this.CommandString = SplittedString[0]; 

				this.Parameters = ArrayTools.OmitElementAtIndex<string>(SplittedString, 0);
				this.Aliases = null;
			}
			
			this.Flags = defaultFlags;

			this.HashFlags();
		}
	}

	public class CommandTools
	{
	}

	/// <summary>
	///	Exception thrown when the provided command is invalid/not in the available commands 
	/// </summary>
	public class CommandNotFoundException : Error
	{
		string Message;

		public CommandNotFoundException()
		{
			this.Message = "Command not found.";
		}

		public CommandNotFoundException(string command)
		{
			this.Message = $"Command \"{command}\" not found.";
		}	

		public CommandNotFoundException(Command command)
		{
			this.Message = $"Command \"{command.CommandString}\" not found.";
		}	
	}

	/// <summary>
	/// Exception thrown when the application delegates are not hashed and u
	/// </summary>
	public class UnhashedApplicationsException : Error
	{
		string Message;

		public UnhashedApplicationsException()
		{
			this.Message = "The available applications were not hashed.";
		}
	}
	

	/// <summary>
	/// Exception thrown when and invalid flag instance is detecteds
	/// </summary>
	public class InvalidFlagException : Error
	{
		// string Message;

		public InvalidFlagException()
		{
			this.Message = "Provided Flag instance is invalid.";
		}
	}


	/// <summary>
	/// Exception thrown when an unknown Flag is detected.
	/// </summary>
	public class UnkownFlagException : Error
	{	
		public UnkownFlagException(string flagString)
		{
			this.Message = $"{flagString} does not exist in the default flags.";
		}
	}
}
