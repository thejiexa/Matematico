namespace Matematico
{
    internal class Player : Game
    {
        public void Move(Point cell, Card card)
        {
            Board[cell.X, cell.Y] = card;

            GetPoints();
        }
    }
}