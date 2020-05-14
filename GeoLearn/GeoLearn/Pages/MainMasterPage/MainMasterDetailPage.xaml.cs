using GeoLearn.Database;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPage : MasterDetailPage
    {
        public MainMasterDetailPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            //MasterPage.BackgroundColor = Colorset.Color1;
            navigationPage.BarBackgroundColor = Colorset.Bright;
            navigationPage.BarTextColor = Colorset.TextOnColored;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMasterDetailPageMasterMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page)
            {
                BarBackgroundColor = Colorset.Bright,
                BarTextColor = Colorset.TextOnColored
            };
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}