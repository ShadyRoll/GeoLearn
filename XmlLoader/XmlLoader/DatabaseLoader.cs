using System;
using GeoLearn.Database;
using GeoLearn.Models;
using GeoLearn.Models.Minerals;
using SQLite;

namespace Version
{
    class DataBaseLoader
    {
        static SQLiteCommand cmd;

        private static void Command(string command)
        {
            cmd.CommandText = command;
            cmd.ExecuteNonQuery();
        }

        static void Main()
        {
            MineralsList.LoadBase();

            var con = new SQLiteConnection(@"C:\Users\Shado\Desktop\GeoLearn\GeoLearn\GeoLearn\Database\GeoLearnDataBase.db");
            cmd = con.CreateCommand("");

            con.DropTable<Mineral>();
            con.DropTable<Saves>();

            con.CreateTable<Saves>();
            con.CreateTable<Mineral>();


            InsertMinerals("ru");

            foreach (var itemi in con.Table<Mineral>().ToList())
            {
                Console.WriteLine($"Name: {itemi.Name}, KeyName:{itemi.KeyName}, Luster:{itemi.Luster}");
            }

            Saves save = new Saves
            {
                BestScore = 0,
                FirstLaunch = 1,
                SuccessPool = 0
            };
            con.Insert(save);

            //Command("INSERT INTO Saves (BestMineScore, FirstLaunch) VALUES(0, 1)");

            //cmd.CommandText = "SELECT BestMineScore FROM Saves";
            //Console.WriteLine($"Best mine score: {cmd.ExecuteQuery<int>()[0]}");

            Console.WriteLine($"Best mine score: {con.Table<Saves>().FirstOrDefault().BestScore}");

            con.Close();
            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        private static void InsertMinerals(string language)
        {
            string comm;
            string startComm = $"INSERT INTO Minerals (KeyName, Priority";
            foreach (string param in Mineral.parametrsList)
                startComm += $", {param}";
            startComm += ")";

            foreach (var mineral in MineralsList.mineralsList[language])
            {
                comm = startComm + $"VALUES('{mineral.Key}', 0";
                foreach (string param in Mineral.parametrsList)
                    comm += $",'{mineral.Value[param]}'";
                Command(comm + ")");
            }
        }
    }
}