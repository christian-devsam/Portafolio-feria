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
    /// Lógica de interacción para ADMDetallePedido.xaml
    /// </summary>
    public partial class ADMDetallePedido //: Window
    {
        OracleConnection con = null;
        public ADMDetallePedido()
        {
            abrirConexion();
            InitializeComponent();
            txtidventa.IsEnabled = false;
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
        private string AUD(String sql_stmt, int state)
        {
            string msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Detalle venta agregado!";
                    msg += "";
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboProductos.SelectedValue.ToString();
                    cmd.Parameters.Add("CANTIDAD", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);

                    break;
                case 1:
                    msg = "Detalle venta modificado!";

                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboProductos.SelectedValue.ToString();
                    cmd.Parameters.Add("CANTIDAD", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                    break;
                case 2:
                    msg = "Detalle venta eliminado!";
                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboProductos.SelectedValue.ToString();

                    break;
            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                }
                return msg;
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
                String sql = "INSERT INTO DETALLE_PEDIDO (ID_PEDIDO,ID_PRODUCTO, CANTIDAD)" + "VALUES(:ID_PEDIDO, :ID_PRODUCTO, :CANTIDAD )";
                string respuesta = this.AUD(sql, 0);

                

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
                String sql = "UPDATE DETALLE_PEDIDO SET ID_PRODUCTO = :ID_PRODUCTO," + "CANTIDAD = :CANTIDAD " + "WHERE ID_PEDIDO = :ID_PEDIDO";
                string respueta = this.AUD(sql, 1);
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
                String sql = "DELETE FROM DETALLE_PEDIDO " + "WHERE ID_PRODUCTO = :ID_PRODUCTO";
                string respuesta = this.AUD(sql, 2);
                this.limpiar();
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
        private void limpiar()
        {
            
            cboProductos.Text = "";
            txtcantidadProducto.Text = "";
            
        }
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            this.limpiar();
        }

        

        private void BtnListo_Click(object sender, RoutedEventArgs e)
        {
            VisualizarVentas vis = new VisualizarVentas();
            this.Close();
            vis.ShowDialog();
        }

        

        

        
    }

}
