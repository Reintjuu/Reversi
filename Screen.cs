using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Reversi
{
	public partial class Screen : Form
	{
		// Fix to avoid flickering.
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams handleParam = base.CreateParams;
				handleParam.ExStyle |= 0x02000000;
				return handleParam;
			}
		}

		private const int BOARD_SIZE = 8;

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

		private readonly Brush darkGreen = new Pen(Color.DarkGreen).Brush;
		private readonly Brush green = new Pen(Color.Green).Brush;
		private readonly Brush white = new Pen(Color.White).Brush;
		private readonly Brush black = new Pen(Color.Black).Brush;

		private readonly PictureBox graphics;
		private readonly int[,] board;
		private int currentPlayer;

		public Screen()
		{
			graphics = new PictureBox();
			graphics.Size = new Size(ClientRectangle.Size.Width, ClientRectangle.Size.Height);
			graphics.Paint += Render;
			Resize += UpdateBoardSize;
			graphics.MouseClick += OnClick;
			Controls.Add(graphics);

			board = new int[BOARD_SIZE, BOARD_SIZE];
			CreateInitialBoard();
			currentPlayer = 1;

			InitializeComponent();
		}

		private void UpdateBoardSize(object sender, EventArgs e)
		{
			Control control = (Control) sender;
			graphics.Size = new Size(control.ClientRectangle.Size.Width, control.ClientRectangle.Size.Height);
		}

		private void CreateInitialBoard()
		{
			PlaceInitialStartingSquare(BOARD_SIZE / 2 - 1, BOARD_SIZE / 2 - 1);
		}

		private void PlaceInitialStartingSquare(int startRow, int startColumn)
		{
			board[startRow, startColumn++] = 2;
			board[startRow++, startColumn] = 1;
			board[startRow, startColumn--] = 2;
			board[startRow, startColumn] = 1;
		}

		private void Render(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			ScreenOptions screenOptions = CalculateScreenOptions();
			int tileSize = screenOptions.TileSize;

			bool toggleColor = false;
			for (int row = 0; row < BOARD_SIZE; row++)
			{
				for (int column = 0; column < BOARD_SIZE; column++)
				{
					Rectangle r = new Rectangle(
						column * tileSize + screenOptions.Offset.X,
						row * tileSize + screenOptions.Offset.Y, tileSize, tileSize);
					toggleColor = !toggleColor;
					g.FillRectangle(toggleColor ? darkGreen : green, r);

					if (board[row, column] == 0)
					{
						continue;
					}

					g.FillEllipse(board[row, column] == 1 ? black : white, r);
				}

				toggleColor = !toggleColor;
			}
		}

		private ScreenOptions CalculateScreenOptions()
		{
			int sizeMin = Math.Min(graphics.Size.Width, graphics.Size.Height);
			int tileSize = sizeMin / BOARD_SIZE;
			return new ScreenOptions
			{
				TileSize = tileSize,
				Offset = new Point(
					(graphics.Size.Width - tileSize * BOARD_SIZE) / 2,
					(graphics.Size.Height - tileSize * BOARD_SIZE) / 2)
			};
		}

		private void OnClick(object sender, MouseEventArgs e)
		{
			ScreenOptions screenOptions = CalculateScreenOptions();
			int row = (int) Math.Ceiling((e.Y - screenOptions.Offset.Y) / (double) (BOARD_SIZE * screenOptions.TileSize) * BOARD_SIZE) - 1;
			int column = (int) Math.Ceiling((e.X - screenOptions.Offset.X) / (double) (BOARD_SIZE * screenOptions.TileSize) * BOARD_SIZE) - 1;

			// If the field has a piece, return.
			if (board[row, column] != 0 || !IsValid(row, column))
			{
				return;
			}

			board[row, column] = currentPlayer;

			// Swap players.
			currentPlayer = OtherPlayer();

			Invalidate();
		}

		private int OtherPlayer()
		{
			return currentPlayer == 1 ? 2 : 1;
		}

		private bool IsValid(int row, int column)
		{
			int other = OtherPlayer();
			bool valid = false;

			foreach (var d in ALL_DELTAS)
			{
				foreach (var pieceToChange in OtherPiecesInDirection(other, row, column, d.X, d.Y))
				{
					// Change a valid piece.
					board[pieceToChange.Y, pieceToChange.X] = currentPlayer;
					// Apparently this is a valid move, otherwise we wouldn't have changed a piece.
					valid = true;
				}
			}

			return valid;
		}

		private List<Point> OtherPiecesInDirection(int toCheck, int row, int column, int rowDelta, int columnDelta)
		{
			var otherPieces = new List<Point>();
			while (true)
			{
				// If inside board bounds ...
				if (row + rowDelta < BOARD_SIZE && row + rowDelta >= 0 && column + columnDelta < BOARD_SIZE &&
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
							// We're done, the last tile is a player tile and we have other pieces in between.
							break;
						}

						// No more tiles in this direction and the last tile is not a player, so return an empty list.
						return new List<Point>();
					}
				}
				else
				{
					// Outside of board bounds, so break the loop.
					break;
				}

				row += rowDelta;
				column += columnDelta;
			}

			return otherPieces;
		}
	}
}