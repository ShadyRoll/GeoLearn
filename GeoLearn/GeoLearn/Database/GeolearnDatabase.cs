using GeoLearn.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoLearn.Database
{
    public interface ISQLite
    {
        string GetDatabasePath(string filename);
    }

    public class MineralAsyncRepository
    {
        readonly SQLiteConnection database;
        readonly SQLiteCommand cmd;

        public class TableName
        {
            public TableName() { }
            public string Name { get; set; }
        }

        public MineralAsyncRepository(string databasePath)
        {
            database = new SQLiteConnection(databasePath, SQLiteOpenFlags.ReadWrite);

            cmd = database.CreateCommand("");
        }

        public void UpdateMineral(Mineral mineral)
        {
            database.Update(mineral);
        }

        public int GetMineBestScore()
        {
            int ans;
            try
            {
                ans = database.Table<Saves>().FirstOrDefault().BestScore;
            }
            catch (Exception)
            {
                database.CreateTable<Saves>();
                database.Insert(new Saves() { BestScore = 0, FirstLaunch = 1, SuccessPool = 0 });
                ans = database.Table<Saves>().FirstOrDefault().BestScore;
            }
            return ans;
        }

        public bool GetFirstLaunch(bool setFirstLaunchFalse = true)
        {
            Saves save;

            try
            {
                save = database.Table<Saves>().FirstOrDefault<Saves>();
            }
            catch (Exception)
            {
                database.CreateTable<Saves>();
                database.Insert(new Saves() { BestScore = 0, FirstLaunch = 1, SuccessPool = 0 });
                save = database.Table<Saves>().FirstOrDefault();
            }


            if (save.FirstLaunch == 1)
            {
                if (setFirstLaunchFalse)
                {
                    save.FirstLaunch = 0;
                    database.Update(save);
                }
                return true;
            }
            return false;
        }

        public void SetMineBestScore(int val)
        {
            Saves saves = database.Table<Saves>().FirstOrDefault<Saves>();
            saves.BestScore = val;
            database.Update(saves);

            var chek = database.Get<Saves>(1);
            System.Diagnostics.Debug.WriteLine($"saved score: {chek.BestScore}");
        }

        public Dictionary<string, Mineral> GetMineralsList(string language)
        {
            Dictionary<string, Mineral> minerals = new Dictionary<string, Mineral>();

            cmd.CommandText = "SELECT * FROM Minerals";

            foreach (Mineral min in cmd.ExecuteQuery<Mineral>())
            {
                if (min.KeyName != null)
                    minerals[min.KeyName] = min;
            }
            return minerals;
        }
    }
}

