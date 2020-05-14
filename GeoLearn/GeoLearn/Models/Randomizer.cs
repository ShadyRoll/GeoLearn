using GeoLearn.Models.Minerals;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GeoLearn.Models
{
    public static class Randomizer
    {
        public enum EducationAlgorithmMode
        {
            GeoLearnAdaptive,
            RandomQuestions
        };

        public static EducationAlgorithmMode Mode;

        static Random random = new Random();

        public static int Randint(int left, int right) => random.Next(left, right);

        public static int Randint(int right) => random.Next(0, right);

        public static void Shuffle(ref List<string> list)
        {
            List<string> ans = new List<string>();
            int randint;
            for (int i = 0; i < list.Count; i++)
            {
                do
                {
                    randint = Randint(4);
                } while (ans.Contains(list[randint]));
                ans.Add(list[randint]);
            }
            list = ans;
        }

        public static void Shuffle(ref string[] arr)
        {
            string[] ans = new string[4];
            int randint;
            for (int i = 0; i < arr.Length; i++)
            {
                do
                {
                    randint = Randint(4);
                } while (ans[0] != arr[randint] && ans[1] != arr[randint] && ans[2] != arr[randint]);
                ans[i] = arr[randint];
            }

            arr = ans;
        }

        public static Mineral RandomMineral(Mineral previousMineral = null, bool randomly = false, string language = "ru")
        {
            // Инициализируем переменные
            string lang = language.ToLower();
            var en = MineralsList.mineralsList[lang].Keys.GetEnumerator();
            int len;
            
            // Если выбран алгортим "Случайные вопросы"
            if (Mode == EducationAlgorithmMode.RandomQuestions || randomly)
            {
                // Выбираем абсолютно случайный минерал
                len = random.Next(MineralsList.mineralsList[lang].Count);
                for (int i = 0; i <= len; i++)
                    en.MoveNext();
            }
            // Если выбран алгортим "GeoLearn Adaptive"
            else if (Mode == EducationAlgorithmMode.GeoLearnAdaptive)
            {
                // Выбираем минерал с шансом пропорциональным его приоритету
                len = random.Next(MineralsList.SumPriority);

                for (int i = 0; i < MineralsList.mineralsList[lang].Count; i++)
                {
                    en.MoveNext();
                    len -= MineralsList.mineralsList[lang][en.Current].Priority;
                    if (len <= 0)
                        break;
                }
            }

            // Если произошло совпадение выбранного и предудещего минералов
            if (MineralsList.mineralsList[lang][en.Current] == previousMineral)
            {
                // Изменяем на минерал, отличный от текущего
                len = random.Next(1, MineralsList.mineralsList[lang].Count - 1);
                for (int i = 0; i < len; i++)
                    en.MoveNext();
            }

            // Если в какой-то момент мы оказались вышли за границы списка, берем 1й элемент
            if (en.Current == null)
            {
                en = MineralsList.mineralsList[lang].Keys.GetEnumerator();
                en.MoveNext();
            }

            return MineralsList.mineralsList[lang][en.Current];
        }

        public enum QuestionType
        {
            _1from4,
            yesNo
        }

        public static Question RandomQuestion(Mineral mineral = null, string parametr = "random", QuestionType questionType = QuestionType._1from4, Question previousQuestion = null, bool randomly = false, string language = "ru")
        {
            try
            {
                parametr = parametr.ToLower();
                // Если конкретный минерал не указан, выберем новый
                if (mineral == null)
                    mineral = RandomMineral(randomly: randomly);
                // Если конкретный параметр не указан, выберем новый
                if (parametr == "random")
                    parametr = GetRandomParametr(mineral, previousQuestion);

                Question returningQuestion = null;
                while (returningQuestion == null)
                    try
                    {
                        // В зависимости от типа вопроса генерируем его
                        if (questionType == QuestionType.yesNo)
                            returningQuestion = new QuestionYesNo(mineral, parametr.ToLower(), language.ToLower());
                        else if (questionType == QuestionType._1from4)
                            returningQuestion = new Question1from4(mineral, parametr.ToLower(), language.ToLower());
                        else
                            throw new MineralException("Wrong question type!");
                    }
                    catch (MineralException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // Если произошла некая ошибка - выберем другой параметр
                        parametr = GetRandomParametr(mineral, previousQuestion);
                    }

                return returningQuestion;
            }
            catch (Exception e)
            {
                throw new MineralException("Question generation failed. Error:" + e.Message);
            }
        }

        private static string GetRandomParametr(Mineral mineral, Question previousQuestion)
        {
            List<string> prohibitedParametr = new List<string>() { "crystalshape" };

            string parametr;
            int randPos = Randint(1, Mineral.parametrsList.Length);
            parametr = Mineral.parametrsList[randPos];
            while (previousQuestion != null && previousQuestion.Mineral == mineral && previousQuestion.Parametr == parametr && !prohibitedParametr.Contains(parametr))
                parametr = Mineral.parametrsList[((randPos +
                    Randint(1, Mineral.parametrsList.Length)) % (Mineral.parametrsList.Length - 1)) + 1];
            //Debug.WriteLine("Param was: " + parametr);
            return parametr;
        }
    }
}
