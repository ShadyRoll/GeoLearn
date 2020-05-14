
using GeoLearn.Database;
using GeoLearn.Models.Minerals;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MineralsPage : ContentPage
    {
        StackLayout layout;
        Frame searchFrame;
        Entry searchEntry;
        List<MineralShow> mineralShows;

        public MineralsPage()
        {
            InitializeComponent();

            layout = new StackLayout { };
            string padding = "5,10,5,10";

            searchFrame = new Frame()
            {
                BackgroundColor = Colorset.BackgroundColor,
                Padding = new Thickness(1, 1, 1, 1)
            };
            searchEntry = new Entry()
            {
                //Margin = new Thickness(15, 0, 15, 0),
                Placeholder = "Найти минерал",
                BackgroundColor = Colorset.BackgroundColor,
                PlaceholderColor = Colorset.SlightlyImportant,
                TextColor = Colorset.Text
            };
            searchEntry.Completed += SearchEntry_Completed;
            searchEntry.TextChanged += SearchEntry_TextChanged;
            searchFrame.Content = searchEntry;
            layout.Children.Add(searchFrame);

            mineralShows = new List<MineralShow>();
            var en = MineralsList.mineralsList["ru"].Keys.GetEnumerator();

            for (int mineralNum = 0; mineralNum < MineralsList.mineralsList["ru"].Count; mineralNum++)
            {
                if (en.Current == null)
                    en.MoveNext();
                mineralShows.Add(new MineralShow(MineralsList.GetByName(en.Current), ref layout, padding));
                en.MoveNext();
            }

            ScrollView scrollView = new ScrollView
            {
                Content = layout,
                BackgroundColor = Colorset.BackgroundColor
            };
            this.Content = scrollView;
        }

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchEntry.Text.Trim() == "")
                SearchEntry_Completed(searchEntry, null);
        }

        private void SearchEntry_Completed(object sender, System.EventArgs e)
        {
            for (int i = 0; i < mineralShows.Count; i++)
            {
                if (!mineralShows[i].currectMineral.Name.ToLower().Trim().StartsWith(
                        searchEntry.Text.ToLower().Trim()))
                    layout.Children.Remove(mineralShows[i].frame);
                else
                    if (!layout.Children.Contains(mineralShows[i].frame))
                    layout.Children.Add(mineralShows[i].frame);
            }
        }
    }
}