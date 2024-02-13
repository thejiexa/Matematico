namespace Matematico
{
    public partial class Description : Form
    {
        public Description()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Height = 700;
            Width = 900;
            BackColor = Color.DodgerBlue;
            Text = "Description";

            Label label = new()
            {
                Location = new Point(20, 20),
                Font = new Font("Calibri", 25, FontStyle.Bold, GraphicsUnit.Pixel),
                Size = new Size(Width - 50, Height),
                Text = "\"Matematico\" is an Italian mathematical game played on a 5x5 grid. There is a deck of 52 cards, each containing numbers from 1 to 13, with each number appearing four times. The goal is to create combinations using the numbers on the board to earn the highest number of points. Each combination is associated with a specific number of points.\r\n\r\n" +
                    "Combinations and Points:\r\n\r\n" +
                    "   - For 2 identical numbers: 10 points.\r\n" +
                    "   - For 2 pairs of identical numbers: 20 points.\r\n" +
                    "   - For 3 identical numbers: 40 points.\r\n" +
                    "   - For 5 consecutive numbers, not necessarily in order: 50 points.\r\n" +
                    "   - For 3 identical numbers and 2 other identical numbers: 80 points.\r\n" +
                    "   - Three times of 1 and two times of 13: 100 points.\r\n" +
                    "   - Numbers 1, 13, 12, 11 and 10, but not necessarily in order: 150 points.\r\n" +
                    "   - For 4 identical numbers: 160 points.\r\n" +
                    "   - For 4 units: 200 points.\r\n\r" +
                    "In addition to the standard points, players receive an extra 10 points for each completed combination along a primal diagonal on the game board."
            };

            Controls.Add(label);
        }
    }
}