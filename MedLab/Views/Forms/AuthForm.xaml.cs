using MedLab.Classes;
using MedLab.Database.DBHelper;
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
        public AuthForm()
        {
            InitializeComponent();
            UsersCombo.ItemsSource = DBHelper.GetContext().User.ToList();
            SetTimerSetting();
        }

        /// <summary>
        /// Установка настроек таймера
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

        private void UnBlock()
        {
            this.IsEnabled = true;
            captchaString = CaptchaGen.GenerateCaptcha(CaptchaCanvas);
            LoginGrid.VerticalAlignment = VerticalAlignment.Top;
            LoginGrid.Margin = new Thickness(0, 0, 0, 0);
            CaptchaText.Visibility = Visibility.Visible;
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
                if(attemps == 5)
                {
                    timer.Start();
                    this.IsEnabled = false;
                }
                return;
            }

            if(captchaString != "" && CaptchaText.Text != captchaString)
            {
                MessageBox.Show("Неверно введена капча.");
                return;
            }

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
                MessageBox.Show($"{errors.ToString()}\nОсталось попыток: {4-attemps}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return null;
            }

            if (user.Password != int.Parse(password))
            {
                MessageBox.Show($"Неправильный пароль!\nОсталось попыток: {4-attemps}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            
            return user;
        }

        /// <summary>
        /// Ограничение текстового поля на ввод одних только цифр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }
    }
}
