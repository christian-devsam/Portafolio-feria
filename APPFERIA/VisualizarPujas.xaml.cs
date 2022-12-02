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
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VisualizarPujas.xaml
    /// </summary>
    public partial class VisualizarPujas : Window
    {

        OracleConnection con = null;
        public VisualizarPujas()
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
            try
            {

            
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "select p.ID_SUBASTA, t.NOMBRE, p.VALOR from puja p join transportista t on p.id_transportista = t.id_transportista where p.id_subasta = " + "'" + txtid.Text + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);

            var data = (dt as System.ComponentModel.IListSource).GetList();

            dgvListado.ItemsSource = data;

            
            dr.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
