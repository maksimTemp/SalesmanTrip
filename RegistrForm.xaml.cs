using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{

    public partial class RegistrForm : Window
    {
        public RegistrForm()
        {
            InitializeComponent();

        }
        private void Registration(object sender, RoutedEventArgs e)
        {
            RegistrForm registrForm = new RegistrForm();
            AuthorizationForm authorizationForm = new AuthorizationForm();
            string login = loginBox.Text.Trim();
            string pass1 = passBox1.Password.Trim();
            string pass2 = passBox2.Password.Trim();

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
            else if (pass1 != pass2)
            {
                passBox1.Background = Brushes.Transparent;
                passBox2.ToolTip = "Введённые пароли не совпадают";
                passBox2.Background = Brushes.DarkRed;
            }
            else if (AppContext.FindUserByLogin(login))
            {
                MessageBox.Show("Такой пользователь уже зарегистрирован, выберите другой логин для регистрации");
            }
            else
            {
                loginBox.ToolTip = "";
                loginBox.Background = Brushes.Transparent;
                passBox1.ToolTip = "";
                passBox1.Background = Brushes.Transparent;
                passBox2.ToolTip = "";
                passBox2.Background = Brushes.Transparent;


                User newUser = new User(login, pass1);
                AppContext.SaveNewUser(newUser);

                MessageBox.Show("Регистрация прошла успешно");
                UserWindow mainWindow = new UserWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void AuthorizationWindowActivation(object sender, RoutedEventArgs e)
        {
            AuthorizationForm authorizationForm = new AuthorizationForm();
            authorizationForm.Show();
            Close();
        }
    }
}
