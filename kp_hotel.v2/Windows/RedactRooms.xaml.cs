using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для RedactRooms.xaml
    /// </summary>
    public partial class RedactRooms : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;

        public RedactRooms()
        {
            InitializeComponent();
        }

        public void UpdateRooms()
        {
            var room = db.Room;
            var quary = from r in room
                        select new { Номер = r.idRoom, Категория = r.Category, Цена = r.Price, Статус = r.Status };
            dgRedRooms.ItemsSource = quary.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            db.Room.Load();
            dgRedRooms.ItemsSource = db.Room.Local.ToBindingList();
            UpdateRooms();
        }

        private void DgRedRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = dgRedRooms.SelectedItem;
            if (index != null)
            {
                cmbCategoryRooms.Text = (dgRedRooms.SelectedCells[1].Column.GetCellContent(index) as TextBlock).Text;
                txtPrice.Text = (dgRedRooms.SelectedCells[2].Column.GetCellContent(index) as TextBlock).Text;
                cmbStatus.Text = (dgRedRooms.SelectedCells[3].Column.GetCellContent(index) as TextBlock).Text;
            }
        }

        private void BtnRedRooms_Click(object sender, RoutedEventArgs e)
        {
            var index1 = dgRedRooms.SelectedItem;
            if (index1 != null)
            {
                id = Convert.ToInt32((dgRedRooms.SelectedCells[0].Column.GetCellContent(index1) as TextBlock).Text);
            }

            var rooms = db.Room.Where(p => p.idRoom == id);

            foreach (var p in rooms)
            {
                p.Category = cmbCategoryRooms.Text;
                p.Price = Convert.ToInt16(txtPrice.Text);
                p.Status = cmbStatus.Text;

                db.Entry(p).State = EntityState.Modified;
            }
            db.SaveChanges();
            MessageBox.Show("Редактирование успешно.");
            UpdateRooms();
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
