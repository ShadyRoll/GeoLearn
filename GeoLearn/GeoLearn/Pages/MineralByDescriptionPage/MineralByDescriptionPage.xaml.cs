using GeoLearn.Database;
using GeoLearn.Extensions;
using GeoLearn.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MineralByDescriptionPage : ContentPage
    {
        readonly ScrollView scrollLayout;
        readonly StackLayout stackLayout;
        readonly RelativeLayout relLayout;
        Mineral answerMineral;
        TintableButton answerButton, nextQuestionButton;
        Entry answerEntry;
        readonly MineralShow mineralShow;
        Label headerLabel;

        public MineralByDescriptionPage()
        {
            InitializeComponent();

            // Генерируем вопрос
            answerMineral = Randomizer.RandomMineral(randomly: false);

            // Инициализируем контейнеры объектов
            stackLayout = new StackLayout();
            relLayout = new RelativeLayout();

            // Создаем вспомогательный frame для объектов
            Frame stackFrame = new Frame()
            {
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = stackLayout
            };
            relLayout.Children.Add(stackFrame,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => (parent.Height * (.03))));

            // Добавляем скроллинг для окна
            Content = new ScrollView()
            {
                Content = relLayout
            };
            scrollLayout = this.Content as ScrollView;
            scrollLayout.BackgroundColor = Colorset.BackgroundColor;


            // Инициализируем элементы окна
            InitHeaderLabel();
            mineralShow = new MineralShow(answerMineral, ref stackLayout, "10,0,10,0",
                dontShowParametrs: new string[1] { "name" });
            SetMineralShowFrame();
            InitEntry();
            InitButtons();
        }

        private void SetMineralShowFrame()
        {
            mineralShow.frame.Padding = new Thickness(0, 0, 0, 0);
            mineralShow.frame.Margin = new Thickness(0, 0, 0, 0);
            mineralShow.frame.BorderColor = Color.Transparent;
            mineralShow.frame.HasShadow = false;
        }

        private void InitEntry()
        {
            answerEntry = new Entry
            {
                Placeholder = "Введи название минерала",
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colorset.Text,
                PlaceholderColor = Colorset.Text
            };
            answerEntry.Completed += AnswerButton_Clicked;
            stackLayout.Children.Add(answerEntry);
        }

        private void InitButtons()
        {
            Grid buttonsGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 0)//40)
            };
            buttonsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });//Auto });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });//Auto });

            answerButton = new TintableButton
            {
                Text = "Проверить\nответ",
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colorset.TextOnColored,
                TintColor = Colorset.Bright,
                WidthRequest = Application.Current.MainPage.Width * 0.4,
                HeightRequest = Application.Current.MainPage.Height * 0.08,
            };
            answerButton.Clicked += AnswerButton_Clicked;
            buttonsGrid.Children.Add(answerButton, 0, 0);

            nextQuestionButton = new TintableButton
            {
                Text = "Следующий вопрос",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colorset.Text,
                TintColor = Colorset.NonImportant,
                WidthRequest = Application.Current.MainPage.Width * 0.4,
                HeightRequest = Application.Current.MainPage.Height * 0.08,
            };
            nextQuestionButton.Clicked += NextQuestionButton_Clicked;
            buttonsGrid.Children.Add(nextQuestionButton, 1, 0);

            stackLayout.Children.Add(buttonsGrid);
        }

        private void InitHeaderLabel()
        {
            headerLabel = new Label()
            {
                Text = "Определи минерал",
                BackgroundColor = Colorset.SubBright,
                TextColor = Colorset.TextOnColored,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                //HeightRequest = Application.Current.MainPage.Height * 0.06,
                //Padding = new Thickness(0, 0, 0, (0.5 - 0.425 - 0.06) * Application.Current.MainPage.Height)
            };
            //AbsoluteLayout.SetLayoutFlags(headerLabel, AbsoluteLayoutFlags.All);
            //AbsoluteLayout.SetLayoutBounds(headerLabel, new Rectangle(0.5, 0, 1, 0.07));
            relLayout.Children.Add(headerLabel,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => parent.Width),
                Constraint.RelativeToParent((parent) => parent.Height * 0.04));
            //stackLayout.Children.Add(headerLabel);
        }

        private void NextQuestionButton_Clicked(object sender, EventArgs e)
        {
            answerEntry.Text = "";
            answerEntry.TextColor = Colorset.Text;
            answerEntry.InputTransparent = false;

            answerMineral = Randomizer.RandomMineral(previousMineral: answerMineral, randomly: false);
            mineralShow.ChangeMineral(answerMineral);
        }

        private void AnswerButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(answerEntry.Text) && !answerEntry.InputTransparent)
            {
                answerEntry.InputTransparent = true;
                if (answerEntry.Text.Trim().ToLower() == answerMineral.Name.ToLower())
                {
                    answerEntry.TextColor = Color.Green;
                    AnswerHandler.CorrectAnswer(Question.QestionType.MisteryMineral, answerMineral);
                }
                else
                {
                    answerEntry.TextColor = Color.Red;
                    answerEntry.Text = answerMineral.Name.UpperFirstLetter();
                    AnswerHandler.WrongAnswer(Question.QestionType.MisteryMineral, answerMineral);
                }
            }
        }
    }
}