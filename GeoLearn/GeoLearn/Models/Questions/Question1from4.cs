using System;
using System.Collections.Generic;

namespace GeoLearn.Models
{
    public class Question1from4 : Question
    {
        public List<string> Answers { get; private set; }

        private static List<string> GetAnswers(string rightAnswer, string parametr, uint amount = 3, string language = "ru")
        {
            List<string> answers = new List<string> { rightAnswer };
            string ans;

            int loopCounter = 0;
            for (int ansNum = 0; ansNum < amount; ansNum++)
            {
                do
                {
                    if (loopCounter >= 40)
                        throw new ArgumentOutOfRangeException("to many tryes to find diffrent answer");
                    ans = Randomizer.RandomMineral(language: language, randomly: true)[parametr];
                    loopCounter++;
                } while (answers.Contains(ans));
                answers.Add(ans);
            }
            Randomizer.Shuffle(ref answers);
            return answers;
        }

        public Question1from4(Mineral mineral, string parametr, string language = "ru") : base(mineral)
        {
            parametr = parametr.ToLower();
            {
                if (Array.IndexOf(Mineral.parametrsList, parametr.ToLower()) == -1)
                    throw new MineralException($"Invalid parametr({parametr})!");
            }

            Answers = GetAnswers(mineral[parametr], parametr, language: language);
            CorrectAnswer = Answers.IndexOf(mineral[parametr]);

            if (language.ToLower() == "ru")
            {
                switch (parametr.ToLower())
                {
                    case "color":
                        Text = $"Какой цвет имеет";
                        break;
                    case "hardness":
                        Text = $"Какую твердость имеет";
                        break;
                    case "density":
                        Text = $"Какую плотность имеет";
                        break;
                    case "origin":
                        Text = $"Какое происхождение имеет";
                        break;
                    case "chemicalformula":
                        Text = $"Какую химическую формулу имеет";
                        break;
                    case "crystalshape":
                        Text = $"Какую форму нахождения имеет";
                        break;
                    case "streak":
                        Text = $"Какой цвет черты имеет";
                        break;
                    case "luster":
                        Text = $"Какой блеск имеет";
                        break;
                    case "trannsperency":
                        Text = $"Какую прозрачность имеет";
                        break;
                    case "cleavage":
                        Text = $"Какую спайность имеет";
                        break;
                    case "fracture":
                        Text = $"Какой излом имеет";
                        break;
                    case "usage":
                        Text = $"Какое применение имеет";
                        break;
                    case "diagnosticproperties":
                        Text = $"Какие диагностические черты имеет";
                        break;
                    case "specialproperties":
                        Text = $"Какими особыми свойствами обладает";
                        break;
                    default:
                        throw new ArgumentException($"Invalid parametr({parametr})!");
                }
                Text += $" {mineral.Name}?";
            }
            else if (language.ToLower() == "eng")
            {
                Text = $"Which {parametr} do {mineral.Name} have?";
            }
            else throw new MineralException($"Invalid language({language})!");
        }
    }
}
