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
        private string AUD(String sql_stmt, int state)
        {
            string msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Venta agregada!";
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("FECHA_ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedValue.ToString();


                    

                    break;
                case 1:
                    msg = "Venta modificado!";
                    cmd.Parameters.Add("NOMBRE_PRO", OracleDbType.Varchar2, 20).Value = cboProductos.Text;
                    cmd.Parameters.Add("CANTIDAD_PRO", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);
                    cmd.Parameters.Add("ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;                
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                    break;
                case 2:
                    msg = "Venta eliminado!";
                    cmd.Parameters.Add("ID_VENTA", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                    break;
                case 3:
                    msg = "Detalle venta agregado!";

                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboProductos.SelectedValue.ToString();
                    cmd.Parameters.Add("CANTIDAD", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);

                    break;
            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    return msg;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return ex.Message.ToString();
            }
        }

        

        private void BtnAgregarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO PEDIDO (ID_PEDIDO, FECHA_INGRESO, FECHA_ENVIO, RUT_CLI)" + "VALUES(:ID_PEDIDO, :FECHA_INGRESO, :FECHA_ENVIO, :RUT_CLI )";
                string mensaje = this.AUD(sql, 0);

                if (mensaje == "Venta agregada!")
                {
                    btnCrear.IsEnabled = false;
                    btnTerminar.IsEnabled = false;
                    btnActualizar.IsEnabled = false;
                    btnIngresarDetallePedido.IsEnabled = true;
                }
               
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

        

        /*private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM VENTA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();

        }*/

        private void btnListaVenta_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            //listar();
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
            cboProductos.Text = "";
            txtcantidadProducto.Text = "";
            DpickerFinal.ToString();
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
                cboProductos.Text = dr["NOMBRE_PRO"].ToString();
                txtcantidadProducto.Text = dr["CANTIDAD_PRO"].ToString();
                DpickerFinal.SelectedDate = Convert.ToDateTime(dr["ENVIO"]);

            }
        }

        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            //this.listar();
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
            OracleCommand cmd3 = new OracleCommand("SELECT RUT_CLI, NOMBRE FROM CLIENTE", con);

            OracleDataReader rw = cmd3.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rw);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboNombreCliente.ItemsSource = data;

            cboNombreCliente.DisplayMemberPath = dt.Columns["NOMBRE"].ToString();
            cboNombreCliente.SelectedValuePath = dt.Columns["RUT_CLI"].ToString();
        }

        

        private void BtnAgregarDetalleVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO DETALLE_PEDIDO (ID_PEDIDO,ID_PRODUCTO, CANTIDAD)" + "VALUES(:ID_PEDIDO, :ID_PRODUCTO, :CANTIDAD )";
                this.AUD(sql, 3);

                btnCrear.IsEnabled = false;
                btnTerminar.IsEnabled = true;
                btnActualizar.IsEnabled = true;
                btnIngresarDetallePedido.IsEnabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboProductos_loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select distinct(rp.id_producto), nombre_producto from producto p join registro_producto rp on p.id_producto= rp.id_producto", con);

            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboProductos.ItemsSource = data;
            cboProductos.DisplayMemberPath = dt.Columns["NOMBRE_PRODUCTO"].ToString();
            cboProductos.SelectedValuePath = dt.Columns["ID_PRODUCTO"].ToString();
        }

        private void txtidventa_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(id_pedido)+1 as id_pedido from pedido", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidventa.Text = dt.Rows[0][0].ToString();
        }

        public void llenarProductos(int idventa)
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM detalle_pedido where id_pedido = " + "'"+idventa+"'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void txtidventa_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                int entero = int.Parse(txtidventa.Text);
                if (txtidventa.Text.Length != 0)
                {
                    llenarProductos(int.Parse(txtidventa.Text));
                }
            }
            catch (Exception ex)
            {

                limpiar();
            }
        }
    }
}
