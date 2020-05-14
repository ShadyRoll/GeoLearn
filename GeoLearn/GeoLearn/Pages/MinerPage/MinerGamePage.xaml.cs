using GeoLearn.Database;
using GeoLearn.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeoLearn.Pages
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinerGamePage : ContentPage
    {
        static RelativeLayout relLayout;
        static RelativeLayout relForFramesLayout;
        static Frame mineFrame, underMineFrame, forFramesFrame;
        static Grid underMineFrameLayout;
        static RelativeLayout frameLayout;
        static Grid buttonsGrid;
        static MinerGamePage page;
        static QuestionYesNo question;
        static Label questionLabel;
        static Label headerLabel, bestScoreLabel, timerLabel;
        static Button[] questionButton;
        static Button startButton;
        static BoxView colorBox;
        static Mine mine;

        public MinerGamePage()
        {
            page = this;
            InitializeComponent();
            InitToolBar();

            try
            {
                relLayout = new RelativeLayout()
                {
                    BackgroundColor = Colorset.BackgroundColor
                };
                this.Content = relLayout;

                InitHeaderLabel();

                relForFramesLayout = new RelativeLayout()
                {
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                forFramesFrame = new Frame()
                {
                    Content = relForFramesLayout,
                    CornerRadius = 10,
                    IsClippedToBounds = true,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                relLayout.Children.Add(forFramesFrame,
                Constraint.RelativeToParent((parent) =>
                    (parent.X + parent.Width * 0.04)),
                Constraint.RelativeToView(headerLabel, (parent, sibling) =>
                    sibling.Y + sibling.Height * 1.5),
                Constraint.RelativeToParent((parent) =>
                    (parent.Width - parent.Width * 0.04 * 2)),
                Constraint.RelativeToParent((parent) =>
                    (parent.Height * 0.46))
                );

                underMineFrameLayout = new Grid()
                {
                    RowSpacing = 0,
                    ColumnSpacing = 0,
                    Padding = new Thickness(0, 0, 0, 0),
                    BackgroundColor = Colorset.BackgroundColor,
                };
                underMineFrameLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                underMineFrameLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                underMineFrameLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                underMineFrame = new Frame
                {
                    BackgroundColor = Colorset.BackgroundColor,
                    Content = underMineFrameLayout,
                    Padding = new Thickness(0, 0, 0, 0),
                    Margin = new Thickness(0, 0, 0, 0),
                    //HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    WidthRequest = forFramesFrame.Width,
                    CornerRadius = 0,
                    IsClippedToBounds = true,
                    HasShadow = false
                };
                timerLabel = new Label()
                {
                    Text = "00:" + Mine.timerDuration,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    BackgroundColor = Colorset.BackgroundColor,
                    TextColor = Colorset.Text,
                    Padding = new Thickness(20, 5, 35, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };
                underMineFrameLayout.Children.Add(timerLabel, 0, 0);
                bestScoreLabel = new Label()
                {
                    Text = "лучший результат: 0%",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colorset.BackgroundColor,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colorset.SlightlyImportant,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(0, 5, 20, 0),
                    Margin = new Thickness(0, 0, 0, 0)
                };

                underMineFrameLayout.Children.Add(bestScoreLabel, 1, 0);

                InitMine();
                mine.UpdateScoreLabel();

                relForFramesLayout.Children.Add(underMineFrame,
                Constraint.RelativeToParent((parent) =>
                    parent.X),
                Constraint.RelativeToView(mineFrame, (parent, sibling) =>
                    sibling.Y + mine.Blocks[0].Height * 10),
                Constraint.RelativeToView(mineFrame, (parent, sibling) =>
                    sibling.Width),
                Constraint.RelativeToView(mineFrame, (parent, sibling) =>
                    (parent.Height - mine.Blocks[0].Height * 10) * 1.05)
                );

                InitQustionLabel();

                InitButtons();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message + " " + ex.Source, "принять этот факт");
            }
        }

        private static void InitMine()
        {
            frameLayout = new RelativeLayout();
            mineFrame = new Frame
            {
                BackgroundColor = Colorset.BackgroundColor,
                Content = frameLayout,
                CornerRadius = 0,
                HasShadow = false,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = Application.Current.MainPage.Height * 0.4
            };

            relForFramesLayout.Children.Add(mineFrame,
                Constraint.RelativeToParent((parent) =>
                    parent.X),
                Constraint.RelativeToParent((parent) =>
                    parent.Y)
                );

            mine = new Mine(frameLayout, App.Database.GetMineBestScore(), timerLabel, bestScoreLabel);
        }

        private static void InitQustionLabel()
        {
            questionLabel = new Label()
            {
                Text = "Нажми \"старт\", чтобы начать бурить шахту",
                TextColor = Colorset.Text,
                BackgroundColor = Colorset.BackgroundColor,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            colorBox = new BoxView()
            {
                BackgroundColor = Colorset.SubBright
            };

            relLayout.Children.Add(colorBox,
                Constraint.RelativeToParent((parent) =>
                   parent.X + 5 - 5),
            Constraint.RelativeToView(forFramesFrame, (parent, sibling) =>
                   sibling.Y + sibling.Height * 1.05),
            Constraint.RelativeToParent((parent) =>
                   parent.Width - 10 + 10),
            Constraint.RelativeToParent((parent) =>
                   parent.Height * 0.2 + 10)
            );

            relLayout.Children.Add(questionLabel,
            Constraint.RelativeToView(colorBox, (parent, sibling) =>
                   sibling.X + 5),
            Constraint.RelativeToView(colorBox, (parent, sibling) =>
                   sibling.Y + 5),
            Constraint.RelativeToView(colorBox, (parent, sibling) =>
                   sibling.Width - 10),
            Constraint.RelativeToView(colorBox, (parent, sibling) =>
                   sibling.Height - 10)
            );
        }

        private void InitHeaderLabel()
        {
            headerLabel = new Label()
            {
                Text = "Бури шахту, отвечая на вопросы",
                BackgroundColor = Colorset.SubBright,
                TextColor = Colorset.TextOnColored,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            relLayout.Children.Add(headerLabel,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => parent.Width),
                Constraint.RelativeToParent((parent) => parent.Height * 0.04));

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
                DisplayAlert("Помощь", "Нажми \"старт\", чтобы начать бурить шахту! Отвечай правильно на вопросы, пробиваясь к центру планеты! Но следи за временем. За каждый неправильный ответ ты получаешь штраф.", @"К игре");
            };
            ToolbarItems.Add(tbXamLogo);
        }

        public static void RestartQuestionLabel(string text = "Нажми \"старт\", чтобы начать бурить шахту")
                    => questionLabel.Text = text;

        private static void StartButton_Clicked(object sender, EventArgs e)
        {
            mine.Reboot(false);
            if (!mine.TimerAlive)
                InitNewQuestion(question);
            mine.StartTimer();
        }

        public static void UpdateStartButtonText(bool inGame)
        {
            if (inGame)
                startButton.Text = "Стоп";
            else
                startButton.Text = "Старт";
        }

        private static void InitNewQuestion(QuestionYesNo prevQuestion = null)
        {
            question = (QuestionYesNo)Randomizer.RandomQuestion(questionType: Randomizer.QuestionType.yesNo, previousQuestion: prevQuestion);

            questionLabel.Text = question.Text;

            foreach (Button button in questionButton)
                button.BackgroundColor = startButton.BackgroundColor;
        }

        private static void InitButtons()
        {
            questionButton = new Button[2];

            buttonsGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };
            buttonsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            buttonsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            buttonsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });//Auto });
            buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });//Auto });

            for (int i = 0; i < 2; i++)
            {
                questionButton[i] = new TintableButton
                {
                    Text = (i == 0) ? "Да" : "Нет",
                    AutomationId = i.ToString(),
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Colorset.TextOnColored,
                    TintColor = Colorset.Bright,
                    WidthRequest = Application.Current.MainPage.Width * 0.4,
                    HeightRequest = Application.Current.MainPage.Height * 0.08,
                };
                questionButton[i].Clicked += QuestionButton_Clicked;
                buttonsGrid.Children.Add(questionButton[i], i, 0);
            }

            startButton = new TintableButton()
            {
                Text = "Старт",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colorset.TextOnColored,
                TintColor = Colorset.SlightlyImportant,
                WidthRequest = Application.Current.MainPage.Width * 0.4,
                HeightRequest = Application.Current.MainPage.Height * 0.08,
            };
            startButton.Clicked += StartButton_Clicked;
            buttonsGrid.Children.Add(startButton, 0, 2, 1, 2);


            relLayout.Children.Add(buttonsGrid,
               Constraint.RelativeToView(questionLabel, (parent, sibling) =>
               {
                   return 0;
               }),
                Constraint.RelativeToView(questionLabel, (parent, sibling) =>
                    sibling.Y + sibling.Height * 1.1),
                Constraint.RelativeToParent((parent) =>
                    parent.Width)
                );



            /*
            relLayout.Children.Add(startButton,
               Constraint.RelativeToView(questionLabel, (parent, sibling) =>
                    sibling.X + 0.5 * (sibling.Width - startButton.Width)),
                Constraint.RelativeToView(buttonsGrid, (parent, sibling) =>
                    sibling.Y + sibling.Height * 1.15),
                Constraint.RelativeToView(questionLabel, (parent, sibling) =>
                    (sibling.Width / 3)),
                Constraint.RelativeToView(questionLabel, (parent, sibling) =>
                    sibling.Height * 0.5)
                );
            */

        }

        private static void QuestionButton_Clicked(object sender, EventArgs e)
        {
            if (question != null && mine.TimerAlive)
            {
                Button button = (Button)sender;
                if (button.AutomationId == question.CorrectAnswer.ToString())
                {
                    colorBox.BackgroundColor = Color.Green;
                    if (mine.AddProgress())
                        page.DisplayAlert("Успех", @"Поздравляю! Ты достиг ядра планеты! Можешь гордиться собой.", @"К игре");
                    AnswerHandler.CorrectAnswer(Question.QestionType.Miner, question.Mineral);
                }
                else
                {
                    mine.DecProgress();
                    colorBox.BackgroundColor = Color.Red;
                    AnswerHandler.WrongAnswer(Question.QestionType.Miner, question.Mineral);
                }

                InitNewQuestion(question);
                Task task = new Task(() =>
                {
                    Task.Delay(300).Wait();
                    colorBox.BackgroundColor = Colorset.SubBright;
                });
                task.Start();
                //await Task.Delay(300);
                //colorBox.BackgroundColor = Colorset.SubBright;
            }
        }
    }
}