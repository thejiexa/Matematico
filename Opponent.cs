namespace Matematico
{
    class Opponent : Game
    {
        private readonly List<Point> _reservedCells =
        [
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(4, 0),
            new Point(4, 1),
            new Point(4, 2),
            new Point(4, 3),
            new Point(4, 4),
            new Point(1, 1),
            new Point(2, 2),
            new Point(3, 3)
        ];

        private readonly List<Point> _freeCells = Enumerable.Range(0, 5)
                .SelectMany(x => Enumerable.Range(0, 5)
                .Select(y => new Point(x, y))).ToList();

        private readonly Random _random = new();

        public void Move(Card card)
        {
            Point usedCell = new(-1, 0);
            Point? x4comboPosition = Get4xComboPosition(card);

            Action bestPossibleMove = (card) switch
            {
                Card.Two or Card.Three or Card.Four or Card.Five => () =>
                {
                    var cards = GetRow(Board, 0);

                    if (!cards[1..].Contains(card) && cards[1..].Contains(Card.Empty))
                    {
                        for (int i = 4; i > 0; i--)
                        {
                            if (Board[0, i] == Card.Empty)
                            {
                                Board[0, i] = card;
                                usedCell = new Point(0, i);
                                return;
                            }
                        }
                    }
                    else
                    {
                        var cell = FindBestPosition(card);
                        usedCell = cell is not null && _freeCells.Contains(cell.Value) && !_reservedCells.Contains(cell.Value) ? cell.Value : GetRandomCell();
                        Board[usedCell.X, usedCell.Y] = card;
                    }
                },
                Card.Six or Card.Seven or Card.Eight or Card.Nine => () =>
                {
                    var cards = GetRow(Board, 4);

                    if (!cards[..4].Contains(card) && cards[..4].Contains(Card.Empty))
                    {
                        for (int i = 3; i >= 0; i--)
                        {
                            if (Board[4, i] == Card.Empty)
                            {
                                Board[4, i] = card;
                                usedCell = new Point(4, i);
                                return;
                            }
                        }
                    }
                    else
                    {
                        var cell = FindBestPosition(card);
                        usedCell = cell is not null && _freeCells.Contains(cell.Value) && !_reservedCells.Contains(cell.Value) ? cell.Value : GetRandomCell();
                        Board[usedCell.X, usedCell.Y] = card;
                    }
                },
                Card.Eleven or Card.Twelve or Card.Thirteen => () =>
                {
                    var cards = GetDiagonal(Board);

                    if (!cards[1..4].Contains(card) && cards[1..4].Contains(Card.Empty))
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            if (Board[i, i] == Card.Empty)
                            {
                                Board[i, i] = card;
                                usedCell = new Point(i, i);
                                return;
                            }
                        }
                    }
                    else
                    {
                        var cell = FindBestPosition(card);
                        usedCell = cell is not null && _freeCells.Contains(cell.Value) && !_reservedCells.Contains(cell.Value) ? cell.Value : GetRandomCell();
                        Board[usedCell.X, usedCell.Y] = card;
                    }
                },
                Card.Ten => () =>
                {
                    if (Board[4, 4] is Card.Empty)
                    {
                        Board[4, 4] = Card.Ten;
                        usedCell = new Point(4, 4);
                    }
                    else
                    {
                        var cell = FindBestPosition(card);
                        usedCell = cell is not null && _freeCells.Contains(cell.Value) && !_reservedCells.Contains(cell.Value) ? cell.Value : GetRandomCell();
                        Board[usedCell.X, usedCell.Y] = card;
                    }
                },
                _ => () =>
                {
                    if (Board[0, 0] is Card.Empty)
                    {
                        Board[0, 0] = Card.One;
                        usedCell = new(0, 0);
                    }
                    else
                    {
                        var cell = FindBestPosition(card);
                        usedCell = cell is not null && _freeCells.Contains(cell.Value) && !_reservedCells.Contains(cell.Value) ? cell.Value : GetRandomCell();
                        Board[usedCell.X, usedCell.Y] = card;
                    }
                }
            };

            Action move = (x4comboPosition is not null, _freeCells.Count is 1) switch
            {
                (true, _) => () =>
                {
                    Board[x4comboPosition.Value.X, x4comboPosition.Value.Y] = card;
                    usedCell = x4comboPosition.Value;
                },
                (_, true) => () =>
                {
                    var cell = _freeCells.First();
                    Board[cell.X, cell.Y] = card;
                },
                _ => () => bestPossibleMove()
            };

            move();

            GetPoints();
            _reservedCells.Remove(usedCell);
            _freeCells.Remove(usedCell);

            if (_freeCells?.Count < 6)
                _reservedCells?.Clear();
        }

        private Point GetRandomCell()
        {
            var point = _freeCells[_random.Next(_freeCells.Count)];
            return _reservedCells.Contains(point) ? GetRandomCell() : point;
        }

        private Point? Get4xComboPosition(Card card)
        {
            List<Card> column;
            List<Card> row;
            List<Card> diagonal = GetDiagonal(Board);

            for (int i = 0; i < Board.GetLength(0); i++)
            {
                row = GetColumn(Board, i);
                column = GetRow(Board, i);

                bool[] found =
                [
                    X4Combo(row, card),
                    X4Combo(column, card),
                    X4Combo(diagonal, card)
                ];

                if (found.Contains(true))
                {
                    return found switch
                    {
                        ([true, _, _]) => new Point(row.FindIndex(x => x is Card.Empty), i),
                        ([_, true, _]) => new Point(i, column.FindLastIndex(x => x is Card.Empty)),
                        _ => new Point(column.FindLastIndex(x => x is Card.Empty), column.FindLastIndex(x => x is Card.Empty))
                    };
                }
            }

            return null;
        }

        private Point? FindBestPosition(Card card)
        {
            List<Card> column;
            List<Card> row;
            List<Card> diagonal = GetDiagonal(Board);
            Card[] set = [Card.One, Card.Ten, Card.Eleven, Card.Twelve, Card.Thirteen];
            bool preferredSide = set.Contains(card);
            bool found;
            Point position;

            for (int i = Board.GetLength(0) - 1; i >= 0; i--)
            {
                row = GetColumn(Board, i);
                found = X3Combo(row, card);

                if (found && !preferredSide)
                {
                    for (int j = 4; j >= 0; j--)
                    {
                        position = new Point(j, i);
                        if (Board[j, i] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }

                }
                else if (found && preferredSide)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        position = new Point(j, i);
                        if (Board[j, i] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }
                }
            }

            for (int i = Board.GetLength(0) - 1; i >= 0; i--)
            {
                column = GetRow(Board, i);
                found = X3Combo(column, card);

                if (found)
                {
                    for (int j = 4; j >= 0; j--)
                    {
                        position = new Point(i, j);
                        if (Board[i, j] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }
                }
            }

            for (int i = Board.GetLength(0) - 1; i >= 0; i--)
            {
                row = GetColumn(Board, i);
                found = X2Combo(row, card);

                if (found && !preferredSide)
                {
                    for (int j = 4; j >= 0; j--)
                    {
                        position = new Point(j, i);
                        if (Board[j, i] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }

                }
                else if (found && preferredSide)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        position = new Point(j, i);
                        if (Board[j, i] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }
                }
            }

            for (int i = Board.GetLength(0) - 1; i >= 0; i--)
            {
                column = GetRow(Board, i);
                found = X2Combo(column, card);

                if (found)
                {
                    for (int j = 4; j >= 0; j--)
                    {
                        position = new Point(i, j);
                        if (Board[i, j] == Card.Empty && !_reservedCells.Contains(position))
                            return position;
                    }
                }
            }

            return null;
        }

        private static List<Card> GetDiagonal(Card[,] board) => Enumerable.Range(0, board.GetLength(1)).Select(i => board[i, i]).ToList();
        private static List<Card> GetColumn(Card[,] board, int column) => Enumerable.Range(0, board.GetLength(0)).Select(i => board[i, column]).ToList();
        private static List<Card> GetRow(Card[,] board, int row) => Enumerable.Range(0, board.GetLength(1)).Select(i => board[row, i]).ToList();
        private static bool X4Combo(List<Card> cards, Card card) => cards.Count(x => x == card) is 3 && cards.Contains(Card.Empty);
        private static bool X3Combo(List<Card> cards, Card card) => cards.Count(x => x == card) is 2 && cards.Contains(Card.Empty);
        private static bool X2Combo(List<Card> cards, Card card) => cards.Count(x => x == card) is 1 && cards.Contains(Card.Empty);
    }
}