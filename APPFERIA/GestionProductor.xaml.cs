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
using System.Windows.Shapes;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GestionProductor.xaml
    /// </summary>
    public partial class GestionProductor
    {
        
        OracleConnection con = null;
        public GestionProductor()
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

                MessageBox.Show("Error");
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
                    msg = "Productor agregado!";
                    cmd.Parameters.Add("RUT_DIS", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("NOMBRE_DIS", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO_DIS", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION_DIS", OracleDbType.Varchar2, 50).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO_DIS", OracleDbType.Int32, 38).Value = txtTelefono.Text;
                    cmd.Parameters.Add("CORREO_DIS", OracleDbType.Varchar2, 50).Value = txtEmail.Text;



                    break;
                case 1:
                    msg = "Productor modificado!";

                    cmd.Parameters.Add("NOMBRE_DIS", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO_DIS", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION_DIS", OracleDbType.Varchar2, 50).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO_DIS", OracleDbType.Int32, 38).Value = int.Parse(txtTelefono.Text);
                    cmd.Parameters.Add("CORREO_DIS", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("RUT_DIS", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;



                    break;
                case 2:
                    msg = "Productor Eliminado!";

                    cmd.Parameters.Add("RUT_DIS", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;

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
            catch (Exception expe) { }
        }

        private void btnGuardarProductor_Click(object sender, RoutedEventArgs e)
        {
            String sql = "INSERT INTO DISTRIBUIDOR (RUT_DIS, NOMBRE_DIS, APELLIDO_DIS, DIRECCION_DIS, TELEFONO_DIS, CORREO_DIS )" + "VALUES( :RUT_DIS, :NOMBRE_DIS, :APELLIDO_DIS, :DIRECCION_DIS, :TELEFONO_DIS, :CORREO_DIS )";
            this.AUD(sql, 0);

            btnGuardarProductor.IsEnabled = false;
            btnEliminarProductor.IsEnabled = true;
            btnActualizarProductor.IsEnabled = true;
        }

        private void btnEliminarProductor_Click(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM DISTRIBUIDOR " +
              "WHERE RUT_DIS = :RUT_DIS";
            this.AUD(sql, 2);
            this.limpiar();
        }

        private void btnActualizarProductor_Click(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE DISTRIBUIDOR SET NOMBRE_DIS = :NOMBRE_DIS," + "APELLIDO_DIS =:APELLIDO_DIS, DIRECCION_DIS =:DIRECCION_DIS, TELEFONO_DIS =:TELEFONO_DIS, CORREO_DIS =:CORREO_DIS " +
                        "WHERE RUT_DIS = :RUT_DIS";
            this.AUD(sql, 1);
        }

        private void limpiar()
        {
            txtRutProductor.Text = "";
            txtNombreProductor.Text = "";
            txtApellidos.Text = "";
            txtDireccion.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";


        }

        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM DISTRIBUIDOR";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        

        

        

        private void btnSalirProductor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow v1 = new MainWindow();
            this.Close();
            v1.ShowDialog();
        }

        private void btnLimpiarProductor_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtRutProductor.Text = dr["RUT_DIS"].ToString();
                txtNombreProductor.Text = dr["NOMBRE_DIS"].ToString();
                txtApellidos.Text = dr["APELLIDO_DIS"].ToString();
                txtDireccion.Text = dr["DIRECCION_DIS"].ToString();
                txtTelefono.Text = dr["TELEFONO_DIS"].ToString();
                txtEmail.Text = dr["CORREO_DIS"].ToString();


                btnGuardarProductor.IsEnabled = false;
                btnEliminarProductor.IsEnabled = true;
                btnActualizarProductor.IsEnabled = true;


            }
        }

        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
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
    }
}
