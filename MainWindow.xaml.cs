using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PartyNest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int SPEED_AMOUNT = 5;
        public const int UPDATE_PERIOD = 25;
        public int Speed { get; set; }
        public int Size { get; set; }
        public Direction Direction { get; set; }
        public double TargetLeft { get; set; }
        public double ParrotLeft { get; set; }
        public AnimationState AnimationState { get; set; }
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            Direction = new Random().Next(2) == 0 ? Direction.Right : Direction.Left;
            AnimationState = AnimationState.Initialising;
            Size = 64;

            Width = Size;
            Height = Size;
            ParrotCanvas.Width = Size;
            ParrotCanvas.Height = Size;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(UPDATE_PERIOD),
            };
            timer.Tick += Timer_Elapsed;

            ParrotImage.Loaded += ParrotImage_Loaded;

            SetRandomParrot(GetAllParrots());
        }

        private void ParrotImage_Loaded(object sender, RoutedEventArgs e)
        {
            NextState();
            timer.Start();
        }

        private IEnumerable<Parrot> GetAllParrots()
        {
            return Parrot.Parrots;
        }

        private IEnumerable<Parrot> GetStandardParrots()
        {
            return Parrot.Parrots.Where(p => !p.IsHD);
        }

        private IEnumerable<Parrot> GetHDParrots()
        {
            var hdParrots = Parrot.Parrots.Where(p => p.IsHD);
            return hdParrots;
        }
        private void SetRandomParrot(IEnumerable<Parrot> parrots)
        {
            var parrot = parrots.ElementAt(new Random().Next(0, parrots.Count()));
            var url = "https://cultofthepartyparrot.com/parrots/" + parrot.Path;
            var source = new ImageSourceConverter();
            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(ParrotImage, (ImageSource)source.ConvertFromString(url));
        }

        private void NextState()
        {
            Console.Write(String.Format("{0} > ", AnimationState.ToString()));
            switch (AnimationState)
            {
                case AnimationState.Initialising:
                    AnimationState = AnimationState.Entering;
                    switch (Direction)
                    {
                        case Direction.Left:
                            // move window the far right of monitor
                            Left = SystemParameters.WorkArea.Width - Size;
                            // move image off the right side of the window
                            ParrotLeft = Size;
                            // advance to the next state when the parrot reaches 0
                            TargetLeft = 0;
                            Speed = -SPEED_AMOUNT;
                            break;
                        case Direction.Right:
                            // move the window to the far left
                            Left = 0;
                            // move the image off the left side of the window
                            ParrotLeft = -Size;
                            // advanceto the next state when the parrot reaches Size
                            TargetLeft = 0;
                            Speed = SPEED_AMOUNT;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    Canvas.SetLeft(ParrotImage, ParrotLeft);
                    Top = SystemParameters.WorkArea.Height - Size;
                    break;
                case AnimationState.Entering:
                    AnimationState = AnimationState.Moving;
                    switch (Direction)
                    {
                        case Direction.Left:
                            TargetLeft = 0;
                            break;
                        case Direction.Right:
                            TargetLeft = SystemParameters.WorkArea.Width - Size;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case AnimationState.Moving:
                    AnimationState = AnimationState.Leaving;
                    switch (Direction)
                    {
                        case Direction.Left:
                            // make sure window is on the far left
                            Left = 0;
                            // advance to the next state when the parrot is off the window
                            TargetLeft = -Size;
                            break;
                        case Direction.Right:
                            // make sure the window is on the far right
                            Left = SystemParameters.WorkArea.Width - Size;
                            // advance to the next state when the parrot is off the window
                            TargetLeft = Size;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case AnimationState.Leaving:
                    // we're all done!
                    Close();
                    break;
                default:
                    throw new NotImplementedException();
            }
            Console.WriteLine(String.Format("{0}", AnimationState.ToString()));
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            switch (AnimationState)
            {
                case AnimationState.Leaving:
                case AnimationState.Entering:
                    ParrotLeft += Speed;
                    switch (Direction)
                    {
                        case Direction.Left:
                            if (ParrotLeft < TargetLeft)
                            {
                                NextState();
                            }
                            break;
                        case Direction.Right:
                            if (ParrotLeft > TargetLeft)
                            {
                                NextState();
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    Canvas.SetLeft(ParrotImage, ParrotLeft);
                    break;
                case AnimationState.Moving:
                    Left += Speed;
                    switch (Direction)
                    {
                        case Direction.Left:
                            if (Left < TargetLeft)
                            {
                                NextState();
                            }
                            break;
                        case Direction.Right:
                            if (Left > TargetLeft)
                            {
                                NextState();
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }

    public enum Direction { Left, Right };

    public enum AnimationState { Initialising, Entering, Moving, Leaving, };
}

