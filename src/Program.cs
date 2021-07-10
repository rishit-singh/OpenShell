using System;
using CSharp_DebugTools;
using System.Collections;
using System.Collections.Generic;

namespace OpenShell
{
	public class Program
	{
		public static Debugger Debug = new Debugger();

		static void Main(string[] args)
		{
			Application application = new Application(new AppConfiguration("Foo", new Command("foo", new Command.Flag[] { new Command.Flag("flag") { Value = "value" }, new Command.Flag("flag1") } )),
													(ApplicationArgument applicationArgument) => {
															return null;
														}
													), 
													application1 = new Application(new AppConfiguration("Foo1", new Command("foo1", new Command.Flag[] { new Command.Flag("flag"), new Command.Flag("flag1") } )),
													(ApplicationArgument applicationArgument) => {
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

			shell.Start();

			return; 
		}  
	}
}
