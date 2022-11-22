using bibliotecaFeria;
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
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GestionSubasta.xaml
    /// </summary>
    public partial class GestionSubasta
    {
        OracleConnection con = null;

        public GestionSubasta()
        {
            this.abrirConexion();
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

        private void AUD(String sql_stmt, int state)
        {
            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Subasta Agregada!";
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = cboIdVenta.SelectedItem;
                    cmd.Parameters.Add("NOMBRE_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedItem;
                    cmd.Parameters.Add("TERMINO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;

                    break;
                case 1:
                    msg = "Subasta Modificada!";
                    cmd.Parameters.Add("NOMBRE_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedItem;
                    cmd.Parameters.Add("TERMINO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = cboIdVenta.SelectedItem;

                    break;
                case 2:
                    msg = "Subasta Eliminada!";
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = cboIdVenta.SelectedItem;

                    break;
            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.listar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void BtnAgregarSubasta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO PUJANZA (ID_VENTA, NOMBRE_CLI, TERMINO )" + " VALUES(:ID_VENTA, :NOMBRE_CLI, :TERMINO )";
                this.AUD(sql, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarSubasta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "UPDATE PUJANZA SET NOMBRE_CLI = :NOMBRE_CLI, TERMINO = :TERMINO " + "WHERE ID_VENTA = :ID_VENTA";
                this.AUD(sql, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTerminarSubasta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "DETELE FROM PUJANZA " + "WHERE ID_VENTA = :ID_VENTA";
                this.AUD(sql, 2);
                this.limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void btnSalirGestionSubasta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow v1 = new MainWindow();
            this.Close();
            v1.ShowDialog();
        }

        private void btnLimpiarSubastaClick(object sender, RoutedEventArgs e)
        {
            limpiar();
        }
        private void limpiar()
        {
            cboIdVenta.SelectedValue = "";
            cboNombreCliente.SelectedValue = "";
            DpickerFinal.ToString();

        }

        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM PUJANZA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }


        

        private void cboIdVenta_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd2 = new OracleCommand("SELECT ID_VENTA FROM VENTA", con);

            OracleDataReader rd = cmd2.ExecuteReader();
            while (rd.Read())
            {
                cboIdVenta.Items.Add(rd["ID_VENTA"].ToString());
            }
        }

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                cboIdVenta.SelectedValue = dr["ID_VENTA"].ToString();
                cboNombreCliente.SelectedValue = dr["NOMBRE_CLI"].ToString();
                DpickerFinal.SelectedDate = Convert.ToDateTime(dr["TERMINO"]);
            }
        }

        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnVerTransportista_Click(object sender, RoutedEventArgs e)
        {
            VisualizarTransportista v1 = new VisualizarTransportista();
            this.Close();
            v1.ShowDialog();
        }

        
        

        private void cboNombreCliente_Loaded(object sender, RoutedEventArgs e)
            
        {
            

            OracleCommand cmd3 = new OracleCommand("", con);

            OracleDataReader rw = cmd3.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rw);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboNombreCliente.ItemsSource = data;

            cboNombreCliente.DisplayMemberPath = dt.Columns["NOMBRES_REGISTRO"].ToString();
            cboNombreCliente.SelectedValuePath = dt.Columns["ID_VENTA"].ToString();
        }

        
    }
}
