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

namespace kp_hotel.v2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        hotelEntities db = new hotelEntities();
        public Autorization()
        {
            InitializeComponent();
        }

        public void UserAutoriz(string Rol)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var users = db.Staff;
            var query = from u in users
                        where ((txtLogin.Text == u.Login) && (txtPassword.Text == u.Password))
                        select u;
            if (txtLogin.Text == "admin" && txtPassword.Text == "admin")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин и/или пароль");
            }
        }
    }
}
