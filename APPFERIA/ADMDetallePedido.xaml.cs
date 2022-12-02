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
                    msg = "Producto agregado!";
                    
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboproductos.SelectedValue;
                    cmd.Parameters.Add("CANTIDAD", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);

                    break;
                case 1:
                    msg = "Producto modificado!";

                    cmd.Parameters.Add("ID_PRODUCTO", OracleDbType.Int32, 6).Value = cboproductos.SelectedValue;
                    cmd.Parameters.Add("CANTIDAD", OracleDbType.Int32, 6).Value = int.Parse(txtcantidadProducto.Text);
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                    break;
                case 2:
                    msg = "Pedido cancelado!";
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

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
        private void btnTerminarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                String sql = "DELETE FROM DETALLE_PEDIDO " + "WHERE ID_PEDIDO = :ID_PEDIDO  ";
                String sql1 = "DELETE FROM PEDIDO " + "WHERE ID_PEDIDO = :ID_PEDIDO";
                string respuesta = this.AUD(sql, 2);
                string respuesta1 = this.AUD(sql1, 2);
                if (respuesta == "Pedido cancelado!" && respuesta1 == "Pedido cancelado!")
                {
                    VisualizarVentas verve = new VisualizarVentas();
                    this.Close();
                    verve.ShowDialog();
                }
                this.limpiar();


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
                String sql = "INSERT INTO DETALLE_PEDIDO (ID_PEDIDO,ID_PRODUCTO, CANTIDAD)" + "VALUES(:ID_PEDIDO, :ID_PRODUCTO, :CANTIDAD )";
                string respuesta = this.AUD(sql, 0);
                this.limpiar();
                this.listar();

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
                String sql = "UPDATE DETALLE_PEDIDO SET ID_PRODUCTO = :ID_PRODUCTO, CANTIDAD = :CANTIDAD " + "WHERE ID_PEDIDO = :ID_PEDIDO";
                string respueta = this.AUD(sql, 1);
                this.limpiar();
                this.listar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

       
        private void limpiar()
        {

            cboproductos.Text = "";
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


        private void cboproductos_Loaded(object sender, RoutedEventArgs e)
        {

            OracleCommand cmds = new OracleCommand("select distinct(rp.id_producto), p.nombre_producto from producto p inner join registro_producto rp on p.id_producto= rp.id_producto", con);

            OracleDataReader ru = cmds.ExecuteReader();

            DataTable dete = new DataTable();
            dete.Load(ru);
            var data = (dete as System.ComponentModel.IListSource).GetList();

            cboproductos.ItemsSource = data;
            cboproductos.DisplayMemberPath = dete.Columns["NOMBRE_PRODUCTO"].ToString();
            cboproductos.SelectedValuePath = dete.Columns["ID_PRODUCTO"].ToString();

        }

        public void listar()
        {
            OracleCommand cmds = con.CreateCommand();

            cmds.CommandText = "SELECT d.id_pedido, p.nombre_producto, d.cantidad, d.valor FROM DETALLE_PEDIDO D INNER JOIN PRODUCTO P ON P.ID_PRODUCTO = D.ID_PRODUCTO where d.id_pedido = " + "'" + txtidventa.Text + "'";
            cmds.CommandType = CommandType.Text;
            OracleDataReader rrd = cmds.ExecuteReader();
            DataTable vd = new DataTable();
            vd.Load(rrd);
            dataGrid.ItemsSource = vd.DefaultView;
            rrd.Close();
        }


        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidventa.Text = dr["ID_PEDIDO"].ToString();
                cboproductos.Text = dr["nombre_producto"].ToString();
                txtcantidadProducto.Text = dr["CANTIDAD"].ToString();





            }
        }

        
    }

}
