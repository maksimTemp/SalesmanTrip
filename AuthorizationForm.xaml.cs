using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationForm.xaml
    /// </summary>
    public partial class AuthorizationForm : Window
    {
        public AuthorizationForm()
        {
            InitializeComponent();

        }

        private void Authorization(object sender, RoutedEventArgs e)
        {


            string login = loginBox.Text.Trim();
            string pass1 = passBox1.Password.Trim();
            if (login.Length < 4)
            {
                loginBox.ToolTip = "Логин должен быть больше 4 символов";
                loginBox.Background = Brushes.DarkRed;
            }
            else if (pass1.Length < 8)
            {
                loginBox.Background = Brushes.Transparent;
                passBox1.ToolTip = "Пароль должен быть больше 8 символов";
                passBox1.Background = Brushes.DarkRed;
            }
            else if (!AppContext.FindUserByLoginAndPass(login, pass1))
            {
                MessageBox.Show("Введённые данные не верны, вход невозможен");
            }
            else
            {
                loginBox.ToolTip = "";
                loginBox.Background = Brushes.Transparent;
                passBox1.ToolTip = "";
                passBox1.Background = Brushes.Transparent;

                MessageBox.Show("Авторизация прошла успешно");

                UserWindow mainWindow = new UserWindow();
                mainWindow.Show();
                Close();
            }

        }

        private void RestrationWindowActivation(object sender, RoutedEventArgs e)
        {
            RegistrForm registrForm = new RegistrForm();
            registrForm.Show();
            Close();
        }
    }
}
