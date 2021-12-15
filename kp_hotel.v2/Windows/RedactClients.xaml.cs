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
    /// Логика взаимодействия для RedactClients.xaml
    /// </summary>
    public partial class RedactClients : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;

        public RedactClients()
        {
            InitializeComponent();
        }

        public void UpdateClients()
        {
            var client = db.Client;
            var quary = from c in client
                        select new { ID = c.idClient, Фамилия = c.Fam, Имя = c.Name, Отчество = c.Otch, Телефон = c.Telephone, };
            dgRedClient.ItemsSource = quary.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            db.Client.Load();
            dgRedClient.ItemsSource = db.Client.Local.ToBindingList();
            UpdateClients();
        }

        private void DgRedClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = dgRedClient.SelectedItem;
            if (index != null)
            {
                txtFam.Text = (dgRedClient.SelectedCells[1].Column.GetCellContent(index) as TextBlock).Text;
                txtName.Text = (dgRedClient.SelectedCells[2].Column.GetCellContent(index) as TextBlock).Text;
                txtOtch.Text = (dgRedClient.SelectedCells[3].Column.GetCellContent(index) as TextBlock).Text;
                txtTelephone.Text = (dgRedClient.SelectedCells[4].Column.GetCellContent(index) as TextBlock).Text;
            }
        }

        private void BtnRedClients_Click(object sender, RoutedEventArgs e)
        {
            var index = dgRedClient.SelectedItem;
            if (index != null)
            {
                id = Convert.ToInt32((dgRedClient.SelectedCells[0].Column.GetCellContent(index) as TextBlock).Text);
            }

            var clients = db.Client.Where(p => p.idClient == id);

            foreach (var p in clients)
            {
                p.Fam = txtFam.Text;
                p.Name = txtName.Text;
                p.Otch = txtOtch.Text;
                p.Telephone = txtTelephone.Text;

                db.Entry(p).State = EntityState.Modified;
            }
            db.SaveChanges();
            MessageBox.Show("Редактирование успешно.");
            UpdateClients();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
