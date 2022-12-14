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
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;

using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal 
    {
        //PATRON SINGLETON
        public static MainWindow ventana;
        public static MainWindow getInstance()
        {
            if (ventana == null)
            {
                ventana = new MainWindow();
            }
            return ventana;
        }

        ClienteCollection coleccion = new ClienteCollection(); //creamos una variable de la clase
        OracleConnection con = null;
        String mail;

        public MenuPrincipal()
        {
            abrirConexion();
            InitializeComponent();
            dgvListado.CanUserAddRows = false; //evita que aparezca una fila vacia cuando se inicia el proyecto
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

            //Cliente clie = new Cliente();
            //clie.Rut = txtRutCliente.Text;

            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Cliente agregado";
                    
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = txtRutCliente.Text;
                    cmd.Parameters.Add("ID_TIPOCLIENTE", OracleDbType.Int32, 10).Value = cboTipoCliente.SelectedValue;
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreCliente.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidoPatCliente.Text;
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 50).Value = txtDireccionCliente.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 10).Value = int.Parse(txtTelefonoCliente.Text);
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtUsuario.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtContrasenia.Text;
                    cmd.Parameters.Add("ID_CLIENTE", OracleDbType.Int32, 10).Value = int.Parse(txtidcliente.Text);
                    cmd.Parameters.Add("ID_PAIS", OracleDbType.Int32, 10).Value = 1;
                    cmd.Parameters.Add("REGION_ID", OracleDbType.Int32, 10).Value = cboregion.SelectedValue;
                    cmd.Parameters.Add("PROVINCIA_ID", OracleDbType.Int32, 10).Value = cboprovincia.SelectedValue;
                    cmd.Parameters.Add("ID_COMUNA", OracleDbType.Int32, 10).Value = cboComuna.SelectedValue;

                    break;
                case 1:
                    msg = "Cliente modificado!";

                    cmd.Parameters.Add("ID_TIPOCLIENTE", OracleDbType.Int32, 10).Value = cboTipoCliente.SelectedValue;
                    cmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2, 50).Value = txtNombreCliente.Text;
                    cmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2, 50).Value = txtApellidoPatCliente.Text;                   
                    cmd.Parameters.Add("DIRECCION", OracleDbType.Varchar2, 50).Value = txtDireccionCliente.Text;
                    cmd.Parameters.Add("TELEFONO", OracleDbType.Int32, 10).Value = int.Parse(txtTelefonoCliente.Text);                    
                    cmd.Parameters.Add("CORREO", OracleDbType.Varchar2, 50).Value = txtEmail.Text;                    
                    cmd.Parameters.Add("USUARIO", OracleDbType.Varchar2, 20).Value = txtUsuario.Text;
                    cmd.Parameters.Add("CONTRASENIA", OracleDbType.Varchar2, 20).Value = txtContrasenia.Text;
                    cmd.Parameters.Add("ID_CLIENTE", OracleDbType.Int32, 10).Value = int.Parse(txtidcliente.Text);
                    cmd.Parameters.Add("ID_PAIS", OracleDbType.Int32, 10).Value = 1;
                    cmd.Parameters.Add("REGION_ID", OracleDbType.Int32, 10).Value = cboregion.SelectedValue;
                    cmd.Parameters.Add("PROVINCIA_ID", OracleDbType.Int32, 10).Value = cboprovincia.SelectedValue;
                    cmd.Parameters.Add("ID_COMUNA", OracleDbType.Int32, 10).Value = cboComuna.SelectedValue;
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = txtRutCliente.Text;
                    


                    break;
                case 2:
                    msg = "Cliente Eliminado!";

                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 50).Value = txtRutCliente.Text;

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
            catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
        
        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            String sql = "INSERT INTO CLIENTE (RUT_CLI, ID_TIPOCLIENTE, NOMBRE, APELLIDO, DIRECCION, TELEFONO, CORREO, USUARIO, CONTRASENIA, ID_CLIENTE, ID_PAIS, REGION_ID, PROVINCIA_ID, ID_COMUNA )" + "VALUES( :RUT_CLI, :ID_TIPOCLIENTE, :NOMBRE, :APELLIDO, :DIRECCION, :TELEFONO, :CORREO, :USUARIO, :CONTRASENIA, :ID_CLIENTE, :ID_PAIS, :REGION_ID, :PROVINCIA_ID, :ID_COMUNA)";
            this.AUD(sql, 0);

            btnAgregar.IsEnabled = false;
            btnTerminar.IsEnabled = true;
            btnActualizar.IsEnabled = true;

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
                String sql = "DELETE FROM CLIENTE " +
                            "WHERE RUT_CLI = :RUT_CLI";
                this.AUD(sql, 2);
                this.limpiar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "UPDATE CLIENTE SET ID_TIPOCLIENTE = :ID_TIPOCLIENTE," + "NOMBRE =:NOMBRE, APELLIDO =:APELLIDO, DIRECCION =:DIRECCION, TELEFONO =:TELEFONO, CORREO =:CORREO, USUARIO =:USUARIO, CONTRASENIA=:CONTRASENIA, ID_CLIENTE =: ID_CLIENTE, ID_PAIS =:ID_PAIS, REGION_ID =:REGION_ID, PROVINCIA_ID=:PROVINCIA_ID, ID_COMUNA=:ID_COMUNA " +
                            "WHERE RUT_CLI = :RUT_CLI";
                this.AUD(sql, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            MainWindow v1 = new MainWindow();
            this.Hide();
            v1.ShowDialog();

        }
       

        private void dgvListado_Loaded(object sender, RoutedEventArgs e)
        {
            this.listar();
        }
        private void listar()
        {
                OracleCommand cmd = con.CreateCommand();
        
                cmd.CommandText = "SELECT * FROM CLIENTE ";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dgvListado.ItemsSource = dt.DefaultView;
                dr.Close();
                //dgvListado.ItemsSource = coleccion.listar(); //cargar la lista sobre la grilla 
                //dgvListado.Items.Refresh(); //refrezca la información presentada en la grilla

            }
       

        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtRutCliente.Text = dr["RUT_CLI"].ToString();
                cboTipoCliente.SelectedValue = dr["ID_TIPOCLIENTE"].ToString();
                txtNombreCliente.Text = dr["NOMBRE"].ToString();
                txtApellidoPatCliente.Text = dr["APELLIDO"].ToString();
                txtDireccionCliente.Text = dr["DIRECCION"].ToString();
                txtTelefonoCliente.Text = dr["TELEFONO"].ToString();
                txtEmail.Text = dr["CORREO"].ToString();
                cboComuna.Text = dr["ID_COMUNA"].ToString();
                txtUsuario.Text = dr["USUARIO"].ToString();
                txtContrasenia.Text = dr["CONTRASENIA"].ToString();
                txtidcliente.Text = dr["ID_CLIENTE"].ToString();


                

            }
        }
         private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }
        private void limpiar()
        {
            
            txtRutCliente.Text = "";
            cboTipoCliente.Text = "";
            txtNombreCliente.Text = "";
            txtApellidoPatCliente.Text = "";
            txtTelefonoCliente.Text = "";
            txtDireccionCliente.Text = "";
            cboComuna.Text = "";
            txtEmail.Text = "";
            txtUsuario.Text = "";
            txtContrasenia.Text = "";
            cboregion.Text = "";
            cboprovincia.Text = "";
            cboComuna.Text = "";


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

        //private void enviarCorreo()
        //{
        //    System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
        //    correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
        //    correo.To.Add(txtEmail.Text); //Correo destino?
        //    correo.Subject = "Informacion"; //Asunto
        //    correo.Body = "¡ FELICIDADES ! El cliente se registro exitosamente en la feria virtual de maipo grande."; //Mensaje del correo
        //    correo.IsBodyHtml = true;
        //    correo.Priority = MailPriority.Normal;
        //    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
        //    smtp.Port = 25; //Puerto de salida
        //    smtp.Credentials = new System.Net.NetworkCredential("feriavirtualmg1@gmail.com", "hantmgmsgkvsljkm");//Cuenta de correo
        //    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        //    smtp.EnableSsl = true;//True si el servidor de correo permite ssl
            
        //    try
        //    {
        //        smtp.Send(correo);
        //        MessageBox.Show("Correo enviado exitosamente");
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void cboTipoCliente_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("SELECT ID_TIPOCLIENTE, DESCRIPCION FROM TIPO_CLIENTE", con);

            OracleDataReader re = cmd1.ExecuteReader();
        
            DataTable dt = new DataTable();
            dt.Load(re);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboTipoCliente.ItemsSource = data;

            cboTipoCliente.DisplayMemberPath = dt.Columns["DESCRIPCION"].ToString();
            cboTipoCliente.SelectedValuePath = dt.Columns["ID_TIPOCLIENTE"].ToString();
            //cboTipoCliente.ItemsSource = data;
            //cbox_alumnos.ItemsSource = ds.Tables["Alumno"].DefaultView;
            //cbox_alumnos.DisplayMemberPath = ds.Tables["Alumno"].Columns["Matricula_A"].ToString();
            //cbox_alumnos.SelectedValuePath = ds.Tables["Alumno"].Columns["ID_Alumno"].ToString();
        }

        private void cboregion_Loaded(object sender, RoutedEventArgs e)
        {
            cargarRegion();
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

        public void cargarProvincia()
        {
            OracleCommand cmd2 = new OracleCommand("SELECT * from PROVINCIA where region_id ="+ "'"+cboregion.SelectedValue+"'", con);
            OracleDataReader rd = cmd2.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(rd);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboprovincia.ItemsSource = data;

            cboprovincia.DisplayMemberPath = dt.Columns["PROVINCIA"].ToString();
            cboprovincia.SelectedValuePath = dt.Columns["PROVINCIA_ID"].ToString();
        }
        public void cargarComuna()
        {
            OracleCommand cmd2 = new OracleCommand("SELECT * from COMUNA where provincia_id =" + "'" + cboprovincia.SelectedValue + "'", con);
            OracleDataReader red = cmd2.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(red);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboComuna.ItemsSource = data;

            cboComuna.DisplayMemberPath = dt.Columns["COMUNA"].ToString();
            cboComuna.SelectedValuePath = dt.Columns["ID_COMUNA"].ToString();
        }

        private void cboregion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cargarProvincia();
        }

        private void cboprovincia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cargarComuna();
        }

        private void txtidcliente_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(ID_CLIENTE)+1 as ID_CLIENTE from CLIENTE", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidcliente.Text = dt.Rows[0][0].ToString();
        }
    }
}


