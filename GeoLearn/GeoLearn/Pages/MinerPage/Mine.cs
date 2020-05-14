using Xamarin.Forms;

namespace GeoLearn.Pages
{
    class Mine
    {
        public int BestScore { get; private set; }
        Image backgroundImage;
        private readonly Label scoreLabel;
        private readonly Label timerLabel;
        int progress;

        public Image[] Blocks { get; }
        public bool AddProgress()
        {
            progress += 1;
            if (progress % 2 == 0)
                Blocks[progress / 2 - 1].IsVisible = false;
            if (progress == 20)
                return true;
            return false;
        }

        public void DecProgress()
        {
            for (int i = 0; i < 2; i++)
                if (progress > 0)
                {
                    if (progress % 2 == 0)
                        Blocks[progress / 2 - 1].IsVisible = true;
                    progress -= 1;
                }
        }

        private Image[] InitMine(RelativeLayout layout)
        {
            progress = 0;

            Image[] blocks = new Image[10];

            blocks[0] = new Image()
            {
                Source = "drawable/earth_top_block.png"
            };
            layout.Children.Add(blocks[0],
                Constraint.RelativeToParent((parent) =>
                    (parent.X + parent.Width / 3)),
                Constraint.RelativeToView(backgroundImage, (parent, sibling) =>
                    (sibling.Y)),
                Constraint.RelativeToView(backgroundImage, (parent, sibling) =>
                    (sibling.Width / 3)),
                Constraint.RelativeToView(backgroundImage, (parent, sibling) =>
                    (sibling.Height / 10))
                );


            for (int blockNum = 1; blockNum < blocks.Length; blockNum++)
            {
                blocks[blockNum] = new Image()
                {
                    Source = "drawable/earth_block.png"
                };
                layout.Children.Add(blocks[blockNum],
                    Constraint.RelativeToView(blocks[blockNum - 1], (parent, sibling) =>
                        (sibling.X)),
                    Constraint.RelativeToView(blocks[blockNum - 1], (parent, sibling) =>
                        (sibling.Y + sibling.Height)),
                    Constraint.RelativeToView(blocks[blockNum - 1], (parent, sibling) =>
                        (sibling.Width)),
                    Constraint.RelativeToView(blocks[blockNum - 1], (parent, sibling) =>
                        (sibling.Height))
                    );
            }
            return blocks;
        }

        private void InitBackgroundImage(RelativeLayout layout)
        {
            backgroundImage = new Image()
            {
                Source = "drawable/earth_background.png",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 0, 0, 0)
            };

            layout.Children.Add(backgroundImage,
                Constraint.RelativeToParent((parent) =>
                    (parent.X)),
                Constraint.RelativeToParent((parent) =>
                    parent.Y),//(sibling.Height * 1.1)))
                Constraint.RelativeToParent((parent) =>
                    parent.Width)
                );
        }

        private double timerSeconds;
        public bool TimerAlive { get; private set; }
        public const double timerDuration = 30;
        const double timerRefreshRate = 1;

        public void StartTimer()
        {
            timerSeconds = 0;
            UpdateScoreLabel();
            //timerImage.Rotation = 0;

            if (TimerAlive)
            {
                TimerAlive = false;
            }
            else
            {
                TimerAlive = true;
                Device.StartTimer(System.TimeSpan.FromMilliseconds(timerRefreshRate * 1000), TimerHandler);
            }
            MinerGamePage.UpdateStartButtonText(TimerAlive);
        }

        private bool TimerHandler()
        {
            timerSeconds += timerRefreshRate;
            int last = 30 - (int)timerSeconds;
            if (last < 10)
                timerLabel.Text = $"00:0{last}";
            else
                timerLabel.Text = $"00:{last}";

            //timerImage.Rotation = timerSeconds / timerDuration * 360;

            if (timerSeconds >= timerDuration)
            {
                TimerAlive = false;
                Reboot(true);
            }

            return TimerAlive;
        }

        public void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Луший результат: {BestScore * 10}%";
            System.Console.WriteLine();
        }

        public void Reboot(bool saveScore = true)
        {
            if (saveScore)
            {
                if (BestScore < progress / 2)
                {
                    BestScore = progress / 2;
                    App.Database.SetMineBestScore(BestScore);
                    UpdateScoreLabel();
                }
            }
            while (progress > 0)
                DecProgress();

            MinerGamePage.RestartQuestionLabel();
        }

        public Mine(RelativeLayout layout, int bestScore, Label timerLabel, Label scoreLabel)
        {
            this.timerLabel = timerLabel;
            this.scoreLabel = scoreLabel;
            BestScore = bestScore;

            InitBackgroundImage(layout);
            Blocks = InitMine(layout);
        }
    }
}