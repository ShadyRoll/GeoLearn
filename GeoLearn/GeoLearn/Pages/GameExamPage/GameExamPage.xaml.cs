using GeoLearn.Database;
using GeoLearn.Extensions;
using GeoLearn.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameExamPage : ContentPage
    {
        RelativeLayout relLayout;
        ScrollView scrollLayout;
        StackLayout stackLayout;
        Mineral curMineral;
        Entry answerEntry;
        Button answerButton, nextQuestionButton;
        Label headerLabel;
        Grid buttonsGrid;
        Label[] paramValLabels;
        Label[] paramNameLabels;
        int unhidedParams = 0;

        public GameExamPage()
        {
            InitializeComponent();
            InitToolBar();

            stackLayout = new StackLayout();
            relLayout = new RelativeLayout();

            Frame stackFrame = new Frame()
            {
                HasShadow = false,
                BackgroundColor = Color.Transparent,
                Content = stackLayout
            };
            //AbsoluteLayout.SetLayoutFlags(stackFrame, AbsoluteLayoutFlags.All);
            //AbsoluteLayout.SetLayoutBounds(stackFrame, new Rectangle(.5, .8, 1, 1.05));
            relLayout.Children.Add(stackFrame,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => (parent.Height * (.03))));

            this.Content = new ScrollView()
            {
                Content = relLayout
            };
            scrollLayout = this.Content as ScrollView;
            scrollLayout.BackgroundColor = Colorset.BackgroundColor;

            InitHeaderLabel();

            curMineral = Randomizer.RandomMineral(curMineral, randomly: false);

            InitPageItems();

        }

        private void InitHeaderLabel()
        {
            headerLabel = new Label()
            {
                Text = "Исследуй и определи минерал",
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

        private void InitPageItems()
        {
            Grid grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },//new GridLength(1.5, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Star }//new GridLength(2, GridUnitType.Star) }
                }
            };

            paramNameLabels = new Label[Mineral.examParametrsList.Length];
            paramValLabels = new Label[Mineral.examParametrsList.Length];

            for (int i = 0; i < Mineral.examParametrsList.Length; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                paramNameLabels[i] = new Label()
                {
                    TextColor = Colorset.SlightlyImportant,
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Start,
                    //HorizontalTextAlignment= TextAlignment.Start
                };
                if (Mineral.parametrsListRU[Mineral.examParametrsList[i]].Length > 16)
                    paramNameLabels[i].Text = Mineral.parametrsListRU[Mineral.examParametrsList[i]].Replace(" ", "\n");
                else
                    paramNameLabels[i].Text = Mineral.parametrsListRU[Mineral.examParametrsList[i]];

                paramValLabels[i] = new Label()
                {
                    AutomationId = i.ToString(),
                    Text = "показать",
                    FontSize = 16,
                    TextColor = Colorset.Text,
                    BackgroundColor = Color.Transparent,
                    //Padding = new Thickness(0,0,0,0),
                    Margin = new Thickness(0, 0, 0, 5),
                    HorizontalTextAlignment = TextAlignment.End,
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.End
                };

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += ParamLabel_Clicked;
                paramValLabels[i].GestureRecognizers.Add(tapGestureRecognizer);

                grid.Children.Add(paramNameLabels[i], 0, i);
                grid.Children.Add(paramValLabels[i], 1, i);
            }

            stackLayout.Children.Add(grid);
            InitEntry();
            InitButtons();
        }

        private void ParamLabel_Clicked(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            int id = int.Parse((label).AutomationId);

            if (label.Text != "показать")
                unhidedParams++;
            label.Text = curMineral[Mineral.parametrsList[Mineral.examParametrsList[id]]];

        }

        private void InitButtons()
        {
            buttonsGrid = new Grid()
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
                PlaceholderColor = Colorset.Text,
                Margin = new Thickness(0, 10, 0, 0)
            };
            answerEntry.Completed += AnswerButton_Clicked;
            stackLayout.Children.Add(answerEntry);
        }

        private void InitToolBar()
        {

            ToolbarItem tbXamLogo = new ToolbarItem
            {
                IconImageSource = "HelpIcon.png",
                Order = ToolbarItemOrder.Primary,
                Priority = 1
            };
            tbXamLogo.Clicked += (e, s) =>
            {
                DisplayAlert("Помощь", "Исследуй свойства минерала, нажимая по кнопкам \"показать\", а потом введи правильный ответ в поле снизу. Чем меньше свойств минерала ты подсмотрел, тем лучше твой результат!", @"К игре");
            };
            ToolbarItems.Add(tbXamLogo);
        }

        private void NextQuestionButton_Clicked(object sender, EventArgs e)
        {
            answerEntry.Text = "";
            answerEntry.TextColor = Colorset.Text;
            answerEntry.InputTransparent = false;
            unhidedParams = 0;

            curMineral = Randomizer.RandomMineral(previousMineral: curMineral, randomly: false);

            foreach (Label label in paramValLabels)
                label.Text = "показать";
        }

        private void AnswerButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(answerEntry.Text) && !answerEntry.InputTransparent)
            {
                if (answerEntry.Text.Trim().ToLower() == curMineral.Name.ToLower())
                {
                    answerEntry.TextColor = Color.Green;
                    AnswerHandler.CorrectAnswer(Question.QestionType.Exam, curMineral, unhidedParams);
                }
                else
                {
                    answerEntry.TextColor = Color.Red;
                    answerEntry.Text = curMineral.Name.UpperFirstLetter();
                    AnswerHandler.WrongAnswer(Question.QestionType.Exam, curMineral, unhidedParams);
                }
                answerEntry.InputTransparent = true;

            }
        }
    }
}
