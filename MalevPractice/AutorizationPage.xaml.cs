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

namespace MalevPractice
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        int number;

        DispatcherTimer timer = new DispatcherTimer();

        string cancel = "";

        public AutorizationPage()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 20);

            timer.Tick += new EventHandler(Timer_Trick);

            tboxNumber.Focus();
        }

        private void tboxNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int n = 0;

                number = Convert.ToInt32(tboxNumber.Text);

                User SignIn = BaseClass.ME.User.FirstOrDefault(z => z.Number == number);

                if (tboxNumber.Text != "")
                {
                    if (int.TryParse(tboxNumber.Text, out n))
                    {
                        if (SignIn == null)
                        {
                            MessageBox.Show("Нет номера");
                        }

                        else
                        {
                            tboxPassword.IsEnabled = true;

                            tboxPassword.Focus();
                        }
                    }

                    else
                    {
                        MessageBox.Show("Нет номера");
                    }

                }

                else
                {
                    MessageBox.Show("Пустой номер");
                }
            }
        }

        private void tboxPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Random random = new Random();

                int code = random.Next(10000, 90000);

                cancel = "";

                string sp = "!@#$%^&*()";

                string cac = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

                string c = "";

                for (int i = 0; cancel.Length < 6; i++)
                {
                    if (random.Next(1, 3) == 1)
                    {
                        cancel = cancel + (char)(random.Next('A', 'Z'));
                    }

                    else
                    {
                        cancel = cancel + (char)(random.Next('a', 'z'));
                    }
                }

                cancel = cancel + Convert.ToString(random.Next(0, 9));

                cancel = cancel + sp[random.Next(sp.Length)];

                for (int i = 0; c.Length < 8; i++)
                {
                    c = c + Convert.ToString(cac[random.Next(cac.Length)]);
                }

                int n = Convert.ToInt32(tboxPassword.Password);

                User SignIn = BaseClass.ME.User.FirstOrDefault(x => x.Password == n);

                if (tboxPassword.Password != "")
                {

                    if (SignIn == null)
                    {
                        MessageBox.Show("Нет пароля");
                    }

                    else
                    {
                        if (MessageBox.Show(cancel.ToString(), "Cгенерированный код доступа", MessageBoxButton.OK) == MessageBoxResult.OK)
                        {
                            tboxCode.IsEnabled = true;

                            tboxCode.Focus();

                            timer.Start();
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Пустой пароль");
                }
            }
        }

        private void tboxCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                User user = BaseClass.ME.User.FirstOrDefault(z => z.Number == number);

                if (tboxCode.Text != "")
                {

                    if (cancel.ToString() != tboxCode.Text.ToString())
                    {
                        MessageBox.Show("Нет кода");

                        tboxCode.IsEnabled = false;

                        btnUpdateCode.IsEnabled = true;

                        btnSignIn.IsEnabled = false;

                        tboxCode.Text = "";

                        timer.Stop();
                    }

                    else
                    {
                        MessageBox.Show("Вы успешно авторизовались! Ваша роль " + user.Role.Name);

                        timer.Stop();
                    }

                }

                else
                {
                    MessageBox.Show("Пустой код");
                }
            }

        }

        private void Timer_Trick(object sender, EventArgs e)
        {
            MessageBox.Show("Время вышло");

            tboxCode.IsEnabled = false;

            btnUpdateCode.IsEnabled = true;

            btnSignIn.IsEnabled = false;

            tboxCode.Text = "";

            timer.Stop();
        }

        private void btnUpdateCode_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            int code = random.Next(10000, 90000);

            cancel = "";

            string[] capcha = new string[9];

            for (int i = 0; cancel.Length < 8; i++)
            {
                if (random.Next(1, 3) == 1)
                {
                    capcha[i] = Convert.ToString(random.Next(0, 9)) + (char)(random.Next('A', 'Z'));

                    cancel = cancel + capcha[i];
                }

                else
                {
                    capcha[i] = Convert.ToString(random.Next(0, 9)) + (char)(random.Next('a', 'z'));

                    cancel = cancel + capcha[i];
                }
            }

            if (MessageBox.Show(cancel.ToString(), "Cгенерированный код доступа", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                btnUpdateCode.IsEnabled = false;

                tboxCode.IsEnabled = true;

                tboxCode.Focus();

                timer.Start();

                btnSignIn.IsEnabled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tboxNumber.Text = "";

            tboxPassword.Password = "";

            tboxCode.Text = "";

            tboxPassword.IsEnabled = false;

            tboxCode.IsEnabled = false;

            timer.Stop();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            User user = BaseClass.ME.User.FirstOrDefault(z => z.Number == number);

            if (tboxCode.Text != "")
            {

                if (cancel.ToString() != tboxCode.Text.ToString())
                {
                    MessageBox.Show("Нет кода");

                    tboxCode.IsEnabled = false;

                    btnUpdateCode.IsEnabled = true;

                    btnSignIn.IsEnabled = false;

                    tboxCode.Text = "";

                    timer.Stop();
                }

                else
                {
                    MessageBox.Show("Вы успешно авторизовались! Ваша роль " + user.Role.Name);

                    timer.Stop();
                }

            }

            else
            {
                MessageBox.Show("Пустой код");
            }
        }

    }
}
