﻿using System;
using CSharp_DebugTools;


namespace OpenShell
{
	public class Program
	{
		public static Debugger Debug = new Debugger();

		static void Main(string[] args)
		{	
			
			Application application = new Application(
				new AppConfiguration("Foo", new Command("foo", new Command.Flag[] { new Command.Flag("flag"), new Command.Flag("flag1") } )),
													(ApplicationArgument applicationArgument) => {
															Console.WriteLine($"FlagStrings: {applicationArgument.Flags[0].FlagString}");

															return null;
														}
													);


			// application.Run();

			// Shell shell = new Shell(new Application[] { new Application(new AppConfiguration("TestApp", new Command("command")), new sAppFunction((string[] args) => { return "app called"; })), new Application(new AppConfiguration("TestApp", new Command("command1")), new sAppFunction((string[] args) => { return "app1 called"; })) } );
			
			Shell shell = new Shell(new Application[] { application });

			shell.PromptString = "osh";

			// shell.AddApplication(new Application(
			// 	new AppConfiguration("clear", new Command("clear")),
			// 	new sAppFunction((string[] args) => {
			// 		Console.Clear();
			// 		shell.RunShellLoop();
					
			// 		return null;
			// 	})
			// ));0

			shell.Start(); 
		}  

		public static void PrintApplicationArgument(ApplicationArgument applicationArgument)
		{
			Console.WriteLine("Flags:");
			for (int x = 0; x < applicationArgument.Flags.Length; x++)
				Console.WriteLine($"{applicationArgument.Flags[x].FlagString}: {applicationArgument.Flags[x].Value}");

			Console.WriteLine('\n');

			Console.WriteLine("Single Flags:");
			for (int x = 0; x < applicationArgument.SingleFlags.Length; x++)
				Console.WriteLine($"{applicationArgument.SingleFlags[x].FlagString}");

			Console.WriteLine('\n');

			Console.WriteLine("Raw Values:");
			for (int x = 0; x < applicationArgument.RawValues.Length; x++)
				Console.WriteLine($"{applicationArgument.RawValues[x]}");
		}
	}
}
