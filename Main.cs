namespace Matematico
{
    public partial class Main : Form
    {
        private readonly Dictionary<int, Image> images = new()
        {
            [1] = Image.FromFile(@"../../../resources/1.png"),
            [2] = Image.FromFile(@"../../../resources/2.png"),
            [3] = Image.FromFile(@"../../../resources/3.png"),
            [4] = Image.FromFile(@"../../../resources/4.png"),
            [5] = Image.FromFile(@"../../../resources/5.png"),
            [6] = Image.FromFile(@"../../../resources/6.png"),
            [7] = Image.FromFile(@"../../../resources/7.png"),
            [8] = Image.FromFile(@"../../../resources/8.png"),
            [9] = Image.FromFile(@"../../../resources/9.png"),
            [10] = Image.FromFile(@"../../../resources/10.png"),
            [11] = Image.FromFile(@"../../../resources/11.png"),
            [12] = Image.FromFile(@"../../../resources/12.png"),
            [13] = Image.FromFile(@"../../../resources/13.png"),
            [14] = Image.FromFile(@"../../../resources/win.png"),
            [15] = Image.FromFile(@"../../../resources/lose.png"),
            [16] = Image.FromFile(@"../../../resources/draw.png")
        };

        readonly PictureBox[,] playerBoard = new PictureBox[6, 6];
        readonly PictureBox[,] opponentBoard = new PictureBox[6, 6];
        readonly PictureBox imagePb = new();
        readonly Label playerLabel = new();
        readonly Label opponentLabel = new();
        readonly Description description = new();
        readonly MenuStrip menuStrip = new();
        readonly Point pbSize = new(95, 75);
        readonly Font font = new("Calibri", 30, FontStyle.Bold, GraphicsUnit.Pixel);
        Game.Card currentCard;
        Deck deck;
        Player player;
        Opponent opponent;

        public Main()
        {
            InitializeComponent();
            BuildForm();
            NewGame();
        }

        private void NewGame()
        {
            ClearBoards();
            player = new Player();
            opponent = new Opponent();
            deck = new Deck();
            currentCard = deck.Cards.Pop();
            imagePb.Image = images[(int)currentCard];
            ShowScores(player, playerBoard, playerLabel);
            ShowScores(opponent, opponentBoard, opponentLabel);
        }

        private void BuildForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Width = pbSize.X * 15 + 60;
            Height = pbSize.Y * 10;
            Text = "Matematico";
            Icon = Icon.ExtractAssociatedIcon(@"../../../resources/icon.ico");
            BackColor = Color.DodgerBlue;

            string[] row = ["New Gme", "Description", "Close"];
            menuStrip.Font = new("Calibri", 15, FontStyle.Bold, GraphicsUnit.Pixel);

            foreach (string rw in row)
                menuStrip.Items.Add(new ToolStripMenuItem(rw));

            menuStrip.Items[0].Click += NewGame_Click;
            menuStrip.Items[1].Click += Description_Click;
            menuStrip.Items[2].Click += Close_Click;

            imagePb.Size = new Size(pbSize.X * 2, pbSize.X * 2);
            imagePb.Location = new Point(pbSize.X * 6 + (pbSize.X / 2) + 20, 150);
            imagePb.SizeMode = PictureBoxSizeMode.StretchImage;

            Controls.Add(menuStrip);
            Controls.Add(imagePb);

            BuildPlayerBoard();
            BuildOpponentBoard();
        }

        private void BuildPlayerBoard()
        {
            for (int i = 0; i < playerBoard.GetLength(0); i++)
            {
                for (int j = 0; j < playerBoard.GetLength(1); j++)
                {
                    var p = new PictureBox();
                    p.Location = new Point(p.Location.X + (i * pbSize.X) + 15, p.Location.Y + (j * pbSize.Y) + 35);
                    p.Size = new Size(pbSize);
                    p.BorderStyle = BorderStyle.Fixed3D;
                    p.Cursor = i == 5 || j == 5 ? Cursors.No : Cursors.Hand;
                    p.SizeMode = PictureBoxSizeMode.StretchImage;

                    playerBoard[i, j] = p;
                    playerBoard[i, j].BackColor = i == 5 || j == 5 ? Color.DarkBlue : j == i ? Color.LightPink : Color.White;
                    Controls.Add(playerBoard[i, j]);

                    if (i is not 5 && j is not 5)
                    {
                        playerBoard[i, j].Tag = new Point(i, j);
                        playerBoard[i, j].Click += Board_Click;
                    }
                }
            }

            playerLabel.Location = new Point(playerLabel.Location.X + 15, playerLabel.Location.Y + pbSize.Y * 7);
            playerLabel.Font = font;
            playerLabel.Size = new Size(pbSize.X * 7, pbSize.Y * 4);
            Controls.Add(playerLabel);
        }

        private void BuildOpponentBoard()
        {
            for (int i = 0; i < opponentBoard.GetLength(0); i++)
            {
                for (int j = 0; j < opponentBoard.GetLength(1); j++)
                {
                    var p = new PictureBox();
                    p.Location = new Point(p.Location.X + (i * pbSize.X) + (pbSize.X * 9) + 25, p.Location.Y + (j * pbSize.Y) + 35);
                    p.Size = new Size(pbSize);
                    p.BorderStyle = BorderStyle.Fixed3D;
                    p.Cursor = Cursors.No;
                    p.SizeMode = PictureBoxSizeMode.StretchImage;

                    opponentBoard[i, j] = p;
                    opponentBoard[i, j].BackColor = j == 5 || i == 5 ? Color.Maroon : j == i ? Color.Cyan : Color.White;
                    Controls.Add(opponentBoard[i, j]);
                }
            }

            opponentLabel.Location = new Point(opponentLabel.Location.X + (pbSize.X * 9) + 25, opponentLabel.Location.Y + pbSize.Y * 7);
            opponentLabel.Font = font;
            opponentLabel.Size = new Size(pbSize.X * 7, pbSize.Y * 4);
            Controls.Add(opponentLabel);
        }

        private void RefreshGameBoard(Game game, PictureBox[,] board)
        {
            for (byte i = 0; i < board.GetLength(0) - 1; i++)
            {
                for (byte j = 0; j < board.GetLength(1) - 1; j++)
                {
                    var card = (int)game.Board[i, j];
                    board[i, j].Cursor = card is 0 && game is Player ? Cursors.Hand : Cursors.No;
                    board[i, j].Image = card is 0 ? null : images[card];
                }
            }
        }

        private void ClearBoards()
        {
            for (byte i = 0; i < 5; i++)
            {
                for (byte j = 0; j < 5; j++)
                {
                    opponentBoard[i, j].Image = null;
                    playerBoard[i, j].Image = null;
                    playerBoard[i, j].Cursor = Cursors.Hand;
                }
            }

            imagePb.Image = null;
            playerLabel.Text = opponentLabel.Text = null;
        }

        private void ShowScores(Game game, PictureBox[,] board, Label label)
        {
            for (int i = 0; i < game.Points.GetLength(0); i++)
            {
                var pointsImage = new Bitmap(pbSize.X, pbSize.Y);
                var graphics = Graphics.FromImage(pointsImage);
                var location = game.Points[i] switch
                {
                    > 99 => new Point(pbSize.X / 5, pbSize.Y / 5),
                    > 0 => new Point(pbSize.X / 4, pbSize.Y / 5),
                    _ => new Point(pbSize.X / 3, pbSize.Y / 5),
                };

                graphics.DrawString(game.Points[i].ToString(), font, Brushes.White, location);

                Action drawPoints = i switch
                {
                    1 => () => board[5, 0].Image = pointsImage,
                    2 => () => board[5, 1].Image = pointsImage,
                    3 => () => board[5, 2].Image = pointsImage,
                    4 => () => board[5, 3].Image = pointsImage,
                    5 => () => board[5, 4].Image = pointsImage,
                    6 => () => board[0, 5].Image = pointsImage,
                    7 => () => board[1, 5].Image = pointsImage,
                    8 => () => board[2, 5].Image = pointsImage,
                    9 => () => board[3, 5].Image = pointsImage,
                    10 => () => board[4, 5].Image = pointsImage,
                    _ => () => board[5, 5].Image = pointsImage,
                };

                drawPoints();
            }

            var rows = string.Join(", ", game.Points[1..6]);
            var cols = string.Join(", ", game.Points[6..11]);
            label.Text = string.Format("Diagonal: {0}\nRows: {1}\nColumns: {2}\nTotal: {3}", game.Points.First(), rows, cols, game.Points.Sum());
        }

        private void Board_Click(object sender, EventArgs e)
        {
            var p = (Point)(sender as Control).Tag;

            if (player.Board[p.X, p.Y] is not Game.Card.Empty) 
                return;

            player.Move(p, currentCard);
            opponent.Move(currentCard);

            RefreshGameBoard(player, playerBoard);
            RefreshGameBoard(opponent, opponentBoard);

            if (deck.Cards.Count is not 0)
            {
                currentCard = deck.Cards.Pop();
                imagePb.Image = images[(int)currentCard];
            }
            else
            {
                var playerSum = player.Points.Sum();
                var opponentSum = opponent.Points.Sum();
                imagePb.Image = playerSum.CompareTo(opponentSum) switch
                {
                    1 => images[14],
                    -1 => images[15],
                    _ => images[16]
                };
            }

            ShowScores(player, playerBoard, playerLabel);
            ShowScores(opponent, opponentBoard, opponentLabel);
        }

        private void Description_Click(object? sender, EventArgs e) => description.ShowDialog();
        private void NewGame_Click(object? sender, EventArgs e) => NewGame();
        private void Close_Click(object? sender, EventArgs e) => Close();
    }
}