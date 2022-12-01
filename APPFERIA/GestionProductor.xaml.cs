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
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 70).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 38).Value = int.Parse(txtTelefono.Text);
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 50).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("REGION_ID", OracleDbType.Varchar2, 50).Value = cboregion.SelectedValue.ToString();


                    break;
                case 1:
                    msg = "Productor modificado!";

                    
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreProductor.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidos.Text;
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 70).Value = txtDireccion.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 38).Value = int.Parse(txtTelefono.Text);
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 20).Value = txtEmail.Text;
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtEmail.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtRutProductor.Text;
                    cmd.Parameters.Add("REGION_ID", OracleDbType.Int32, 6).Value = cboregion.SelectedValue.ToString();
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
            String sql = "INSERT INTO PRODUCTOR (RUT_PRO, NOMBRE, APELLIDO, DIRECCION, TELEFONO, CORREO, USUARIO, CONTRASENIA, REGION_ID )" + "VALUES( :RUT_PRO, :NOMBRE, :APELLIDO, :DIRECCION, :TELEFONO, :CORREO, :USUARIO, :CONTRASENIA, :REGION_ID )";
            this.AUD(sql, 0);

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
            String sql = "UPDATE PRODUCTOR SET NOMBRE = :NOMBRE," + "APELLIDO =:APELLIDO, DIRECCION =:DIRECCION, TELEFONO =:TELEFONO, CORREO =:CORREO, USUARIO =: USUARIO, CONTRASENIA =: CONTRASENIA, REGION_ID =: REGION_ID " +
                        "WHERE RUT_PRO = :RUT_PRO";
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
            cboregion.Text = "";


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
                txtNombreProductor.Text = dr["NOMBRE"].ToString();
                txtApellidos.Text = dr["APELLIDO"].ToString();
                txtDireccion.Text = dr["DIRECCION"].ToString();
                txtTelefono.Text = dr["TELEFONO"].ToString();
                txtEmail.Text = dr["CORREO"].ToString();
                cboregion.Text = dr["REGION_ID"].ToString();



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

        private void cboregion_Loaded(object sender, RoutedEventArgs e)
        {
            this.cargarRegion();
        }
        public void cargarRegion()
        {
            OracleCommand cmd1 = new OracleCommand("SELECT * from region", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboregion.ItemsSource = data;

            cboregion.DisplayMemberPath = dt.Columns["REGION"].ToString();
            cboregion.SelectedValuePath = dt.Columns["REGION_ID"].ToString();
        }
    }
}
