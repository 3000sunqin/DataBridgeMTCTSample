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
using DataBridgeMTCTSample.DataBaseConnection;
using NHibernate;
using DataBridgeMTCTSample.Entities;

namespace DataBridgeMTCTSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (ISession session = DatabaseConnectionHelper.CreateDatabaseSessionFactory().OpenSession())
            {
                ABCEntity entity = new ABCEntity{A= "A",B= "B",C= 1.00 };
                session.SaveOrUpdate(entity);
                DataGrid1.ItemsSource = session.QueryOver<ABCEntity>().List();
            }
        }
    }
}
