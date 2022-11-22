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
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VisualizarTransportista.xaml
    /// </summary>
    public partial class VisualizarTransportista //: Window
    {

        OracleConnection con = null;
        public VisualizarTransportista()
        {
            abrirConexion();
            InitializeComponent();
        }

        private void abrirConexion()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["oracleDB"].ConnectionString;
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT ID_TRANSPORTISTA, NOMBRE, APELLIDO, TELEFONO, LICENCIA_CONDUCIR FROM TRANSPORTISTA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
        }

        private void btnSalirListadoTransportista(object sender, RoutedEventArgs e)
        {
            GestionSubasta v2 = new GestionSubasta();
            this.Close();
            v2.ShowDialog();
        }
    }
}
