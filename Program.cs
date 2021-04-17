using System;
using CSharp_DebugTools;


namespace OpenShell
{
	public class Program
	{
		public static Debugger Debug = new Debugger();
	
		static void Main(string[] args)
		{	
			Shell shell = new Shell(new Application[] { new Application(new AppConfiguration("TestApp", new Command("command")), new sAppFunction((string[] args) => { return "app called"; })), new Application(new AppConfiguration("TestApp", new Command("command1")), new sAppFunction((string[] args) => { return "app1 called"; })) } );
			
			shell.PromptString = "osh";

			shell.AddApplication(new Application(
				new AppConfiguration("clear", new Command("clear")),
				new sAppFunction((string[] args) => {
					Console.Clear();
					shell.RunShellLoop();
					
					return null;
				})
			));

			shell.Start(); 
		}   
	}
}
