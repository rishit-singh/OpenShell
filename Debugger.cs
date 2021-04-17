using System; 

namespace CSharp_DebugTools
{
	public class UITools
	{
		///<summary>
		/// Creates a string containing multiple occurances of the provided char, hence replicating it. 
		///</summary>
		///<param name = "chr"> Character to be replicated. </param>
		///<param name = "times"> Number of occurances/replicants of the char </param>
		public static string ReplicateChar(char chr, int times)
		{
			string MultipleChars = null;
					
			for (int x = 0; x < times; x++)
				MultipleChars += chr;

			return MultipleChars;
		}
	}
	
	public class GeneralTools
	{
		public static void PrintArray<T>(T[] array)
		{
			for (int x = 0; x < array.Length; x++)
				Console.WriteLine(array[x]);
		}
	}
	
	public class Debugger
	{
		public string Title; 	//	Title of the current instance
	 
		private int LogCount;	// Stores the number of times a message is logged using current instance

		/// <summary>
		/// Prints the provided debug message of any type T to the console.
		/// </summary>
		/// <param name="val">Value to be printed</param>
		/// <typeparam name="T">Type of the value.</typeparam>
		public void Log<T>(T val)
		{
			Console.WriteLine($"Debug {this.LogCount++}: {val}");
		}

		/// <summary>
		/// Prints the debug message T[] to the console.
		/// </summary>
		/// <param name="array">Array of debug values.</param>
		/// <typeparam name="T">Type of the array</typeparam>
		public void Log<T>(T[] array)
		{
			Console.WriteLine($"Debug {this.LogCount++}:");

			GeneralTools.PrintArray<T>(array);
		}

		///<summary>
		/// Prints the provided debug message to the console.
		///</summary>
		///<param name = "Message"> Message tot be printed </param>
		public void Log(string Message) 
		{
			Console.WriteLine($"\nDebug {this.LogCount++}: {Message}");
		}

		/// <summary>
		/// Prints a string[] as log messages.
		/// </summary>
		/// <param name="array"> Array to be printed. </param>

		public void Log(string[] array)
		{
			Console.Write($"Debug {this.LogCount++}: ");
			GeneralTools.PrintArray<string>(array);	
		}
	
		///<summary>
		///	Sets up the console UI. 
		///</summary>
		public void StartConsole()	// Sets up the console UI
		{
			Console.WriteLine($"{this.Title}\n{UITools.ReplicateChar('-', this.Title.Length)}");		
		}

		public Debugger()
		{
			this.LogCount = 0;	//	Initializes the LogCount
			this.Title = "New Debugging Console";
		}

		public Debugger(string title)			
		{
			this.LogCount = 0; //  Initializes the LogCount
			this.Title = title;
		}
	}
}
