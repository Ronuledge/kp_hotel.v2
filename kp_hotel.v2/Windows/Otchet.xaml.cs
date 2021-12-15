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
    /// Логика взаимодействия для Otchet.xaml
    /// </summary>
    public partial class Otchet : Window
    {
        hotelEntities db = new hotelEntities();
        public DataGrid DG { get; set; }
        int id;
        public Otchet()
        {
            InitializeComponent();
        }

        private void BtnAddOtchet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
