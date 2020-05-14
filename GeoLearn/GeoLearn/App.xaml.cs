using GeoLearn.Database;
using GeoLearn.Models;
using GeoLearn.Pages;
using System;
using System.IO;
using Xamarin.Forms;

namespace GeoLearn
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "Database.GeoLearnDataBase.db";
        private static MineralAsyncRepository database;
        public static MineralAsyncRepository Database
        {
            get
            {
                if (database == null)
                {
                    // путь, по которому будет находиться база данных
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DATABASE_NAME);
                    database = new MineralAsyncRepository(dbPath);
                }
                //System.Diagnostics.Debug.WriteLine("MineralsLen: " + database.GetItemsAsync().Result.Count);
                return database;
            }
        }


        public App()
        {
            InitializeComponent();

            bool firstLaunch = Database.GetFirstLaunch();
            if (firstLaunch)
                Randomizer.Mode = Randomizer.EducationAlgorithmMode.GeoLearnAdaptive;
            Models.Minerals.MineralsList.LoadBaseSql(firstLaunch);

            MainPage = new MainMasterDetailPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
