using System;

namespace GeoLearn.Models
{
    public class QuestionYesNo : Question
    {
        public QuestionYesNo(Mineral mineral, string parametr, string lenguage = "ru") : base(mineral)
        {
            parametr = parametr.ToLower();
            {
                if (Array.IndexOf(Mineral.parametrsList, parametr) == -1)
                    throw new MineralException($"Invalid parametr({parametr})!");
            }

            //Answers = GetAnswers(mineral[parametr], parametr, lenguage: lenguage);
            CorrectAnswer = Randomizer.Randint(2);//Answers.IndexOf(mineral[parametr]);

            Text = mineral.Name + '\n';

            string questionParametr;
            if (CorrectAnswer == 0)
                questionParametr = mineral[parametr];
            else
                questionParametr = Randomizer.RandomMineral(mineral, true)[parametr];

            if (lenguage.ToLower() == "ru")
            {
                Text += Mineral.parametrsListRU[Array.IndexOf(Mineral.parametrsList, parametr)] +
                    " - " + questionParametr + '?';
            }
            else if (lenguage.ToLower() == "eng")
            {
                Text = $"{parametr} - {questionParametr}?";
            }
            else throw new MineralException($"Invalid lenguage({lenguage})!");
        }
    }
}
