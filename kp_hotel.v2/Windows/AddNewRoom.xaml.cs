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
    /// Логика взаимодействия для AddNewRoom.xaml
    /// </summary>
    public partial class AddNewRoom : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;

        public AddNewRoom()
        {
            InitializeComponent();
        }

        private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            Room room = new Room();

            room.Category = cmbCategoryRooms.Text;
            room.Price = Convert.ToInt32(txtPrice.Text);
            room.Status = cmbStatus.Text;

            db.Room.Add(room);
            db.SaveChanges();
            MessageBox.Show("Номер успешно добавлен");
            
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        int idCategory;
        int idStatus;

        private void CmbCategoryRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategoryRooms.SelectedItem == catStandart)
            {
                txtPrice.Text = Convert.ToString("1500");
                idCategory = 0;
            }

            if (cmbCategoryRooms.SelectedItem == catVip)
            {
                txtPrice.Text = Convert.ToString("4500");
                idCategory = 1;
            }

        }

        private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStatus.SelectedItem == statReady)
                idStatus = 0;
            if (cmbStatus.SelectedItem == statBusy)
                idStatus = 1;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
