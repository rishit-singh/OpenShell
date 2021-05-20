using System; 
using System.Collections;
using System.Collections.Generic;
using OpenShell.Errors; 

namespace OpenShell.Tools
{
	public class ArrayTools
	{
		public static void PrintArray<T>(T[] array)
		{
			for (int x = 0; x < array.Length; x++)
				Console.WriteLine(array[x]);
		}		

		public static T[] InvertArray<T>(T[] array)
		{
			T[] InvertedArray = new T[array.Length];

			for (int x = 0; x < array.Length; x++)
				InvertedArray[x] = array[array.Length - (1 + x)];

			return InvertedArray;
		}

		public static T[] OmitElementAtIndex<T>(T[] array, int index)
		{
			Stack<T> OmitStack = new Stack<T>();

			for (int x = array.Length - 1; x >= 0; x--)
				if (x == index)
					continue;
				else
					OmitStack.Push(array[x]);

			return OmitStack.ToArray();
		}

		public static T[] IEnumerableToArray<T>(IEnumerable<T> ienum)
		{
			Stack<T> IEnumStack = new Stack<T>();

			foreach (T temp in ienum)
				IEnumStack.Push(temp);

			return IEnumStack.ToArray();
		}

		public static bool ArrayCmp<T>(T[] array, T[] array1)
		{
			if (array1.Length < array.Length)
				return false;

			for (int x = 0; x < array.Length; x++)
				if (!array[x].Equals(array1[x]))
					return false;

			return true;
		}	

		public static bool IsElement(Command.Flag flag, Command.Flag[] flags)
		{
			for (int x = 0; x < flags.Length; x++)
				if (flag.IsEqual(flags[x]))
					return true;
					
			return false;
		}
	}

	public class StringTools
	{	
		public static string[] Split(string str, char splitChar)    //  splits the provided string to a string[] on the basis of the splitChar
		{
			Stack<string> SplitStack = new Stack<string>(); 

			string temp = null; 

			for (int x = 0; x < str.Length; x++)
				if (str[x] == splitChar)
				{
					SplitStack.Push(temp);

					temp = null; 

					continue;
				} 
				else if (x == (str.Length - 1))
				{
					temp += str[x];

					SplitStack.Push(temp);

					temp = null;

					continue;	
				}
				else
					temp += str[x]; 
					
			return ArrayTools.InvertArray<string>(SplitStack.ToArray());
		}

		public static int GetOccurances(char chr, string str)	//	Returns the occureances of a character in the provided string
		{
			int Occurances = 0;

			for (int x = 0; x < str.Length; x++)
				if (str[x] == chr)
				Occurances++; 

			return  Occurances;
		}
				
		public static int GetOccurances(char chr, string str, bool reversed)	//	Returns the occureances of a character in the provided string. Starts from the the end of the string if the reversed bool is true
		{
			int Occurances = 0;

			if (reversed)
				for (int x = (str.Length - 1); x >= 0; x--)
					if (str[x] == chr)
						Occurances++; 
			else
				return StringTools.GetOccurances(chr, str);
		
			return Occurances;
		}

		public static int GetContinousOccurance(char chr, string str, int start)
		{
			int Occurance = 0;
			
			try
			{
				for (int x = start; str[x] == chr; x++)
					Occurance++;	
			}
			catch (IndexOutOfRangeException)
			{
				Error.Log("Provided index is out of the range of the provided string");
			}
	
			return Occurance;
		}
		
		public static string OmitCharOccurances(char chr, int occurances, string str)		//	 Removes the provided number of occurance of a char in the provided string
		{
			string AlteredString = null;

			int Occurance = 0;

			for (int x = 0; x < str.Length; x++)
				if (str[x] == chr)
				{
					Occurance++; 

					if (Occurance <= occurances)
						continue;

					AlteredString += str[x]; 
				}
				else 
					AlteredString += str[x]; 


			return AlteredString;	
		}
	}
}