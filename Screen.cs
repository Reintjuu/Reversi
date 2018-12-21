using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
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
		private const int BOARD_WIDTH = 6;
		private const int BOARD_HEIGHT = 6;

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

		private readonly SolidBrush playerOneHintBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
		private readonly SolidBrush playerTwoHintBrush = new SolidBrush(Color.FromArgb(128, 255, 255, 255));

		//private readonly PictureBox canvas;
		private FieldState[,] board;

		private FieldState currentPlayer;
		private int playerOnePieces;
		private int playerTwoPieces;
		private int passAmount = 0;
		private bool showHints = true;
		private bool gameEnded;

		public Screen()
		{
			InitializeComponent();

			canvas.Paint += Render;
			Resize += UpdateBoardSize;
			canvas.MouseClick += OnClick;
			Controls.Add(canvas);

			NewGame();
		}

		private void UpdateBoardSize(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			canvas.Size = new Size(control.ClientSize.Width, control.ClientSize.Height - canvas.Location.Y);
			Invalidate(true);
		}

		private void NewGame(object sender = null, EventArgs e = null)
		{
			gameEnded = false;
			board = new FieldState[BOARD_HEIGHT, BOARD_WIDTH];
			CreateInitialBoard();
			currentPlayer = FieldState.PlayerOne;

			DetermineBoardState();
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
			int hintSizeDifference = tileSize / 2;

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
					g.FillRectangle(toggleColor ? Brushes.DarkGreen : Brushes.Green, r);

					switch (board[row, column])
					{
						case FieldState.Hint:
							if (showHints)
							{
								g.FillEllipse(currentPlayer == FieldState.PlayerOne ? playerOneHintBrush : playerTwoHintBrush,
									r.X + hintSizeDifference / 2,
									r.Y + hintSizeDifference / 2,
									tileSize - hintSizeDifference,
									tileSize - hintSizeDifference);
							}
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
			int sizeMin = Math.Min(canvas.Size.Width, canvas.Size.Height);
			int tileSize = sizeMin / Math.Max(BOARD_WIDTH, BOARD_HEIGHT);
			return new ScreenOptions
			{
				TileSize = tileSize,
				Offset = new Point(
					(canvas.Size.Width - tileSize * BOARD_WIDTH) / 2,
					(canvas.Size.Height - tileSize * BOARD_HEIGHT) / 2)
			};
		}

		private void OnClick(object sender, MouseEventArgs e)
		{
			ScreenOptions screenOptions = CalculateScreenOptions();
			// Translate the clicked location to a location on the board.
			int row = (int)Math.Ceiling((e.Y - screenOptions.Offset.Y) / (double)(BOARD_HEIGHT * screenOptions.TileSize) * BOARD_HEIGHT) - 1;
			int column = (int)Math.Ceiling((e.X - screenOptions.Offset.X) / (double)(BOARD_WIDTH * screenOptions.TileSize) * BOARD_WIDTH) - 1;

			// If the clicked tile is outside board bounds or
			// if the field has a piece, return.
			if (row < 0 || row > BOARD_HEIGHT - 1 ||
				column < 0 || column > BOARD_WIDTH - 1 ||
				board[row, column] == FieldState.PlayerOne ||
				board[row, column] == FieldState.PlayerTwo)
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

			SwapPlayers();

			Invalidate(true);
		}

		private void SwapPlayers()
		{
			currentPlayer = OtherPlayer();

			// If there are no valid moves, enable the pass button.
			bool boardHasValidMoves = DetermineBoardState();
			passButton.Enabled = !boardHasValidMoves && !gameEnded;
			if (boardHasValidMoves)
			{
				passAmount = 0;
			}
		}

		private FieldState OtherPlayer()
		{
			return currentPlayer == FieldState.PlayerOne ? FieldState.PlayerTwo : FieldState.PlayerOne;
		}

		/// <summary>
		/// Counts the pieces per player and set hints where needed.
		/// </summary>
		/// <returns>True if the board still has moves available for the current player, else false.</returns>
		private bool DetermineBoardState()
		{
			playerOnePieces = 0;
			playerTwoPieces = 0;

			int hintAmount = 0;

			for (int row = 0; row < BOARD_HEIGHT; row++)
			{
				for (int column = 0; column < BOARD_WIDTH; column++)
				{
					if (board[row, column] == FieldState.PlayerOne)
					{
						playerOnePieces++;
						// Don't set hints to the fields that already contain a player piece.
					}
					else if (board[row, column] == FieldState.PlayerTwo)
					{
						playerTwoPieces++;
					}
					else if (CalculateValidPieces(row, column).Any())
					{
						// If the field has a valid move, place a hint.
						board[row, column] = FieldState.Hint;
						hintAmount++;
					}
					else
					{
						// If it doesn't, make it empty.
						board[row, column] = FieldState.Empty;
					}
				}
			}

			gameEnded = playerOnePieces + playerTwoPieces == BOARD_WIDTH * BOARD_HEIGHT;

			UpdateUI();
			// Zero hints mean we don't have any moves available.
			return hintAmount != 0;
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

		private void PassButton_Click(object sender, EventArgs e)
		{
			if (passAmount == 1)
			{
				gameEnded = true;
				passButton.Enabled = false;
				DetermineBoardState();
				Invalidate(true);
				return;
			}

			passAmount++;
			SwapPlayers();
			Invalidate(true);
		}

		private void HintButton_Click(object sender, EventArgs e)
		{
			showHints = !showHints;
			hintButton.Text = showHints ? "Hide Hints" : "Show Hints";
			Invalidate(true);
		}

		private void UpdateUI()
		{
			StringBuilder sb = new StringBuilder();

			if (gameEnded)
			{
				if (playerOnePieces == playerTwoPieces)
				{
					sb.Append("Remise");
				}
				else if (playerOnePieces > playerTwoPieces)
				{
					sb.Append("Black has won!");
				}
				else
				{
					sb.Append("White has won!");
				}

				// Because labels don't support tabs, we're just adding 8 spaces.
				sb.Append("        ");
			}

			if (!gameEnded && currentPlayer == FieldState.PlayerOne)
			{
				sb.Append("Black's Turn        ");
			}
			sb.Append($"Black: {playerOnePieces}\n");

			if (!gameEnded && currentPlayer == FieldState.PlayerTwo)
			{
				sb.Append("White's Turn        ");
			}
			sb.Append($"White: {playerTwoPieces}");

			gameStatsLabel.Text = sb.ToString();
		}
	}
}