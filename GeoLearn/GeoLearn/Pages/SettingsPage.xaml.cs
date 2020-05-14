using GeoLearn.Database;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        readonly Grid settingsGrid;
        readonly Frame frame;
        RadioButton[] radioButton;
        StackLayout languageStackLayout, modeStackLayout;

        public SettingsPage()
        {
            InitializeComponent();

            settingsGrid = new Grid();
            settingsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = Application.Current.MainPage.Width });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            settingsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            InitLenguagePicker();
            InitModePicker();

            frame = new Frame()
            {
                Padding = new Thickness(10, 20, 10, 20),
                Margin = new Thickness(0, 0, 0, 0),
                Content = settingsGrid,
                BackgroundColor = Colorset.BackgroundColor,
                HasShadow = false
            };

            Content = settingsGrid;
        }

        private void InitLenguagePicker()
        {
            Label lenguageLabel = new Label
            {
                Text = "Язык",
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                TextColor = Colorset.Text,
                FontSize = 18,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start
            };
            //settingsGrid.Children.Add(lenguageLabel, 0, 0);

            languageStackLayout = new StackLayout();
            languageStackLayout.Children.Add(lenguageLabel);
            InitRadioButtons(languageStackLayout, SettingTypes.Lenguage);

            Frame lenguageFrame = new Frame()
            {
                Content = languageStackLayout,
                BackgroundColor = Colorset.BackgroundColor
            };

            settingsGrid.Children.Add(lenguageFrame, 0, 0);
        }

        private void InitModePicker()
        {

            //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Label modeLabel = new Label
            {
                Text = "Учебный алгоритм",
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                TextColor = Colorset.Text,
                FontSize = 18,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start
            };
            //settingsGrid.Children.Add(modeLabel, 0, 2);

            modeStackLayout = new StackLayout();
            modeStackLayout.Children.Add(modeLabel);
            InitRadioButtons(modeStackLayout, SettingTypes.EducationMode);

            Frame modeFrame = new Frame()
            {
                Content = modeStackLayout,
                BackgroundColor = Colorset.BackgroundColor
            };

            settingsGrid.Children.Add(modeFrame, 0, 1);
        }

        private enum SettingTypes
        {
            Lenguage,
            EducationMode
        }

        private void InitRadioButtons(StackLayout stackLayout, SettingTypes type)
        {
            radioButton = new RadioButton[2];
            for (int i = 0; i < radioButton.Length; i++)
            {
                radioButton[i] = new RadioButton()
                {
                    BackgroundColor = Colorset.BackgroundColor,
                    TextColor = Colorset.Text,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 18
                };
                radioButton[i].Clicked += RadioButton_Clicked;
                stackLayout.Children.Add(radioButton[i]);
            }

            if (type == SettingTypes.Lenguage)
            {
                radioButton[0].Text = "Русский";
                radioButton[0].AutomationId = "ru";
                radioButton[1].Text = "English";
                radioButton[1].AutomationId = "eng";
                radioButton[0].IsChecked = true;
            }
            else if (type == SettingTypes.EducationMode)
            {
                radioButton[0].Text = "GeoLearn Adaptive Beta";
                radioButton[0].AutomationId = "GeoLearnAdaptive";
                radioButton[1].Text = "Случайные вопросы";
                radioButton[1].AutomationId = "Random";
                radioButton[0].IsChecked = true;
            }

        }

        private void RadioButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}