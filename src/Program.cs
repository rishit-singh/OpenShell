using System;
using CSharp_DebugTools;


namespace OpenShell
{
	public class Program
	{
		public static Debugger Debug = new Debugger();

		static void Main(string[] args)
		{	

			Application application = new Application(new AppConfiguration("Foo", new Command("foo", new Command.Flag[] { new Command.Flag("flag"), new Command.Flag("flag1") } )),
													(ApplicationArgument applicationArgument) => {
															// Program.PrintApplicationArgument(applicationArgument);
															return null;
														}
													), 
													application1 = new Application(new AppConfiguration("Foo1", new Command("foo1", new Command.Flag[] { new Command.Flag("flag"), new Command.Flag("flag1") } )),
													(ApplicationArgument applicationArgument) => {
															// Program.PrintApplicationArgument(applicationArgument);
															return null;
														}
													);


			Shell shell = new Shell(new Application[] { application, application1 });

			shell.PromptString = "osh";

			shell.AddApplication(new Application(
				new AppConfiguration("clear", new Command("clear")),
						new aAppFunction((ApplicationArgument args) => {
							Console.Clear();
						
							shell.RunShellLoop();
					
							return null;
						})
				));

			// Program.Debug.Log($"FlagString: {application.GetFlag("flag").FlagString}");			

			shell.Start();

			return; 
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
