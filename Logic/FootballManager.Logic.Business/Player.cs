namespace FootballManager.Logic.Business
{
    public class Player
    {
        public string Name { get; set; }
        public int Elo { get; set; }
        public BibColor BibColor { get; set; }
    }

    public enum BibColor
    {
        Orange,
        Red,
        Blue,
        Green
    }
}