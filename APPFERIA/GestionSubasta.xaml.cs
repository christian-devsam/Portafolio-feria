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
                    cmd.Parameters.Add("ID_SUBASTA", OracleDbType.Int32, 6).Value = txtidSubasta.Text;
                    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("FECHA_TERMINO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = Convert.ToInt32(cboIdVenta.SelectedValue);
                    cmd.Parameters.Add("ID_ESTADO", OracleDbType.Int32, 6).Value = Convert.ToInt32(cboEstado.SelectedValue);

                    break;
                case 1:
                    msg = "Subasta Modificada!";
                    
                    cmd.Parameters.Add("FECHA_TERMINO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = cboIdVenta.SelectedItem;
                    cmd.Parameters.Add("ID_ESTADO", OracleDbType.Int32, 6).Value = cboEstado.SelectedItem;
                    cmd.Parameters.Add("ID_SUBASTA", OracleDbType.Int32, 6).Value = txtidSubasta.Text;

                    break;
                case 2:
                    msg = "Subasta Eliminada!";
                    cmd.Parameters.Add("ID_SUBASTA", OracleDbType.Int32, 6).Value = txtidSubasta.Text;

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
                String sql = "INSERT INTO SUBASTA (ID_SUBASTA, FECHA_INGRESO,FECHA_TERMINO, ID_PEDIDO, ID_ESTADO )" + " VALUES(:ID_SUBASTA, :FECHA_INGRESO, :FECHA_TERMINO, :ID_PEDIDO, :ID_ESTADO )";
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
                String sql = "UPDATE SUBASTA SET FECHA_TERMINO = :FECHA_TERMINO, ID_PEDIDO = :ID_PEDIDO, ID_ESTADO = :ID_ESTADO " + "WHERE ID_SUBASTA = :ID_SUBASTA";
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
                String sql = "DELETE FROM SUBASTA " +
                            "WHERE ID_SUBASTA = :ID_SUBASTA";
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
            txtidSubasta.Text = "";
            DpickerFinal.SelectedDate.ToString();
            cboIdVenta.SelectedValue = "";
            cboEstado.SelectedValue = "";
            

        }

        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM SUBASTA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }


        

        private void cboIdVenta_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd2 = new OracleCommand("SELECT ID_PEDIDO FROM PEDIDO", con);

            OracleDataReader rd = cmd2.ExecuteReader();
            while (rd.Read())
            {
                cboIdVenta.Items.Add(rd["ID_PEDIDO"].ToString());
            }
        }

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidSubasta.Text = dr["ID_SUBASTA"].ToString();
                cboIdVenta.SelectedValue = dr["ID_PEDIDO"].ToString();
                cboEstado.SelectedValue = dr["ID_ESTADO"].ToString();
                DpickerFinal.SelectedDate = Convert.ToDateTime(dr["FECHA_TERMINO"]);
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
            VisualizarPujas v1 = new VisualizarPujas();
            this.Close();
            v1.ShowDialog();
        }

        

        private void cboEstado_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("SELECT ID_ESTADO, DESCRIPCION FROM ESTADO_SUBASTA", con);

            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboEstado.ItemsSource = data;

            cboEstado.DisplayMemberPath = dt.Columns["DESCRIPCION"].ToString();
            cboEstado.SelectedValuePath = dt.Columns["ID_ESTADO"].ToString();
        }

        
    }
}
