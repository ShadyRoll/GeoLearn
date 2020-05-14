namespace GeoLearn.Models
{
    public abstract class Question
    {
        public Question(Mineral mineral)
        {
            Mineral = mineral;
        }

        public enum QestionType
        {
            _1from4,
            MisteryMineral,
            Exam,
            Miner
        }

        public Mineral Mineral { get; protected set; }
        public string Text { get; protected set; }
        public string Parametr { get; protected set; }
        public int CorrectAnswer { get; protected set; }
        public bool Answered { get; set; } = false;
    }
}
