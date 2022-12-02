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

                    cmd.Parameters.Add("RUT_PRO", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("NOMBRES", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO_PAT", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("APELLIDO_MAT", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 70).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 38).Value = int.Parse(txtTelefono.Text);
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("FECHA_CREACION", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 50).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("ID_CONTRATO", OracleDbType.Int32, 6).Value = int.Parse(txtidpro.Text);
                    


                    break;
                case 1:
                    msg = "Productor modificado!";

                    
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO_PAT", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("APELLIDO_MAT", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 70).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 38).Value = int.Parse(txtTelefono.Text);
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 20).Value = txtEmail.Text;
                    cmd.Parameters.Add("FECHA_CREACION", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtEmail.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("ID_CONTRATO", OracleDbType.Int32, 6).Value = int.Parse(txtidpro.Text);
                    cmd.Parameters.Add("RUT_PRO", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;


                    break;
                case 2:
                    msg = "Productor Eliminado!";

                    cmd.Parameters.Add("RUT_PRO", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;

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
            String sql = "INSERT INTO PRODUCTOR (RUT_PRO, NOMBRES, APELLIDO_PAT, APELLIDO_MAT, DIRECCION, TELEFONO, CORREO, FECHA_CREACION, USUARIO, CONTRASENIA, ID_PRODUCTOR )" + "VALUES( :RUT_PRO, :NOMBRES, :APELLIDO_PAT, :APELLIDO_MAT, :DIRECCION, :TELEFONO, :CORREO, :FECHA_CREACION, :USUARIO, :CONTRASENIA, :ID_PRODUCTOR )";
            this.AUD(sql, 0);
            this.listar();
            this.limpiar();

        }

        private void btnEliminarProductor_Click(object sender, RoutedEventArgs e)
        {
            String sql = "DELETE FROM PRODUCTOR " +
              "WHERE RUT_PRO = :RUT_PRO";
            this.AUD(sql, 2);
            this.limpiar();
        }

        private void btnActualizarProductor_Click(object sender, RoutedEventArgs e)
        {
            String sql = "UPDATE PRODUCTOR SET NOMBRES = :NOMBRES," + "APELLIDO_PAT =:APELLIDO_PAT, APELLIDO_MAT =:APELLIDO_MAT, DIRECCION =:DIRECCION, TELEFONO =:TELEFONO, CORREO =:CORREO, FECHA_CREACION=:FECHA_CREACION, USUARIO =: USUARIO, CONTRASENIA =: CONTRASENIA, ID_PRODUCTOR =: ID_PRODUCTOR " +
                        "WHERE RUT_PRO = :RUT_PRO";
            this.AUD(sql, 1);
            this.listar();
            this.limpiar();
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

            cmd.CommandText = "SELECT * FROM PRODUCTOR";
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
                txtRutProductor.Text = dr["RUT_PRO"].ToString();
                txtNombreProductor.Text = dr["NOMBRES"].ToString();
                txtApellidos.Text = dr["APELLIDO_PAT"].ToString();
                txtApellidos.Text = dr["APELLIDO_MAT"].ToString();
                txtDireccion.Text = dr["DIRECCION"].ToString();
                txtTelefono.Text = dr["TELEFONO"].ToString();
                txtEmail.Text = dr["CORREO"].ToString();



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

        

        private void txtidpro_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(ID_PRODUCTOR)+1 as ID_PRODUCTOR from PRODUCTOR", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidpro.Text = dt.Rows[0][0].ToString();
        }
    }
}
