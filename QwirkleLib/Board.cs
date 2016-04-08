using QwirkleLib.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace QwirkleLib {
	/// <summary>
	/// A board that holds and verfies multiple Qwirkle tiles.
	/// </summary>
	public class Board {
		public int RowCount {
			get;
			private set;
		}
		public int ColumnCount {
			get;
			private set;
		}

		private Tile[,] _board;

		/// <summary>
		/// Creates a new board with the specified dimensions.
		/// </summary>
		/// <param name="rowCount">The number of rows the board will have.</param>
		/// <param name="columnCount">The number of columns the board will have.</param>
		public Board(int rowCount, int columnCount) {
			this.RowCount = rowCount;
			this.ColumnCount = columnCount;

			this._board = new Tile[rowCount, columnCount];
		}

		/// <summary>
		/// Clears the board of all tiles.
		/// </summary>
		public void Clear() {
			this._board = new Tile[RowCount, ColumnCount];
		}

		/// <summary>
		/// Places the tile at the specified position,
		/// without checking for tile compatibility.
		/// </summary>
		/// <param name="tile">The tile that you want to place.</param>
		/// <param name="row">The row you want to place the tile in.</param>
		/// <param name="column">The column you want to place the tile in.</param>
		public void Place(Tile tile, int row, int column) {
			// Off-board placement
			if (OffBoardPosition(row, column)) {
				throw new InvalidPlacementException("Cannot place tile outside the board dimensions.");
			}

			// Piece already placed at location
			if (_board[row, column] != null) {
				throw new InvalidPlacementException("Cannot place tile at the same position of another tile.");
			}

			_board[row, column] = tile;
		}

		/// <summary>
		/// Returns the tile at the specified position without removing it.
		/// If the position is empty, it will return null.
		/// </summary>
		/// <param name="row">The row with the tile you want to peek.</param>
		/// <param name="column">The column with the tile you want to peek.</param>
		/// <returns>The tile at the specified position.</returns>
		public Tile Peek(int row, int column) {
			return _board[row, column];
		}

		/// <summary>
		/// Removes and returns the tile at the specified position.
		/// If the position is empty, it will return null.
		/// </summary>
		/// <param name="row">The row of the tile you want to remove.</param>
		/// <param name="column">The column of the tile you want to remove.</param>
		/// <returns>The tile at the specified position.</returns>
		public Tile Remove(int row, int column) {
			Tile tile = _board[row, column];

			_board[row, column] = null;

			return tile;
		}

		/// <summary>
		/// Checks if the tile can be placed at the specified position according to the rules.
		/// </summary>
		/// <param name="tile">The tile that you will want to place.</param>
		/// <param name="row">The row you will want to place the tile in.</param>
		/// <param name="column">The column you will want to place the tile in.</param>
		/// <returns>Whether the tile can be placed at the specified position.</returns>
		public bool IsValidPlacement(Tile tile, int row, int column) {
			// Off-board placement
			if (OffBoardPosition(row, column)) {
				return false;
			}

			// Piece already placed at location
			if (_board[row, column] != null) {
				return false;
			}

			// Check possibility for surrounding tiles
			bool potentialHorizontal = (!OffBoardPosition(row, column - 1) && tile.IsCompatibleWith(_board[row, column - 1])) ||
									   (!OffBoardPosition(row, column + 1) && tile.IsCompatibleWith(_board[row, column + 1]));

			bool potentialVertical = (!OffBoardPosition(row - 1, column) && tile.IsCompatibleWith(_board[row - 1, column])) ||
									 (!OffBoardPosition(row + 1, column) && tile.IsCompatibleWith(_board[row + 1, column]));

			// Temporarily add piece to board
			_board[row, column] = tile;

			if(potentialHorizontal) {
				var rowGroup = GetRowGroupTiles(row, column);

				// Check for duplicates
				if(rowGroup.Distinct().Count() != rowGroup.Count()) {
					// Remove tile
					_board[row, column] = null;

					return false;
				}
			}
			if(potentialVertical) {
				var columnGroup = GetColumnGroupTiles(row, column);

				// Check for duplicates
				if (columnGroup.Distinct().Count() != columnGroup.Count()) {
					// Remove tile
					_board[row, column] = null;

					return false;
				}
			}

			// Remove tile
			_board[row, column] = null;
			return true;
		}

		/// <summary>
		/// Checks if the specified position is outside the board dimensions.
		/// </summary>
		/// <param name="row">The row of the positioyou want to check.</param>
		/// <param name="column">The column of the position you want to check.</param>
		/// <returns>Whether the specified position is outside the board dimensions.</returns>
		public bool OffBoardPosition(int row, int column) {
			return ((row < 0 || row >= RowCount) || (column < 0 || column >= ColumnCount));
		}

		public IEnumerable<Tile> GetRowGroupTiles(int row, int column) {
			if (OffBoardPosition(row, column) || (_board[row, column] == null)) {
				throw new InvalidGroupException("Cannot get tiles in row group from this row/column.");
			}

			yield return _board[row, column];

			// Start checking toward the left
			int pos = column - 1;
			while (!OffBoardPosition(row, pos) && (_board[row, pos] != null)) {
				yield return _board[row, pos];

				pos--;
			}

			// Now check toward the right
			pos = column + 1;
			while (!OffBoardPosition(row, pos) && (_board[row, pos] != null)) {
				yield return _board[row, pos];

				pos++;
			}
		}

		public IEnumerable<Tile> GetColumnGroupTiles(int row, int column) {
			if (OffBoardPosition(row, column) || (_board[row, column] == null)) {
				throw new InvalidGroupException("Cannot get tiles in column group from this row/column.");
			}

			yield return _board[row, column];

			// Start checking toward the top
			int pos = row - 1;
			while (!OffBoardPosition(pos, column) && (_board[pos, column] != null)) {
				yield return _board[pos, column];

				pos--;
			}

			// Now check toward the bottom
			pos = row + 1;
			while (!OffBoardPosition(pos, column) && (_board[pos, column] != null)) {
				yield return _board[pos, column];

				pos++;
			}
		}
	}
}
