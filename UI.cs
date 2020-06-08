using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace SyntaxManager
{
	class UI
	{
		public enum DecorChar
		{ 
			Hyphen,
			Space,
			Tab,
			BoxDrawingChar0 //later
		}

		public enum Element
		{ 
			Header,
			Body
		}

		public enum UIText
		{ 
			Heading
		}

		public static char[] DecorChars = new char[] {
			'-',
			' ',
			'\t'
		};

		public static string[] UITexts = new string[] { 
			"CodeBook Syntax Manager"
		};

		public static UI.ElementConfig[] Elements = new UI.ElementConfig[] {
			new UI.ElementConfig(UI.UITexts[(int)UI.UIText.Heading], UI.DecorChars[(int)UI.DecorChar.Hyphen], ElementConfig.H_Alignment.Center, ElementConfig.V_Alignment.Middle) //UI.UITexts[x] exceptions to be handled, later
		};

		
		public struct ElementConfig
		{

			//Implemented due to misunderstanding. But might be required later.
			public struct Margin
			{
				public int X, Y, X_neg, Y_neg;

				public Margin(int x, int y, int x_neg, int y_neg)
				{
					this.X = x;
					this.Y = y;
					this.X_neg = x_neg;
					this.Y_neg = y_neg;
				}

				public Margin(int x, int y)
				{
					this.X = x;
					this.Y = y;
					this.X_neg = 0;
					this.Y_neg = 0;
				}

				public Margin(short x_neg, short y_neg) //short used just to prevent errors.
				{
					this.X_neg = x_neg;
					this.Y_neg = y_neg;
					this.X = 100 - x_neg;
					this.Y = 100 - y_neg;
				}
			}

			public static ElementConfig.Margin[,] Margins = new ElementConfig.Margin[,] {
				{   
					new ElementConfig.Margin(),
					new ElementConfig.Margin(50, 0),
					new ElementConfig.Margin(100, 0)
				},
				{ 
					new ElementConfig.Margin(),
					new ElementConfig.Margin(0, 0, 0, 50),
					new ElementConfig.Margin(0, 0, 0, 100) 
				}
			};

			public struct Nulls
			{
				public static ElementConfig.Margin MarginNull = new ElementConfig.Margin();
				public static ElementConfig.H_Alignment H_AlignmentNull = (ElementConfig.H_Alignment)(-1);
				public static ElementConfig.V_Alignment V_AlignmentNull = (ElementConfig.V_Alignment)(-1);
			}


			public enum H_Alignment
			{
				Left,
				Center,
				Right
			}

			public enum V_Alignment
			{
				Top,
				Middle,
				Bottom
			}

			public enum Alignments
			{
				Left,
				Center,
				Right,
				Top,
				Middle,
				Bottom
			}

			public static int DefaultDecorTimes = 50;

			public string Text;
			public char DecorChar;
			public int DecorTimes, DecorLines;
			public ElementConfig.H_Alignment H_Align;
			public ElementConfig.V_Alignment V_Align;

			public ElementConfig.Margin hAlignment, vAlignment;

			public ElementConfig(string text)
			{
				this.Text = text;
				this.DecorChar = new System.Char(); //null initialisation
				this.H_Align = (ElementConfig.H_Alignment)(-1);
				this.V_Align = (ElementConfig.V_Alignment)(-1);
				this.DecorTimes = 0;
				this.DecorLines = 0;

				this.hAlignment = new Margin();
				this.vAlignment = new Margin();
			}

			public ElementConfig(string text, char decorChar)
			{
				this.Text = text;
				this.DecorChar = decorChar;
				this.H_Align = (ElementConfig.H_Alignment)(-1);
				this.V_Align = (ElementConfig.V_Alignment)(-1);
				this.DecorTimes = ElementConfig.DefaultDecorTimes; ///default
				this.DecorLines = 0;

				this.hAlignment = new Margin();
				this.vAlignment = new Margin();
			}

			public ElementConfig(string text, UI.DecorChar decorChar)
			{
				this.Text = text;

				try
				{
					this.DecorChar = UI.DecorChars[(int)decorChar];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");

					this.DecorChar = new System.Char(); //null initialisation
				}

				this.H_Align = (ElementConfig.H_Alignment)(-1);
				this.V_Align = (ElementConfig.V_Alignment)(-1);
				this.DecorTimes = ElementConfig.DefaultDecorTimes; ///default
				this.DecorLines = 0;

				this.hAlignment = new Margin();
				this.vAlignment = new Margin();
			}

			public ElementConfig(string text, char decorChar, ElementConfig.H_Alignment h_align, ElementConfig.V_Alignment v_align)
			{
				this.Text = text;
				this.DecorChar = decorChar;
				this.H_Align = h_align;
				this.V_Align = v_align;
				this.DecorTimes = ElementConfig.DefaultDecorTimes;
				this.DecorLines = 0;
				try
				{
					this.hAlignment = ElementConfig.Margins[0, (int)H_Align];
					this.vAlignment = ElementConfig.Margins[1, (int)V_Align];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid Alignment Identifier.");

					this.hAlignment = new ElementConfig.Margin();
					this.vAlignment = new ElementConfig.Margin();
				}
				this.hAlignment = ElementConfig.Margins[0, (int)h_align];
				this.vAlignment = new Margin();
			}

			public ElementConfig(string text, UI.DecorChar decorChar, ElementConfig.H_Alignment h_align, ElementConfig.V_Alignment v_align)
			{
				this.Text = text;

				try
				{
					this.DecorChar = UI.DecorChars[(int)decorChar];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid ALignment Identifier.");

					this.DecorChar = new System.Char(); //null initialisation
				}

				this.H_Align = h_align;
				this.V_Align = v_align;
				this.DecorTimes = ElementConfig.DefaultDecorTimes;
				this.DecorLines = 0;

				try
				{
					this.hAlignment = ElementConfig.Margins[0, (int)H_Align];
					this.vAlignment = ElementConfig.Margins[1, (int)V_Align];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");

					this.hAlignment = new ElementConfig.Margin();
					this.vAlignment = new ElementConfig.Margin();
				}
			}

			public ElementConfig(string text, char decorChar, ElementConfig.H_Alignment h_align, ElementConfig.V_Alignment v_align, int decorTimes, int decorLines)
			{
				this.Text = text;
				this.DecorChar = decorChar;
				this.H_Align = h_align;
				this.V_Align = v_align;
				this.DecorTimes = decorTimes;
				this.DecorLines = decorLines;

				try
				{
					this.hAlignment = ElementConfig.Margins[0, (int)H_Align];
					this.vAlignment = ElementConfig.Margins[1, (int)V_Align];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");

					this.hAlignment = new ElementConfig.Margin();
					this.vAlignment = new ElementConfig.Margin();
				}
			}

			public ElementConfig(string text, UI.DecorChar decorChar, ElementConfig.H_Alignment h_align, ElementConfig.V_Alignment v_align, int decorTimes, int decorLines)
			{
				this.Text = text;

				this.H_Align = h_align;
				this.V_Align = v_align;

				try
				{
					this.DecorChar = UI.DecorChars[(int)decorChar];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");

					this.DecorChar = new System.Char(); //null initialisation
				}

				this.DecorTimes = decorTimes;
				this.DecorLines = decorLines;

				try
				{
					this.hAlignment = ElementConfig.Margins[0, (int)h_align];
					this.vAlignment = ElementConfig.Margins[1, (int)v_align];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
					
					this.hAlignment = new ElementConfig.Margin();
					this.vAlignment = new ElementConfig.Margin();
				}
			}

			public ElementConfig(string text, char decorChar, int decorTimes, int decorLines)
			{
				this.Text = text;
				this.DecorChar = decorChar;
				this.DecorTimes = decorTimes;
				this.DecorLines = decorLines;
				this.H_Align = ElementConfig.Nulls.H_AlignmentNull;
				this.V_Align = ElementConfig.Nulls.V_AlignmentNull;
				this.hAlignment = ElementConfig.Nulls.MarginNull;
				this.vAlignment = ElementConfig.Nulls.MarginNull;
			}

			public ElementConfig(string text, UI.DecorChar decorChar, int decorTimes, int decorLines)
			{
				this.Text = text;

				try
				{
					this.DecorChar = UI.DecorChars[(int)decorChar];
				}
				catch (IndexOutOfRangeException e)
				{
					Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");

					this.DecorChar = new System.Char(); //null initialisation
				}

				this.DecorTimes = decorTimes;
				this.DecorLines = decorLines;
				this.H_Align = ElementConfig.Nulls.H_AlignmentNull;
				this.V_Align = ElementConfig.Nulls.V_AlignmentNull;
				this.hAlignment = new ElementConfig.Margin();
				this.vAlignment = new ElementConfig.Margin();
			}
		}

		public static string MultiplyChar(char single, int times)
		{
			string str = null;

			for (int x = 0; x < times; x++)
				str += single;

			return str;
		}

		public static string MultiplyChar(UI.DecorChar single, int times)
		{
			string str = null;
			char chr = new System.Char();

			try
			{
				chr = UI.DecorChars[(int)single];
			}
			catch (IndexOutOfRangeException e)
			{
				Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
			}

			for (int x = 0; x < times; x++)
				str += chr;

			return str;
		}

		public static void IterPrint(UI.DecorChar decorChar, int times)
		{
			try
			{
				for (int x = 0; x < times; x++)
					Console.Write(UI.DecorChars[(int)decorChar]);
			}
			catch (IndexOutOfRangeException e)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
			}
		}

		public static void IterPrint(UI.DecorChar decorChar, int times, long lines) //long was used just to prevent errors
		{
			try
			{
				for (int x = 0; x < lines; x++)
					for (int y = 0; y < times; y++)
						Console.Write(UI.DecorChars[(int)decorChar]);
			}
			catch (IndexOutOfRangeException e)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
			}
		}

		public static void IterPrint(UI.DecorChar decorChar, int times, int spaces)
		{
			try
			{
				for (int x = 0; x < times; x++)
					Console.Write($"{UI.DecorChars[(int)decorChar]}{UI.MultiplyChar(UI.DecorChars[(int)decorChar], spaces)}");
			}
			catch (IndexOutOfRangeException e)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
			}
		}

		public static void IterPrint(UI.DecorChar decorChar, int times, int lines, int spaces)
		{
			try
			{
				for (int x = 0; x < lines; x++)
					for (int y = 0; y < times; y++)
						Console.Write($"{UI.DecorChars[(int)decorChar]}{UI.MultiplyChar(UI.DecorChars[(int)decorChar], spaces)}");
			}
			catch (IndexOutOfRangeException e)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.White;

				Console.WriteLine($"{e.Message} exception was thrown because of invalid DecorChar.");
			}
		}

		public static bool SetupElement(UI.Element element)
		{
			UI.ElementConfig currentElement = new UI.ElementConfig();

			string PrintStr = null;
			
			try
			{
				currentElement = UI.Elements[(int)element];
			}
			catch (IndexOutOfRangeException e)
			{
				Console.WriteLine($"{e.Message} exception was thrown because of invalid Element Identifier.");

				return false;
			}

			PrintStr = $"{UI.MultiplyChar('\n', currentElement.vAlignment.Y)}{UI.MultiplyChar(UI.DecorChar.Space, currentElement.hAlignment.X)}{UI.Elements[(int)element].Text}\n{UI.MultiplyChar(UI.DecorChar.Hyphen, 120)}\n"; //formatted string to be printed.

			Console.Write(PrintStr);
			
			return true;
		}

		public static void Setup()
		{
			UI.SetupElement(UI.Element.Header); //sets up the headerS
		}
	}
}
