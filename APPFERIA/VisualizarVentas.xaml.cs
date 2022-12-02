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
    /// Lógica de interacción para VisualizarVentas.xaml
    /// </summary>
    public partial class VisualizarVentas : Window
    {
        OracleConnection con = null;
        public VisualizarVentas()
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
        private void limpiar()
        {
            dataGrid.Columns.Clear();
        }
        private void limpiarid()
        {
            txtidventa.Text = "";
        }
        public void llenarProductos(int idventa)
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT DISTINCT(d.id_pedido), c.nombre, pe.fecha_envio FROM detalle_pedido d inner join pedido pe on pe.id_pedido = d.id_pedido inner join producto p on p.id_producto = d.id_producto  join cliente c on pe.rut_cli=c.rut_cli where pe.id_pedido = " + "'" + idventa + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
            OracleCommand cmr = con.CreateCommand();

            cmr.CommandText = "SELECT d.id_pedido, p.nombre_producto, d.cantidad, d.valor FROM detalle_pedido d inner join pedido pe on pe.id_pedido = d.id_pedido inner join producto p on p.id_producto = d.id_producto  join cliente c on pe.rut_cli=c.rut_cli where pe.id_pedido = " + "'" + idventa + "'";
            cmr.CommandType = CommandType.Text;
            OracleDataReader rr = cmr.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(rr);
            dataGriddetalle.ItemsSource = dt2.DefaultView;
            rr.Close();


        }

        
        public void listarPedido()
        {
            OracleCommand cmd1 = con.CreateCommand();

            cmd1.CommandText = "SELECT p.id_pedido, p.fecha_ingreso, p.fecha_envio, c.rut_cli, c.nombre||' '||c.apellido as NOMBRE_CLIENTE from pedido p INNER JOIN CLIENTE C on c.rut_cli = p.rut_cli where p.id_pedido = " + "'" + txtidventa.Text + "'";
            cmd1.CommandType = CommandType.Text;
            OracleDataReader dg = cmd1.ExecuteReader();
            DataTable db = new DataTable();
            db.Load(dg);
            dataGrid.ItemsSource = db.DefaultView;
            dg.Close();
        }
        public void listarDetalle()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT d.id_pedido, p.nombre_producto, d.cantidad, d.valor FROM DETALLE_PEDIDO D INNER JOIN PRODUCTO P ON P.ID_PRODUCTO = D.ID_PRODUCTO where d.id_pedido = " + "'" + txtidventa.Text + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGriddetalle.ItemsSource = dt.DefaultView;
            dr.Close();

        }

        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.listarPedido();
        }

        private void dataGriddetalle_Loaded(object sender, RoutedEventArgs e)
        {
            this.listarDetalle();
        }

        private void btnSalirGestionSubasta_Click(object sender, RoutedEventArgs e)
        {
            MainWindow menu = new MainWindow();
            this.Close();
            menu.ShowDialog();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            GestionVenta ven = new GestionVenta();
            this.Close();
            ven.ShowDialog();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (txtidventa.SelectedValue != null)
            {
                String del = "DELETE FROM DETALLE_PEDIDO WHERE ID_PEDIDO = " + "'" + txtidventa.SelectedValue.ToString() + "'";
                String delet = "DELETE FROM PEDIDO WHERE ID_PEDIDO = " + "'" + txtidventa.SelectedValue.ToString() + "'";

                OracleCommand cmd2 = new OracleCommand(del, con);
                OracleCommand cmd1 = new OracleCommand(delet, con);

                
                cmd2.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
                
                MessageBox.Show("pedido numero " + txtidventa.SelectedValue.ToString() + " Eliminado !");

                this.limpiarid();
                MainWindow m = new MainWindow();
                this.Close();
                m.ShowDialog();

                
                //GestionVenta ven = new GestionVenta();
                //this.Close();
                //ven.txtidventa.Text = txtidventa.SelectedValue.ToString();
                //ven.ShowDialog();
            }
            else
            {
                MessageBox.Show("Debe elegir un id venta existente.");
            }
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (txtidventa.SelectedValue != null)
            {
                ADMDetallePedido admde = new ADMDetallePedido();
                this.Close();
                admde.txtidventa.Text = txtidventa.Text;
                admde.ShowDialog();
            }
            else
            {
                MessageBox.Show("Debe elegir un id venta existente.");
            }
        }

        private void txtidventa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //int entero = int.Parse(txtidventa.SelectedValue.ToString());
                if (txtidventa.Text.Length != 0)
                {
                    llenarProductos(int.Parse(txtidventa.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            {

                limpiar();
            }
        }

        private void txtidventa_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("SELECT ID_PEDIDO FROM PEDIDO", con);

            OracleDataReader re = cmd1.ExecuteReader();
            while (re.Read())
            {

                txtidventa.Items.Add(re["ID_PEDIDO"].ToString());
                

            }
        }
    }
}
