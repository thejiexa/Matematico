namespace Matematico
{
    internal class Game
    {
        internal enum Card
        {
            Empty,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Eleven,
            Twelve,
            Thirteen,
        }

        public Card[,] Board { get; private set; }

        public int[] Points { get; private set; }

        public Game()
        {
            Board = new Card[5, 5];
            Points = new int[11];
        }

        protected void GetPoints()
        {
            Points = 
            [
                CountPoints(CardsCheck.GetDiagonal(Board), true),
                CountPoints(CardsCheck.GetColumn(Board, 0)),
                CountPoints(CardsCheck.GetColumn(Board, 1)),
                CountPoints(CardsCheck.GetColumn(Board, 2)),
                CountPoints(CardsCheck.GetColumn(Board, 3)),
                CountPoints(CardsCheck.GetColumn(Board, 4)),
                CountPoints(CardsCheck.GetRow(Board, 0)),
                CountPoints(CardsCheck.GetRow(Board, 1)),
                CountPoints(CardsCheck.GetRow(Board, 2)),
                CountPoints(CardsCheck.GetRow(Board, 3)),
                CountPoints(CardsCheck.GetRow(Board, 4)),
            ];
        }

        private int CountPoints(Card[] cards, bool diagonal = false)
        {
            bool[] checkedCards = 
            [
                CardsCheck.FourOfUnits(cards),
                CardsCheck.FourOfKind(cards),
                CardsCheck.OneTenElevenTwelveThirteen(cards),
                CardsCheck.ThreeOfUnitsAndTwoOfThirteen(cards),
                CardsCheck.FullHouse(cards),
                CardsCheck.Straight(cards),
                CardsCheck.ThreeOfKind(cards),
                CardsCheck.TwoPairs(cards),
                CardsCheck.Pair(cards)
            ];
            
            var points = checkedCards switch
            {
                ([true, _, _, _, _, _, _, _, _]) => 200,
                ([_, true, _, _, _, _, _, _, _]) => 160,
                ([ _, _, true, _, _, _, _, _, _]) => 150,
                ([ _, _, _, true, _, _, _, _, _]) => 100,
                ([ _, _, _, _, true, _, _, _, _]) => 80,
                ([ _, _, _, _, _, true, _, _, _]) => 50,
                ([ _, _, _, _, _, _, true, _, _]) => 40,
                ([ _, _, _, _, _, _, _, true, _]) => 20,
                ([ _, _, _, _, _, _, _, _,true]) => 10,
                _ => 0
            };

            return diagonal && points > 0 ? points + 10 : points;
        }

        private static class CardsCheck
        {
            public static Card[] GetDiagonal(Card[,] board) => Enumerable.Range(0, board.GetLength(1)).Select(i => board[i, i]).ToArray();
            public static Card[] GetColumn(Card[,] board, int column) => Enumerable.Range(0, board.GetLength(0)).Select(i => board[i, column]).ToArray();
            public static Card[] GetRow(Card[,] board, int row) => Enumerable.Range(0, board.GetLength(1)).Select(i => board[row, i]).ToArray();
            public static bool FourOfUnits(Card[] cards) => cards.GroupBy(_ => _).Any(x => x.Count() is 4 && x.Key is Card.One);
            public static bool FourOfKind(Card[] cards) => cards.GroupBy(_ => _).Any(x => x.Count() is 4 && x.Key is not Card.Empty);
            public static bool OneTenElevenTwelveThirteen(Card[] cards) => cards.OrderBy(_ => _).SequenceEqual([Card.One, Card.Ten, Card.Eleven, Card.Twelve, Card.Thirteen]);
            public static bool ThreeOfUnitsAndTwoOfThirteen(Card[] cards) => cards.OrderBy(_ => _).SequenceEqual([Card.One, Card.One, Card.One, Card.Thirteen, Card.Thirteen]);
            public static bool FullHouse(Card[] cards) => ThreeOfKind(cards) && Pair(cards);
            public static bool Straight(Card[] cards) => !cards.Contains(Card.Empty) && cards.OrderBy(_ => _).Select((x, i) => x - i).Distinct().Skip(1).Count() is 0;
            public static bool ThreeOfKind(Card[] cards) => cards.GroupBy(_ => _).Any(x => x.Count() is 3 && x.Key is not Card.Empty);
            public static bool TwoPairs(Card[] cards) => cards.GroupBy(_ => _).Where(x => x.Count() is 2 && x.Key is not Card.Empty).Count() is 2;
            public static bool Pair(Card[] cards) => cards.GroupBy(_ => _).Where(x => x.Count() is 2 && x.Key is not Card.Empty).Count() is 1;
        }
    }
}