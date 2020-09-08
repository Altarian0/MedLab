using MedLab.Classes;
using MedLab.Classes.Models;
using MedLab.Database.DBHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MedLab.Views.Forms
{
    /// <summary>
    /// Interaction logic for AuthForm.xaml
    /// </summary>
    public partial class AuthForm : Window
    {
       private DispatcherTimer timer = new DispatcherTimer();
        private int time = 0;
        private int attemps = 0;
        private string captchaString = "";
        private bool captchaActive = false;
        public AuthForm()
        {
            InitializeComponent();
            UsersCombo.ItemsSource = DBHelper.GetContext().User.ToList(); 
            SetTimerSetting();
            CheckTimerData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetTimerSetting()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            this.Title = (10 - time).ToString();
            if (time >= 10)
            {
                timer.Stop();
                UnBlock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CaptchaActivate()
        {
            captchaString = CaptchaGen.GenerateCaptcha(CaptchaCanvas);
            captchaActive = true;

            CaptchaText.Visibility = Visibility.Visible;
            CaptchaButton.Visibility = Visibility.Visible;

            LoginGrid.VerticalAlignment = VerticalAlignment.Top;
            LoginGrid.Margin = new Thickness(0, 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnBlock()
        {
            this.IsEnabled = true;
            CaptchaActivate();

            time = 0;
            attemps = 0;

            Title = "Авторизация";
        }

        /// <summary>
        /// Вход в систему при успешной валидации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user = DataValidation(PasswordBox.Text, (User)UsersCombo.SelectedItem);

            if (user == null)
            {
                attemps++;
                if (attemps == 5)
                {
                    timer.Start();
                    this.IsEnabled = false;
                }
                return;
            }

            if (captchaString != "" && CaptchaText.Text != captchaString)
            {
                attemps++;
                MessageBox.Show("Неверно введена капча.");
                captchaString = CaptchaGen.GenerateCaptcha(CaptchaCanvas);
                return;
            }

            captchaActive = false;
            attemps = 0;
            time = 0;
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Валидация веденных пользователем данных
        /// </summary>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private User DataValidation(string password, User user)
        {
            StringBuilder errors = new StringBuilder();

            if (UsersCombo.SelectedItem == null)
                errors.AppendLine("Выберите ваш логин!");
            if (string.IsNullOrWhiteSpace(password))
                errors.AppendLine("Введите пароль!");

            if (errors.Length > 0)
            {
                MessageBox.Show($"{errors.ToString()}\nОсталось попыток: {4 - attemps}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return null;
            }

            if (user.Password != int.Parse(password))
            {
                MessageBox.Show($"Неправильный пароль!\nОсталось попыток: {4 - attemps}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }  

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            captchaString = CaptchaGen.GenerateCaptcha(CaptchaCanvas);

        }

        /// <summary>
        /// Проверка последнего выхода на блокировку
        /// </summary>
        private void CheckTimerData()
        {
            string json = "";

            using (FileStream fileStream = new FileStream(@"TimerData.json", FileMode.OpenOrCreate))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    json = streamReader.ReadToEnd();
                }
            }

            TimerData timerData = JsonConvert.DeserializeObject<TimerData>(json);
            time = timerData.Time;
            attemps = timerData.Attemps;
            captchaActive = timerData.CaptchaActive;

            if (time != 0)
            {
                this.IsEnabled = false;
                timer.Start();
            }

            if(captchaActive == true)
            {
                CaptchaActivate();
            }
        }

        /// <summary>
        /// Сохранение данных при выходе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (FileStream fileStream = new FileStream(@"TimerData.json", FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(new TimerData { Time = time, Attemps = attemps, CaptchaActive = captchaActive }));
                }
            }
        }
    }
}
