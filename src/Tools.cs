using System; 
using System.Collections;
using System.Collections.Generic;
using OpenShell.Errors; 

namespace OpenShell.Tools
{
	public class GeneralTools 
	{
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp = a;

			a = b; 
			b = temp;
		}
	}

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

		/// <summary>
		/// Checks if the provided Command.Flag instance exits in the provided Command.Flag[] array.
		/// </summary>
		/// <param name="flag"> Command.Flag instance. </param>
		/// <param name="flags"> Command.Flag[] array. </param>
		/// <returns> Bool representing if the condition is met. </returns>
		public static bool IsElement(Command.Flag flag, Command.Flag[] flags)
		{
			for (int x = 0; x < flags.Length; x++)
				if (flag.IsEqual(flags[x]))
					return true;
					
			return false;
		}

		public static Command.Flag[] BubbleSort(Command.Flag[] flagArray)
		{
			for (int x = 0; x < flagArray.Length - 1; x++)
				for (int y = x + 1; y < flagArray.Length; y++)
					if (flagArray[x].FlagString.CompareTo(flagArray[x + 1].FlagString) > 0)
						GeneralTools.Swap<Command.Flag>(ref flagArray[x], ref flagArray[x + 1]);

			return flagArray;
		}
		
		public static int BinarySearch(string flagString, Command.Flag[] flags, int start, int end)
		{
			if (start < end)
			{
				int mid = (start + end) / 2;

				if (flagString.CompareTo(flags[mid].FlagString) < 0)
					return ArrayTools.BinarySearch(flagString, flags, start, mid);

				if (flagString.CompareTo(flags[mid].FlagString) > 0)
					return ArrayTools.BinarySearch(flagString, flags, mid, end);

				if (flagString.CompareTo(flags[mid].FlagString) == 0)
					return mid;
			}

			return -1;
		}

		public static int BinarySearch(string flagString, Command.Flag[] flags)
		{
			return ArrayTools.BinarySearch(flagString, flags, 0, flags.Length);		
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

			if (start > str.Length)
			{
				Error.Log("Provided index is out of the range of the provided string");

				return Occurance;
			}

			for (int x = start; str[x] == chr; x++)
				Occurance++;	

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

		public static string GetSubString(string str, int start, int end)
		{
			string subString = null;

			if ((end > str.Length || end < start) || (start < 0 || start > end))
			{
				Error.Log("Invalid start or end index provided.");	

				return null;
			}

			for (int x = start; x < end; x++)
				subString += str[x];

			return subString;
		}
	}
}