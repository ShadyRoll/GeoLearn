using GeoLearn.Database;
using GeoLearn.Extensions;
using GeoLearn.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLearn.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionsPage : ContentPage
    {
        readonly AbsoluteLayout layout;
        Label headerLabel;
        Label questionLabel;
        Question1from4 question;
        RadioButton[] radioButton;
        Button skipButton;
        readonly StackLayout stackLayout;

        public QuestionsPage()
        {
            try
            {
                InitializeComponent();

                // Устанавливаем layout страницы
                layout = Content as AbsoluteLayout;
                layout.BackgroundColor = Colorset.BackgroundColor;

                // Создаем вспомогательный frame для объектов окна
                stackLayout = new StackLayout();
                Frame stackFrame = new Frame()
                {
                    BackgroundColor = Color.Transparent,
                    HasShadow = false,
                    Content = stackLayout
                };
                AbsoluteLayout.SetLayoutFlags(stackFrame, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(stackFrame, new Rectangle(.5, .5, 1, .85));
                layout.Children.Add(stackFrame);

                // Инициализруем компоненты окна
                InitHeaderLabel();
                InitQuestionLabel();
                InitRadioButtons();
                InitSkipButton();

                // Симулируем нажатие кнопки пропуска для инициализации вопроса
                SkipButton_Clicked(skipButton, null);
            }
            catch (Exception ex)
            {
                // Выводим ошибку на экран
                DisplayAlert("Ошибка", "Ошибка загрузки страницы :" + ex.Message, "отмена");
            }
        }

        private void InitQuestionLabel()
        {
            questionLabel = new Label()
            {
                Text = "Текст вопроса",
                TextColor = Colorset.Text,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colorset.BackgroundColor,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 0, 0, 10)
            };
            stackLayout.Children.Add(questionLabel);
            //AbsoluteLayout.SetLayoutFlags(questionLabel, AbsoluteLayoutFlags.All);
            //AbsoluteLayout.SetLayoutBounds(questionLabel, new Rectangle(.5, .1, .9, .25));
            //layout.Children.Add(questionLabel);
        }

        private void InitSkipButton()
        {
            skipButton = new Button()
            {
                Text = "Следующий вопрос",
                TextColor = Colorset.TextOnColored,
                BackgroundColor = Colorset.NonImportant,//Colorset.SubBright,
                FontSize = 14,
                Margin = new Thickness(80, 20, 80, 0)
            };
            skipButton.Clicked += SkipButton_Clicked;
            stackLayout.Children.Add(skipButton);
            //AbsoluteLayout.SetLayoutFlags(skipButton, AbsoluteLayoutFlags.All);
            //AbsoluteLayout.SetLayoutBounds(skipButton, new Rectangle(0.5, .9, 0.5, .06));
            //layout.Children.Add(skipButton);
        }

        private void InitRadioButtons()
        {
            radioButton = new RadioButton[4];
            for (int i = 0; i < radioButton.Length; i++)
            {
                radioButton[i] = new RadioButton()
                {
                    AutomationId = i.ToString(),
                    Text = "ответ 1",
                    BackgroundColor = Colorset.BackgroundColor,
                    TextColor = Colorset.Text,
                    VerticalOptions = LayoutOptions.Center
                    ,
                    FontSize = 18
                };
                radioButton[i].Clicked += QuestionsPage_Clicked;
                stackLayout.Children.Add(radioButton[i]);
                //AbsoluteLayout.SetLayoutFlags(radioButton[i], AbsoluteLayoutFlags.All);
                //AbsoluteLayout.SetLayoutBounds(radioButton[i], new Rectangle(0.5, .398 + i * 0.13, 0.97, .13));
                //layout.Children.Add(radioButton[i]);
            }
        }

        private void QuestionsPage_Clicked(object sender, EventArgs e)
        {
            if (question.Answered)
                return;
            question.Answered = true;

            RadioButton button = (RadioButton)sender;
            int id = int.Parse(button.AutomationId);

            if (id != question.CorrectAnswer)
            {
                button.TextColor = Color.Red;
                AnswerHandler.WrongAnswer(Question.QestionType._1from4, question.Mineral);
            }
            else
            {
                AnswerHandler.CorrectAnswer(Question.QestionType._1from4, question.Mineral);
            }

            radioButton[question.CorrectAnswer].TextColor = Color.Green;
            for (int i = 0; i < radioButton.Length; i++)
            {
                radioButton[i].InputTransparent = true;
            }
        }

        private void InitHeaderLabel()
        {
            headerLabel = new Label()
            {
                Text = "Выбери верный ответ",
                BackgroundColor = Colorset.SubBright,
                TextColor = Colorset.TextOnColored,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            AbsoluteLayout.SetLayoutFlags(headerLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(headerLabel, new Rectangle(0.5, 0, 1, 0.04));
            layout.Children.Add(headerLabel);
        }

        private void InitQuestion(Question1from4 question = null)
        {
            if (question == null)
                question = this.question;

            questionLabel.Text = question.Text;

            for (int buttonNum = 0; buttonNum < radioButton.Length; buttonNum++)
            {
                radioButton[buttonNum].IsChecked = false;
                radioButton[buttonNum].InputTransparent = false;
                radioButton[buttonNum].TextColor = Colorset.Text;
                radioButton[buttonNum].Text = question.Answers[buttonNum].UpperFirstLetter();
            }
        }

        private void SkipButton_Clicked(object sender, EventArgs e)
        {
            question = (Question1from4)Randomizer.RandomQuestion(
                questionType: Randomizer.QuestionType._1from4, previousQuestion: question);
            InitQuestion();
        }
    }
}