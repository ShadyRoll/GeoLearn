using GeoLearn.Database;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MainMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MainMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;

            Grid header = (Grid)ListView.Header;
            header.BackgroundColor = Colorset.Bright;
            Label headerLabel = new Label()// ((Label)header.Children[0]);
            {
                BackgroundColor = header.BackgroundColor,
                TextColor = Colorset.TextOnColored,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 35,
                Padding = new Thickness(15, 5, 0, 0),
                Text = "Меню"
            };
            header.Children.Add(headerLabel, 0, 0);

            ListView.BackgroundColor = Colorset.BackgroundColor;
        }

        class MainMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainMasterDetailPageMasterMenuItem> MenuItems { get; set; }

            public MainMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMasterDetailPageMasterMenuItem>(new[]
                {
                    new MainMasterDetailPageMasterMenuItem { Id = 0, Title = "Справочник", TargetType = typeof(MineralsPage) },
                    new MainMasterDetailPageMasterMenuItem { Id = 1, Title = "Игра: 1 из 4", TargetType = typeof(QuestionsPage) },
                    new MainMasterDetailPageMasterMenuItem { Id = 2, Title = "Игра: загадочный минерал", TargetType = typeof(MineralByDescriptionPage) },
                    new MainMasterDetailPageMasterMenuItem { Id = 3, Title = "Игра: экзамен", TargetType = typeof(GameExamPage) },
                    new MainMasterDetailPageMasterMenuItem { Id = 3, Title = "Игра: шахтер", TargetType = typeof(MinerGamePage) },
                    //new MainMasterDetailPageMasterMenuItem { Id = 3, Title = "Статистика", TargetType = typeof(UnderConstructionPage) },
                    new MainMasterDetailPageMasterMenuItem { Id = 3, Title = "Настройки", TargetType = typeof(SettingsPage) }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}