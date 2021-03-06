using System;
using System.Collections;
using System.Collections.Generic;
using OpenShell.UI;
using OpenShell.Errors;
using CSharp_DebugTools;

namespace OpenShell
{
	public class Shell
	{
		public ShellUI shellUI;	//	UI instance for current class 

		public Stack<Application> ApplicationStack;	//	Stores all the application instances of a shell

		public Hashtable ApplicationHash;	//	Contains the delegates referring to the AppFunctions
		
		public bool AppsHashed;	// Represents the status of Applicatio	nHash
	
		private bool IsRunning = false;	//	Represents the status of the shell		public string PromptString;	//	String to be used as a prompting message for default shell prompts.
	
		public string PromptString;	//	String to be printed before the prompt character.

		/// <summary>
		/// Adds all the provided application instances to the ApplicationStack
		/// </summary>
		/// <param name="applications"></param>
		/// <returns> Bool reperesenting if the ApplicationStack was built. </returns>
		private bool BuildApplicationStack(Application[] applications)
		{
			if (applications.Length <= 0)
				return false;

			for (int x = 0; x < applications.Length; x++)
				this.ApplicationStack.Push(applications[x]);

			return true;
		}


		/// <summary>
		/// Hashes all the application provided in the applications array.
		/// </summary>
		/// <param name="applications"> Array of Application instances. </param>
		/// <returns> Hashtable containing Application instances hashed to their respective commands. </returns>
		public Hashtable HashApplications(Application[] applications)	//	hashes the applications with their names on shell start
		{
			Hashtable applicationHash = new Hashtable(); 
			
			for (int x = 0; x < applications.Length; x++)
				applicationHash.Add(applications[x].Configuration.AppCommand.CommandString.ToLower(), applications[x]);

			this.AppsHashed = true;

			return applicationHash;
		}

		/// <summary>
		/// Returns the sAppFunction delegate hashed to the provied command.
		/// </summary>
		/// <param name="command"> Command instance. </param>
		/// <returns> Hashed sAppFunction delegate. </returns>
		public Application GetApplication(Command command)		//	Returns the application function delegate	
		{
			Application application = null;

			try 
			{
				if ((application = this.GetHashedApplication(command.CommandString)) == null)
					throw new UnhashedApplicationsException();
			
				Console.WriteLine($"Function Call {application.Run()}");

				return application;
			} 
			catch (UnhashedApplicationsException e)
			{
				e.Log();
			}
			
			return application;
		}

		/// <summary>
		/// Returns the app function hashed to the provided command.
		/// </summary>
		/// <param name="command"></param>
		/// <returns> The hashed application function. </returns>
		private Application GetHashedApplication(string command)
		{
			Application application = null;

			if (command == null)
				return new Application(new aAppFunction((ApplicationArgument argument) => null)); 

			try
			{
				if (this.AppsHashed)
					try
					{
						application = (Application)this.ApplicationHash[command];
					}	
					catch (KeyNotFoundException e)
					{
						Error.Log(e);

						return application;	
					}
				else
					throw new UnhashedApplicationsException();
			}
			catch (UnhashedApplicationsException e)
			{
				e.Log();
			}

			return application;
		}

		/// <summary>
		/// Pushes a new Application instance to the Application stack and updates the Application Hash
		/// </summary>
		/// <param name="application"></param>
		/// <returns></returns>
		public Application AddApplication(Application application)
		{
			this.ApplicationStack.Push(application);
		
			this.ApplicationHash.Add(application.Configuration.AppCommand.CommandString, application);

			return application; 
		}

		/// <summary>
		/// Starts the shell.
		/// </summary>
		/// <returns> Bool representing if the shell was started successfully. </returns>
		public bool Start()	//	Starts the shell.
		{
			this.IsRunning = true;			
		
			ShellUI.Setup();

			return RunShellLoop();
		}

		/// <summary>
		/// Starts the prompt and execution loop
		/// </summary>
		/// <returns> bool representing the execution success/failure of the function. </returns>
		public bool RunShellLoop()	
		{
			while (this.IsRunning)
			{	
				this.Print(this.ExecuteCommand(new Command(this.Prompt(this.PromptString))));
			}
	
			return true;
		}

		/// <summary>
		/// Outputs a newline to the shell.
		/// </summary>
		public void Print()
		{
			Console.WriteLine();
		}

		/// <summary>
		/// Outputs the provided string to the shell.
		/// </summary>
		/// <param name="str"> String to output. </param>
		public void Print(string str)
		{
			Console.WriteLine(str);
		}

		/// <summary>
		/// Prompts the user for input.
		/// </summary>
		/// <returns> User input. </returns>
		public string Prompt()
		{
			Console.Write("> ");

			return Console.ReadLine();
		}

		/// <summary>
		/// Prompts the user for an input with the provided message.
		/// </summary>
		/// <param name="message"> Message to prompt with. </param>
		/// <returns> User input. </returns>
		public string Prompt(string message)	
		{
			Console.Write($"{message}> ");

			return Console.ReadLine();
		}
		
		/// <summary>
		/// Executes the provided command.
		/// </summary>
		/// <param name="command"> Command to be executed. </param>
		/// <returns> Delegates refering to the reDEspective command, null when invalid. </returns>
		public string ExecuteCommand(Command command)
		{
			try
			{
				if (this.AppsHashed)
					switch (command.CommandString) 
					{
						case null:
							return null;

							break;

						case "exit":
							this.Terminate();
								
							break;

						default:
							try
							{
								Application application = null;

								if ((application = this.GetApplication(command)) != null)
									return application.Run(command);
		
								throw new CommandNotFoundException(command);
							}
							catch (CommandNotFoundException e)
							{
								Console.WriteLine($"\nError: \"{command.CommandString}\" is not a valid command.");
							
								return null;
							}
							
							break;
					}
			}
			catch (UnhashedApplicationsException e)
			{
				e.Log();
			}
			
			return null;
		}

		/// <summary>
		/// Ends the shell loop.
		/// </summary>
		public void Terminate()
		{
			this.IsRunning = false;
		}

		public Shell()
		{
			this.PromptString = null;
			this.ApplicationHash = null;			
		}

		public Shell(string prompt) 
		{
			this.PromptString = prompt ;
				this.ApplicationHash = null;		
		}

		public Shell(Application[] apps)
		{
			this.ApplicationHash = this.HashApplications(apps); 

			this.ApplicationStack = new Stack<Application>();

			this.BuildApplicationStack(apps);
		}
	}
}

