using QwirkleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QwirkleConsole {
	class Program {
		static Board board;

		static void Main(string[] args) {
			board = new Board(5, 5);

			Console.OutputEncoding = Encoding.UTF8;

			Console.WriteLine("Welcome to the QuirkleSharp Console application.");
			Console.WriteLine("Enter a command or enter 'help' for a list of commands.");

			string cmdLine = string.Empty;
			bool exitCmd = false;

			while(!exitCmd) {
				Console.Write("> ");
				cmdLine = Console.ReadLine();

				string[] parts = cmdLine.Split(' ');

				switch(parts[0].ToLower()) {
					case "resize":
						ResizeBoard(parts.Skip(1).ToArray());
						break;
					case "board":
						PrintBoard();
						break;
					case "help":
						PrintHelp();
						break;
					case "quit":
						goto case "exit";
					case "exit":
						exitCmd = true;
						break;
					case "":
						continue;
					default:
						Console.WriteLine("Invalid command. Enter 'help' for list of commands.");
						break;
				}
			}
		}

		static void WriteTile(Tile tile) {
			if(tile == null) {
				Console.Write(" ");
				return;
			}

			string character = string.Empty;

			switch (tile.Color) {
				case Color.Red:
					Console.BackgroundColor = ConsoleColor.DarkRed;
					break;
				case Color.Orange:
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case Color.Yellow:
					Console.BackgroundColor = ConsoleColor.DarkYellow;
					break;
				case Color.Green:
					Console.BackgroundColor = ConsoleColor.DarkGreen;
					break;
				case Color.Blue:
					Console.BackgroundColor = ConsoleColor.Blue;
					break;
				case Color.Purple:
					Console.BackgroundColor = ConsoleColor.DarkMagenta;
					break;
			}

			switch (tile.Shape) {
				case Shape.Circle:
					character = "●";
					break;
				case Shape.Square:
					character = "▪";
					break;
				case Shape.Diamond:
					character = "♦";
					break;
				case Shape.Starburst:
					character = "*";
					break;
				case Shape.Clover:
					character = "♣";
					break;
				case Shape.X:
					character = "X";
					break;
			}

			Console.Write(character);

			Console.ResetColor();
		}

		static void ResizeBoard(string[] args) {
			bool valid = true;

			if (args.Length != 2) {
				valid = false;
			}
			else {
				int rows = 0;
				int columns = 0;

				if (!int.TryParse(args[0], out rows)) {
					valid = false;
				}
				else if (!int.TryParse(args[1], out columns)) {
					valid = false;
				}
				else if ((rows <= 0) || (columns <= 0)) {
					valid = false;
				}
				else {
					board = new Board(rows, columns);
				}
			}

			if (!valid) {
				Console.WriteLine("Invalid arguments. Usage: resize <rowCount> <columnCount>");
			}
		}

		static void PrintBoard() {
			Console.WriteLine("Board Size: {0} rows and {1} columns.", board.RowCount, board.ColumnCount);

			Console.WriteLine("+".PadRight(board.RowCount + 1, '-') + "+");

			for (int i = 0; i < board.RowCount; i++) {
				Console.Write("|");

				for (int j = 0; j < board.ColumnCount; j++) {
					WriteTile(board.Peek(i, j));
				}
				
				Console.WriteLine("|");
			}

			Console.WriteLine("+".PadRight(board.RowCount + 1, '-') + "+");
		}

		static void PrintHelp() {
			// TODO: expand
			Console.WriteLine("Help and stuff");
		}

		static bool Warn(string message) {
			Console.WriteLine("Warning: {0} Continue? (y/n)", message);

			string line = string.Empty;

			while(string.IsNullOrEmpty(line)) {
				line = Console.ReadLine();
				line = line.ToLower();

				if(line == "y" || line == "yes") {
					return true;
				}
			}

			return false;
		}
	}
}
