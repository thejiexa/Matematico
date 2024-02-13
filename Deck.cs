namespace Matematico
{
    internal class Deck
    {
        public Stack<Game.Card> Cards { get; private set; }

        public Deck()
        {
            var intList = new List<int>();
            var cardList = new List<Game.Card>();
            var cards = new Stack<Game.Card>();

            intList.AddRange(Enumerable.Range(1, 13));
            intList.AddRange(Enumerable.Range(1, 13));
            intList.AddRange(Enumerable.Range(1, 13));
            intList.AddRange(Enumerable.Range(1, 13));

            ListShuffle(intList);
            intList.RemoveRange(25, 27);
            cardList = intList.ConvertAll(card => (Game.Card)card);
            cardList.ForEach(cards.Push);

            Cards = cards;
        }

        private static void ListShuffle<T>(List<T> list)
        {
            var random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public override string ToString() => string.Join(",\n", Cards);
    }
}