using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace BlackScreenApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _countdownTimer;
        private TimeSpan _countdownDuration;

        private ImageBrush _buttonBackgroundImageBrush;

        private string _baseUrl;
        private double _numOfDurationMinutes;
        private int _deleyMiliseconds;


        public MainWindow()
        {
            InitializeComponent();
            _numOfDurationMinutes = Constants.DefaultNumOfDurationMinutes;
            _deleyMiliseconds = Constants.DefaultDeleyMiliseconds;
            _countdownDuration = TimeSpan.FromMinutes(_numOfDurationMinutes);
            _baseUrl = AppDomain.CurrentDomain.BaseDirectory;

            _buttonBackgroundImageBrush = new ImageBrush();

            SubsribeToMainButtonEvents();
            SetMainButtonImage(Constants.DoctorImageName);

            InitializeCountdownTimer();

        }

        private void SubsribeToMainButtonEvents()
        {
            mainButton.PreviewMouseRightButtonDown += (s, e) =>
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    _numOfDurationMinutes++;
                    _countdownDuration = _countdownDuration.Add(TimeSpan.FromMinutes(1));
                    UpdateCountdownText();
                }
            };
            mainButton.Click += (s, e) =>
            {
                _numOfDurationMinutes--;
                if (_numOfDurationMinutes<=0)
                    _numOfDurationMinutes = Constants.DefaultNumOfDurationMinutes;

                _countdownDuration = _countdownDuration.Subtract(TimeSpan.FromMinutes(1));
                UpdateCountdownText();
            };
        }

        private void InitializeCountdownTimer()
        {
            _countdownTimer = new DispatcherTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_countdownDuration.TotalSeconds > 0)
            {
                _countdownDuration = _countdownDuration.Subtract(TimeSpan.FromSeconds(1));
                UpdateCountdownText();
            }
            else
            {
                CountdownCompleted();
            }
        }

        private async void CountdownCompleted()
        {
            mainButton.IsEnabled = false;

            countdownText.Text = "Бабах!";
            SetMainButtonImage(Constants.FistImageName);

            _countdownTimer.Stop();
            Window1 window1 = new Window1();

            window1.Closed += OnWindow1Closed;

            window1.Show();
            await Task.Delay(_deleyMiliseconds);
            if (window1.IsActive)
                window1.Close();
            window1.Closed -= OnWindow1Closed;
        }

        private void OnWindow1Closed(object sender, EventArgs e)
        {
            _countdownDuration = TimeSpan.FromMinutes(_numOfDurationMinutes);
            _countdownTimer.Start();

            UpdateCountdownText();
            SetMainButtonImage(Constants.DoctorImageName);

            mainButton.IsEnabled = true;
        }

        private void UpdateCountdownText()
        {
            countdownText.Text = $"{_countdownDuration:mm\\:ss}";
        }

        private void SetMainButtonImage(string fileName)
        {
            string fullPath = System.IO.Path.Combine(_baseUrl, fileName);

            _buttonBackgroundImageBrush.ImageSource = new BitmapImage(new Uri(fullPath));

            mainButton.Background = _buttonBackgroundImageBrush;
        }
    }
}
