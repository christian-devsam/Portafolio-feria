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


using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GestionTransportista.xaml
    /// </summary>
    public partial class GestionTransportista //: Window
    {
        OracleConnection con = null;

        public GestionTransportista()
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
        }

       

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            MainWindow volver = new MainWindow();
            this.Close();
            volver.ShowDialog();
        }


        private String AUD(String sql_stmt, int state)
        {
            string msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Transportista Agregado !";

                    cmd.Parameters.Add("ID_TRANSPORTISTA", OracleDbType.Int32, 20).Value = int.Parse(txtidtransportista.Text);
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreTransportista.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidoTransportista.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 20).Value = int.Parse(txtTelefonotransportista.Text);
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtUsuario.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtContrasenia.Text;
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 20).Value = txtEmailTransportista.Text;
                    cmd.Parameters.Add("ID_LICENCIA", OracleDbType.Int32, 20).Value = cboLicencia.SelectedValue;


                    break;
                case 1:
                    msg = "Transportista Modificado !";
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreTransportista.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidoTransportista.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 20).Value = int.Parse(txtTelefonotransportista.Text);
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtUsuario.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtContrasenia.Text;
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 20).Value = txtEmailTransportista.Text;
                    cmd.Parameters.Add("ID_LICENCIA", OracleDbType.Int32, 20).Value = cboLicencia.SelectedValue;
                    cmd.Parameters.Add("ID_TRANSPORTISTA", OracleDbType.Int32, 20).Value = int.Parse(txtidtransportista.Text);

                    break;
                case 2:
                    msg = "Transportista eliminado!";
                    cmd.Parameters.Add("ID_TRANSPORTISTA", OracleDbType.Int32, 6).Value = int.Parse(txtidtransportista.Text);

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
                return msg;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return ex.Message.ToString();
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO TRANSPORTISTA (ID_TRANSPORTISTA, NOMBRE, APELLIDO, TELEFONO, USUARIO, CONTRASENIA, CORREO, ID_LICENCIA)" + "VALUES(:ID_TRANSPORTISTA, :NOMBRE, :APELLIDO, :TELEFONO , :USUARIO, :CONTRASENIA, :CORREO , :ID_LICENCIA )";
                this.AUD(sql, 0);
                this.limpiar();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        


        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "DELETE FROM TRANSPORTISTA " + "WHERE ID_TRANSPORTISTA = :ID_TRANSPORTISTA";
                this.AUD(sql, 2);
                this.limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            this.limpiar();
        }
        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT ID_TRANSPORTISTA, NOMBRE , TELEFONO, CORREO FROM TRANSPORTISTA";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void limpiar()
        {
            txtidtransportista.Text = "";
            txtNombreTransportista.Text = "";
            txtApellidoTransportista.Text = "";
            txtTelefonotransportista.Text = "";
            cboLicencia.SelectedItem = "";
            txtUsuario.Text = "";
            txtContrasenia.Text = "";
            txtEmailTransportista.Text = "";
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "UPDATE TRANSPORTISTA SET NOMBRE = :NOMBRE," + "APELLIDO = :APELLIDO, TELEFONO = :TELEFONO, USUARIO = :USUARIO, CONTRASENIA = :CONTRASENIA, CORREO = :CORREO, ID_LICENCIA = : ID_LICENCIA " + "WHERE ID_TRANSPORTISTA = :ID_TRANSPORTISTA";
                this.AUD(sql, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtidtransportista_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(ID_TRANSPORTISTA)+1 as ID_TRANSPORTISTA from TRANSPORTISTA", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidtransportista.Text = dt.Rows[0][0].ToString();
        }

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidtransportista.Text = dr["ID_TRANSPORTISTA"].ToString();
                txtNombreTransportista.Text = dr["NOMBRE"].ToString();
                txtApellidoTransportista.Text = dr["APELLIDO"].ToString();
                txtTelefonotransportista.Text = dr["TELEFONO"].ToString();
                cboLicencia.SelectedValue = dr["ID_LICENCIA"].ToString();
                txtUsuario.Text = dr["USUARIO"].ToString();
                txtContrasenia.Text = dr["CONTRASENIA"].ToString();
                txtEmailTransportista.Text = dr["CORREO"].ToString();
            }

        }

        private void cboLicencia_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("SELECT ID_LICENCIA, NOMBRE FROM LICENCIA_CONDUCIR", con);

            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboLicencia.ItemsSource = data;

            cboLicencia.DisplayMemberPath = dt.Columns["NOMBRE"].ToString();
            cboLicencia.SelectedValuePath = dt.Columns["ID_LICENCIA"].ToString();
        }
    }
}
