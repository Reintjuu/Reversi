using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Reversi
{
	public enum FieldState
	{
		Empty,
		Hint,
		PlayerOne,
		PlayerTwo
	}

	public partial class Screen : Form
	{
		private const int BOARD_WIDTH = 3;
		private const int BOARD_HEIGHT = 3;

		private readonly Point[] ALL_DELTAS =
		{
			new Point(1, 0),
			new Point(-1, 0),
			new Point(0, 1),
			new Point(0, -1),
			new Point(1, 1),
			new Point(-1, -1),
			new Point(-1, 1),
			new Point(1, -1)
		};

		private readonly PictureBox graphics;
		private readonly FieldState[,] board;
		private FieldState currentPlayer;

		public Screen()
		{
			graphics = new PictureBox
			{
				Size = new Size(ClientRectangle.Size.Width, ClientRectangle.Size.Height)
			};

			graphics.Paint += Render;
			Resize += UpdateBoardSize;
			graphics.MouseClick += OnClick;
			Controls.Add(graphics);

			board = new FieldState[BOARD_HEIGHT, BOARD_WIDTH];
			CreateInitialBoard();
			currentPlayer = FieldState.PlayerOne;

			InitializeComponent();
		}

		private void UpdateBoardSize(object sender, EventArgs e)
		{
			Control control = (Control) sender;
			graphics.Size = new Size(control.ClientRectangle.Size.Width, control.ClientRectangle.Size.Height);
			Invalidate(true);
		}

		private void CreateInitialBoard()
		{
			PlaceInitialStartingSquare(BOARD_HEIGHT / 2 - 1, BOARD_WIDTH / 2 - 1);
		}

		private void PlaceInitialStartingSquare(int startRow, int startColumn)
		{
			board[startRow, startColumn++] = FieldState.PlayerTwo;
			board[startRow++, startColumn] = FieldState.PlayerOne;
			board[startRow, startColumn--] = FieldState.PlayerTwo;
			board[startRow, startColumn] = FieldState.PlayerOne;
		}

		private void Render(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			ScreenOptions screenOptions = CalculateScreenOptions();
			int tileSize = screenOptions.TileSize;

			bool toggleColor = false;
			for (int row = 0; row < BOARD_HEIGHT; row++)
			{
				for (int column = 0; column < BOARD_WIDTH; column++)
				{
					// Draw the board with (dark) green tiles.
					Rectangle r = new Rectangle(
						column * tileSize + screenOptions.Offset.X,
						row * tileSize + screenOptions.Offset.Y, tileSize, tileSize);
					toggleColor = !toggleColor;
					g.FillRectangle(toggleColor ? Brushes.DarkGreen: Brushes.Green, r);

					switch (board[row, column])
					{
						case FieldState.Hint:
							g.FillEllipse(Brushes.WhiteSmoke, r);
							break;
						case FieldState.PlayerOne:
							g.FillEllipse(Brushes.Black, r);
							break;
						case FieldState.PlayerTwo:
							g.FillEllipse(Brushes.White, r);
							break;
						default:
							continue;
					}
				}

				toggleColor = !toggleColor;
			}
		}

		private ScreenOptions CalculateScreenOptions()
		{
			int sizeMin = Math.Min(graphics.Size.Width, graphics.Size.Height);
			int tileSize = sizeMin / Math.Max(BOARD_WIDTH, BOARD_HEIGHT);
			return new ScreenOptions
			{
				TileSize = tileSize,
				Offset = new Point(
					(graphics.Size.Width - tileSize * BOARD_WIDTH) / 2,
					(graphics.Size.Height - tileSize * BOARD_HEIGHT) / 2)
			};
		}

		private void OnClick(object sender, MouseEventArgs e)
		{
			ScreenOptions screenOptions = CalculateScreenOptions();
			// Translate the clicked location to a location on the board.
			int row = (int) Math.Ceiling((e.Y - screenOptions.Offset.Y) / (double) (BOARD_HEIGHT * screenOptions.TileSize) * BOARD_HEIGHT) - 1;
			int column = (int) Math.Ceiling((e.X - screenOptions.Offset.X) / (double) (BOARD_WIDTH * screenOptions.TileSize) * BOARD_WIDTH) - 1;

			// If the clicked tile is outside board bounds or
			// if the field has a piece, return.
			if (row < 0 || row > BOARD_HEIGHT - 1 ||
				column < 0 || column > BOARD_WIDTH - 1 ||
				board[row, column] != 0)
			{
				return;
			}
			
			var validPieces = CalculateValidPieces(row, column);
			// If the move is invalid, return.
			if (!validPieces.Any())
			{
				return;
			}

			// Change all valid pieces to the current player's color.
			foreach (var pieceToChange in validPieces)
			{
				board[pieceToChange.Y, pieceToChange.X] = currentPlayer;
			}

			// Set the clicked tile to the current player's color as well.
			board[row, column] = currentPlayer;

			// Swap players.
			currentPlayer = OtherPlayer();

			Invalidate(true);
		}

		private FieldState OtherPlayer()
		{
			return currentPlayer == FieldState.PlayerOne ? FieldState.PlayerTwo : FieldState.PlayerOne;
		}

		private IEnumerable<Point> CalculateValidPieces(int row, int column)
		{
			FieldState other = OtherPlayer();
			var validPieces = new List<Point>();

			foreach (Point d in ALL_DELTAS)
			{
				validPieces.AddRange(OtherPiecesInDirection(other, row, column, d.X, d.Y));
			}

			return validPieces;
		}

		private IEnumerable<Point> OtherPiecesInDirection(FieldState toCheck, int row, int column, int rowDelta, int columnDelta)
		{
			var otherPieces = new List<Point>();
			while (true)
			{
				// If inside board bounds ...
				if (row + rowDelta < BOARD_HEIGHT &&
				    row + rowDelta >= 0 &&
				    column + columnDelta < BOARD_WIDTH &&
				    column + columnDelta >= 0)
				{
					// ... Check if current tile + delta is 'correct' ...
					if (board[row + rowDelta, column + columnDelta] == toCheck)
					{
						// ... Add the current tile + delta to the correct tiles array.
						otherPieces.Add(new Point(column + columnDelta, row + rowDelta));
					}
					else
					{
						if (board[row + rowDelta, column + columnDelta] == currentPlayer && otherPieces.Any())
						{
							// We're done, the last tile is the current player's tile and we have other pieces in between.
							break;
						}

						// No more tiles in this direction and the last tile is not from the current player, so return an empty list.
						return Enumerable.Empty<Point>();
					}
				}
				else
				{
					// Outside of board bounds, so this move is invalid, return an empty list.
					return Enumerable.Empty<Point>();
				}

				// Continue with the next tile in this direction.
				row += rowDelta;
				column += columnDelta;
			}

			// This is a valid move, so return the valid changeable pieces.
			return otherPieces;
		}
	}
}