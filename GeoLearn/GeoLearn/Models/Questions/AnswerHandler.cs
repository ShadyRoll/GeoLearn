using GeoLearn.Models;
using GeoLearn.Models.Minerals;
using System;
using System.Diagnostics;
using static GeoLearn.Models.Question;

namespace GeoLearn.Database
{
    static class AnswerHandler
    {
        private static void DisplayPriorities()
        {
            Debug.WriteLine("Priorities:");
            foreach (Mineral mineral in MineralsList.mineralsList["ru"].Values)
            {
                Debug.WriteLine(mineral.Name + ": " + mineral.Priority);
            }
        }

        public static void CorrectAnswer(QestionType qestionType, Mineral mineral, int unhidedParams = 10)
        {
            // Вибрация устройства - маркер верного ответа
            Xamarin.Essentials.Vibration.Vibrate(100);
            // Если выбран алгоритм случайных вопросов - завешаем метод
            if (Randomizer.Mode == Randomizer.EducationAlgorithmMode.RandomQuestions)
                return;
            // В зависимости от типа вопроса уменьшаем приоритет
            switch (qestionType)
            {
                case QestionType._1from4:
                    mineral.Priority -= 2;
                    break;
                case QestionType.MisteryMineral:
                    mineral.Priority -= 3;
                    break;
                case QestionType.Exam:
                    mineral.Priority -= Math.Max(8 - unhidedParams, 3);
                    break;
                case QestionType.Miner:
                    mineral.Priority -= 1;
                    break;
            }
            // Сохраняем данные о текущем приоритете в базу данных
            App.Database.UpdateMineral(mineral);
        }

        public static void WrongAnswer(QestionType qestionType, Mineral mineral, int unhidedParams = 10)
        {
            if (Randomizer.Mode == Randomizer.EducationAlgorithmMode.RandomQuestions)
                return;
            switch (qestionType)
            {
                case QestionType._1from4:
                    mineral.Priority += 2;
                    break;
                case QestionType.MisteryMineral:
                    mineral.Priority += 3;
                    break;
                case QestionType.Exam:
                    mineral.Priority += Math.Min(8 - unhidedParams, 3);
                    break;
                case QestionType.Miner:
                    mineral.Priority += 1;
                    break;
            }
            // Сохраняем данные о текущем приоритете в базу данных
            App.Database.UpdateMineral(mineral);
        }
    }
}
