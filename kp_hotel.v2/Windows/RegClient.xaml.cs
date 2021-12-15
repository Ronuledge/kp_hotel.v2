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
using System.Data.Entity;

namespace kp_hotel.v2.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegClient.xaml
    /// </summary>
    public partial class RegClient : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;

        public RegClient()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRegClient_Click(object sender, RoutedEventArgs e)
        {
            Client client = new Client();

            client.Fam = txtFam.Text;
            client.Name = txtName.Text;
            client.Otch = txtOtch.Text;
            client.Telephone = txtTelephone.Text;

            db.Client.Add(client);
            db.SaveChanges();
            MessageBox.Show("Клиент добавлен");
            this.Close();
        }

        private void TxtNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var num = Convert.ToInt64(txtTelephone.Text);
                txtTelephone.Text = num.ToString("+7##########");
            }
            catch (Exception)
            {

            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
