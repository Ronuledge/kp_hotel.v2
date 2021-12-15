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
using System.Data.Entity;
using kp_hotel.v2.Windows;
using kp_hotel.v2.Properties;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace kp_hotel.v2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        hotelEntities db = new hotelEntities();

        int idBooking;
        int idRoom;
        int id;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateRooms();
            UpdateBooking();
            UpdateClients();
            lblDate.Content = DateTime.Now.ToLongDateString();
            lblDate2.Content = DateTime.Now.ToLongDateString();
            lblDate3.Content = DateTime.Now.ToLongDateString();
        }





        //Таблица Обзор номеров
        public void UpdateRooms()
        {
            var room = db.Room;
            var quary = from r in room
                        select new { Номер = r.idRoom, Категория = r.Category, Цена = r.Price, Статус = r.Status };
            dgRooms.ItemsSource = quary.ToList();
        }

        private void DgRooms_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRooms();
        }

        //Окно добавления нового номера
        private void BtnAddRooms_Click(object sender, RoutedEventArgs e)
        {
            AddNewRoom winAddRooms = new AddNewRoom();
            winAddRooms.DG = dgRooms;
            winAddRooms.Show();
            this.Close();
        }

        //Окно редактирования номера
        private void BtnRedRooms_Click(object sender, RoutedEventArgs e)
        {
            RedactRooms winRedactRooms = new RedactRooms();
            winRedactRooms.DG = dgRooms;
            winRedactRooms.Show();
            this.Close();
        }

        //Удаление номера
        private void BtnDelRooms_Click(object sender, RoutedEventArgs e)
        {
            var index = dgRooms.SelectedItem;
            int idRoom = Convert.ToInt32((dgRooms.SelectedCells[0].Column.GetCellContent(index) as TextBlock).Text);

            MessageBoxResult result = MessageBox.Show("Удалить номер с ID - " + idRoom + "?", "My App", MessageBoxButton.YesNo);
            try
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var room = db.Room.Where(r => r.idRoom == idRoom);
                        foreach (var r in room)
                        {
                            db.Entry(r).State = EntityState.Deleted;
                        }
                        db.SaveChanges();
                        UpdateRooms();
                        MessageBox.Show("Номер успешно удален");

                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Номер невозможно удалить т.к. он забронирован.");
            }
        }

        //Обновление таблицы Обзора номеров
        private void BtnUpdateRooms_Click(object sender, RoutedEventArgs e)
        {
            UpdateRooms();
            db.SaveChanges();
            MessageBox.Show("БД успешно обновлена");
        }

        //Отчет
        private void BtnOtchet_Click(object sender, RoutedEventArgs e)
        {
            Otchet winOtchet = new Otchet();
            winOtchet.DG = dgRooms;
            winOtchet.Show();
        }




        //Таблица Бронирование
        public void UpdateBooking()
        {
            var clients = db.Client;
            var booking = db.Booking;
            var room = db.Room;
            var quary = from c in clients
                        from b in booking
                        where b.idClient == c.idClient
                        select new {ID = b.idBooking, Фамилия = c.Fam, Имя = c.Name, Отчество = c.Otch, Телефон = c.Telephone, Номер = b.idRoom, Дата_заселения = b.CheckIn, Дата_выселения = b.CheckOut };
            dgBooking.ItemsSource = quary.ToList();
        }

        private void DgBooking_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBooking();
        }

        //Окно добавления брони
        private void BtnBooking_Click(object sender, RoutedEventArgs e)
        {
            AddBooking winAddBooking = new AddBooking();
            winAddBooking.DG = dgRooms;
            winAddBooking.Show();
            this.Close();
        }

        //Удаление брони
        private void BtnDelBooking_Click(object sender, RoutedEventArgs e)
        {
            var index = dgBooking.SelectedItem;
            int idBooking = Convert.ToInt32((dgBooking.SelectedCells[0].Column.GetCellContent(index) as TextBlock).Text);

            MessageBoxResult result = MessageBox.Show("Удалить бронь с ID - " + idBooking + "?", "My App", MessageBoxButton.YesNo);
            try
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var booking = db.Booking.Where(p => p.idBooking == idBooking);

                        foreach (var p in booking)
                        {
                            db.Entry(p).State = EntityState.Deleted;
                        }
                        db.SaveChanges();
                        UpdateBooking();

                        MessageBox.Show("Бронь успешно удалена.");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("При удалении произошла ошибка.");
            }
        }

        //Обновление таблицы Бронирование
        private void BtnUpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            UpdateBooking();
            db.SaveChanges();
            MessageBox.Show("БД успешно обновлена");
        }




        //Таблица Клиентов
        public void UpdateClients()
        {
            var client = db.Client;
            var quary = from c in client
                        select new {ID = c.idClient, Фамилия = c.Fam, Имя = c.Name, Отчество = c.Otch, Телефон = c.Telephone };
            dgClient.ItemsSource = quary.ToList();
        }

        //Окно регистрации нового клиента
        private void BtnRegClient_Click(object sender, RoutedEventArgs e)
        {
            RegClient winRegClient = new RegClient();
            winRegClient.DG = dgRooms;
            winRegClient.Show();
            this.Close();
        }

        private void DgClient_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClients();
        }

        //Обновление таблицы Клиенты
        private void BtnUpdateClient_Click(object sender, RoutedEventArgs e)
        {
            UpdateClients();
            db.SaveChanges();
            MessageBox.Show("БД успешно обновлена");
        }

        //Удаление клиента
        private void BtnDelClient_Click(object sender, RoutedEventArgs e)
        {
            var index = dgClient.SelectedItem;
            int idClient = Convert.ToInt32((dgClient.SelectedCells[0].Column.GetCellContent(index) as TextBlock).Text);

            MessageBoxResult result = MessageBox.Show("Удалить клиента с ID - " + idClient + "?", "My App", MessageBoxButton.YesNo);
            try
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var client = db.Client.Where(p => p.idClient == idClient);

                        foreach (var p in client)
                        {
                            db.Entry(p).State = EntityState.Deleted;
                        }
                        db.SaveChanges();
                        UpdateClients();

                        MessageBox.Show("Клиент успешно удален.");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Клиента невозможно удалить т.к. за клиентом забронирован номер.");
            }
        }

        //Окно редактирование клиента
        private void BtnRedClient_Click(object sender, RoutedEventArgs e)
        {
            RedactClients winRedClient = new RedactClients();
            winRedClient.DG = dgRooms;
            winRedClient.Show();
            this.Close();
        }





        //Выгрузка номеров в Excel
        private void BtnExportRooms_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    WindowState = Excel.XlWindowState.xlMaximized
                };

                Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = wb.Worksheets[1];
                DateTime currentDate = DateTime.Now;
                ws.Columns.AutoFit();

                Excel.Range range = ws.Range["A1", "K1"];
                range.Merge(Type.Missing);
                range.Value = "Отчет";
                range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                for (int i = 0; i < dgRooms.Columns.Count; i++)
                {

                    DataGridColumn dgColumn = dgRooms.Columns[i];
                    TextBlock txt = new TextBlock
                    {
                        Text = dgColumn.Header.ToString()
                    };
                    Excel.Range columnRange = (Excel.Range)ws.Cells[3, i + 2];
                    columnRange.Value2 = txt.Text;
                    columnRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    columnRange.ColumnWidth = 30;
                    for (int j = 0; j < dgRooms.Items.Count; j++)
                    {
                        TextBlock text = dgRooms.Columns[i].GetCellContent(dgRooms.Items[j]) as TextBlock;
                        Excel.Range dataRange = (Excel.Range)ws.Cells[j + 4, i + 2];
                        dataRange.Value2 = text.Text;
                        dataRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Экспорт не удался");
            }
        }

        //Выгрузка бронирований в Excel
        private void BtnExportBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    WindowState = Excel.XlWindowState.xlMaximized
                };

                Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = wb.Worksheets[1];
                DateTime currentDate = DateTime.Now;
                ws.Columns.AutoFit();

                Excel.Range range = ws.Range["A1", "K1"];
                range.Merge(Type.Missing);
                range.Value = "Отчет";
                range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                for (int i = 0; i < dgBooking.Columns.Count; i++)
                {

                    DataGridColumn dgColumn = dgBooking.Columns[i];
                    TextBlock txt = new TextBlock
                    {
                        Text = dgColumn.Header.ToString()
                    };
                    Excel.Range columnRange = (Excel.Range)ws.Cells[3, i + 2];
                    columnRange.Value2 = txt.Text;
                    columnRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    columnRange.ColumnWidth = 30;
                    for (int j = 0; j < dgBooking.Items.Count; j++)
                    {
                        TextBlock text = dgBooking.Columns[i].GetCellContent(dgBooking.Items[j]) as TextBlock;
                        Excel.Range dataRange = (Excel.Range)ws.Cells[j + 4, i + 2];
                        dataRange.Value2 = text.Text;
                        dataRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Экспорт не удался");
            }
        }

        //Выгрузка клиентов в Excel
        private void BtnExportClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel.Application app = new Excel.Application
                {
                    Visible = true,
                    WindowState = Excel.XlWindowState.xlMaximized
                };

                Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = wb.Worksheets[1];
                DateTime currentDate = DateTime.Now;
                ws.Columns.AutoFit();

                Excel.Range range = ws.Range["A1", "K1"];
                range.Merge(Type.Missing);
                range.Value = "Отчет";
                range.Borders.Color = Excel.XlRgbColor.rgbBlack;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.Interior.Color = Excel.XlRgbColor.rgbLightGray;

                for (int i = 0; i < dgClient.Columns.Count; i++)
                {

                    DataGridColumn dgColumn = dgClient.Columns[i];
                    TextBlock txt = new TextBlock
                    {
                        Text = dgColumn.Header.ToString()
                    };
                    Excel.Range columnRange = (Excel.Range)ws.Cells[3, i + 2];
                    columnRange.Value2 = txt.Text;
                    columnRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    columnRange.ColumnWidth = 30;
                    for (int j = 0; j < dgClient.Items.Count; j++)
                    {
                        TextBlock text = dgClient.Columns[i].GetCellContent(dgClient.Items[j]) as TextBlock;
                        Excel.Range dataRange = (Excel.Range)ws.Cells[j + 4, i + 2];
                        dataRange.Value2 = text.Text;
                        dataRange.Borders.Color = Excel.XlRgbColor.rgbBlack;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Экспорт не удался");
            }
        }

        //Поиск номера по статуту в теблице номеров
        private void Search1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rooms = db.Room;
            //Запрос к БД где в Where используется метод Contins и текст из текстбокса. 
            var queryRooms = from r in rooms
                              where (r.Status.Contains(search1.Text))
                              select new { Номер = r.idRoom, Категория = r.Category, Цена = r.Price, Статус = r.Status };
            //Просто обновление таблицы с запроса.
            dgRooms.ItemsSource = queryRooms.ToList();
        }

        //Поиск брони по номеру
        private void Search2_TextChanged(object sender, TextChangedEventArgs e)
        {
            var booking = db.Booking;
            var clients = db.Client;
            //Запрос к БД где в Where используется метод Contins и текст из текстбокса. 
            var queryBooking = from b in booking
                               from c in clients
                             where (b.idRoom.ToString().Contains(search2.Text))
                             select new { ID = b.idBooking, Фамилия = c.Fam, Имя = c.Name, Отчество = c.Otch, Телефон = c.Telephone, Номер = b.idRoom, Дата_заселения = b.CheckIn, Дата_выселения = b.CheckOut };
            //Просто обновление таблицы с запроса.
            dgRooms.ItemsSource = queryBooking.ToList();
        }

        //Поиск клиента по имени
        private void Search3_TextChanged(object sender, TextChangedEventArgs e)
        {
            var clients = db.Client;
            //Запрос к БД где в Where используется метод Contins и текст из текстбокса. 
            var queryClients = from c in clients
                             where (c.Name.Contains(search3.Text))
                             select new { ID = c.idClient, Фамилия = c.Fam, Имя = c.Name, Отчество = c.Otch, Телефон = c.Telephone };
            //Просто обновление таблицы с запроса.
            dgClient.ItemsSource = queryClients.ToList();
        }
    }
}
