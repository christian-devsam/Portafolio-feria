using System;
using bibliotecaFeria;
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
    /// Lógica de interacción para GestionVenta.xaml
    /// </summary>
    public partial class GestionVenta
    {

        OracleConnection con = null;

        public GestionVenta()
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
        private void AUD(String sql_stmt, int state)
        {
            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Venta agregada!";
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("NOMBRE_PRO", OracleDbType.Varchar2, 20).Value = txtnombreProducto.Text;
                    cmd.Parameters.Add("CANTIDAD_PRO", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);
                    cmd.Parameters.Add("ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("DESCRIPCION", OracleDbType.Varchar2, 200).Value = txtDescripcion.Text;

                    break;
                case 1:
                    msg = "Venta modificado!";
                    cmd.Parameters.Add("NOMBRE_PRO", OracleDbType.Varchar2, 20).Value = txtnombreProducto.Text;
                    cmd.Parameters.Add("CANTIDAD_PRO", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);
                    cmd.Parameters.Add("ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("DESCRIPCION", OracleDbType.Varchar2, 200).Value = txtDescripcion.Text;                    
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                    break;
                case 2:
                    msg = "Venta eliminado!";
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

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

        

        private void BtnAgregarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO VENTA (ID_VENTA, NOMBRE_PRO, CANTIDAD_PRO, ENVIO, DESCRIPCION )" + "VALUES(:ID_VENTA, :NOMBRE_PRO, :CANTIDAD_PRO, :ENVIO, :DESCRIPCION )";
                this.AUD(sql, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "UPDATE VENTA SET NOMBRE_PRO = :NOMBRE_PRO," + "CANTIDAD_PRO = :CANTIDAD_PRO, ENVIO = :ENVIO, DESCRIPCION = :DESCRIPCION " + "WHERE ID_VENTA = :ID_VENTA";
                this.AUD(sql, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTerminarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "DELETE FROM VENTA " + "WHERE ID_VENTA = :ID_VENTA";
                this.AUD(sql, 2);
                this.limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM VENTA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();

        }

        private void btnListaVenta_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            listar();
        }

        private void btnSalirGestionVenta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow volver = new MainWindow();
            this.Close();
            volver.ShowDialog();
        }

        private void limpiar()
        {
            txtidventa.Text = "";
            txtnombreProducto.Text = "";
            txtcantidadProducto.Text = "";
            DpickerFinal.ToString();
            txtDescripcion.Text = "";
        }
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidventa.Text = dr["ID_VENTA"].ToString();
                txtnombreProducto.Text = dr["NOMBRE_PRO"].ToString();
                txtcantidadProducto.Text = dr["CANTIDAD_PRO"].ToString();
                DpickerFinal.SelectedDate = Convert.ToDateTime(dr["ENVIO"]);
                txtDescripcion.Text = dr["DESCRIPCION"].ToString();

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

        private void cboNombreCliente_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd3 = new OracleCommand("SELECT NOMBRES_REGISTRO FROM CLIENTE2", con);

            OracleDataReader rw = cmd3.ExecuteReader();
            while (rw.Read())
            {
                cboNombreCliente.Items.Add(rw["NOMBRES_REGISTRO"].ToString());
            }
        }

        
    }
}
