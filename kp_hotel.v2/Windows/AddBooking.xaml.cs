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
    /// Логика взаимодействия для AddBooking.xaml
    /// </summary>
    public partial class AddBooking : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;

        public AddBooking()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var client = db.Client;
            List<string> listClient = new List<string>();
            foreach (var c in client)
            {
                listClient.Add(c.Fam + c.Name + c.Otch);
            }
            cmbClient.ItemsSource = listClient;

            var room = db.Room;
            List<string> listRoom = new List<string>();
            foreach (var r in room)
            {
                listRoom.Add(r.idRoom + r.Category + r.Status);
            }
            cmbRoom.ItemsSource = listRoom;
        }

        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = new Booking();
            Room roomes = new Room();

            var client = db.Client;
            var clients = from c in client
                        where (c.Fam + c.Name + c.Otch == cmbClient.SelectedItem.ToString())
                        select new { c.idClient };
            foreach (var c in clients)

            {

                booking.idClient = c.idClient;

    
            }

            var room = db.Room;
            var rooms = from r in room
                        where r.idRoom.ToString()+r.Category+r.Status==cmbRoom.SelectedItem.ToString()
                           select new { r.idRoom };
          
            foreach (var r in rooms)
            {
                booking.idRoom = r.idRoom;
            }

            

            booking.CheckIn = dpCheckIn.SelectedDate.Value;
            booking.CheckOut = dpCheckOut.SelectedDate.Value;


            db.Booking.Add(booking);
            db.SaveChanges();
            MessageBox.Show("Бронь успешно создана");
            
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
