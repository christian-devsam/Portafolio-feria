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
using MahApps.Metro.Controls;
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
    /// Lógica de interacción para GestionContrato.xaml
    /// </summary>
    public partial class GestionContrato
    {
        


        OracleConnection con = null;
        public GestionContrato()
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

                MessageBox.Show("error");
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
                    msg = "Contrato Agregado!";
                    cmd.Parameters.Add("ID_CONTRATO", OracleDbType.Int32, 6).Value = int.Parse(txtidContrato.Text);
                    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("FECHA_TERMINO", OracleDbType.Date).Value = DpickerTermino.SelectedDate;
                    cmd.Parameters.Add("VIGENCIA", OracleDbType.Char).Value = 1;
                    cmd.Parameters.Add("RUT_PRO", OracleDbType.Varchar2, 20).Value = cboNombreProductor.SelectedItem;
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedItem;
                    cmd.Parameters.Add("OBSERVACIONES", OracleDbType.Varchar2, 255).Value = txtObservaciones.Text;
                    break;

                case 1:
                    msg = "Contrato modificado! ";
                    
                    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("FECHA_TERMINO", OracleDbType.Date).Value = DpickerTermino.SelectedDate;
                    cmd.Parameters.Add("VIGENCIA", OracleDbType.Char).Value = true;
                    cmd.Parameters.Add("RUT_PRO", OracleDbType.Varchar2, 20).Value = cboNombreProductor.SelectedItem;
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedItem;
                    cmd.Parameters.Add("OBSERVACIONES", OracleDbType.Varchar2, 255).Value = txtObservaciones.Text;
                    cmd.Parameters.Add("ID_CONTRATO", OracleDbType.Int32, 6).Value = int.Parse(txtidContrato.Text);

                    break;
                case 2:
                    msg = "Contrato eliminado !";
                    cmd.Parameters.Add("ID_CONTRATO", OracleDbType.Int32, 6).Value = int.Parse(txtidContrato.Text);

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

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO CONTRATO (ID_CONTRATO, FECHA_INICIO, FECHA_TERMINO, VIGENCIA, RUT_PRO, RUT_CLI, OBSERVACIONES )" + " VALUES(:ID_CONTRATO, :FECHA_INICIO, :FECHA_TERMINO, :VIGENCIA, :RUT_PRO, :RUT_CLI, :OBSERVACIONES )";
                this.AUD(sql, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
            
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                String sql = "UPDATE CONTRATO SET FECHA_INICIO = :FECHA_INICIO," + "FECHA_TERMINO = :FECHA_TERMINO, VIGENCIA = :VIGENCIA, RUT_PRO = :RUT_PRO, RUT_CLI = :RUT_CLI, OBSERVACIONES = :OBSERVACIONES " + "WHERE ID_CONTRATO = :ID_CONTRATO";
                this.AUD(sql, 1);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnTerminarContrato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "DELETE FROM CONTRATO " +
                "WHERE ID_CONTRATO = :ID_CONTRATO";
                this.AUD(sql, 2);
                this.Limpiar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void listar()
        {
            OracleCommand cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM CONTRATO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvListado.ItemsSource = dt.DefaultView;
            dr.Close();
        }

        private void Limpiar()
        {
            txtidContrato.Text = "";
            cboNombreProductor.SelectedValue = "";
            cboNombreCliente.SelectedValue = "";
            DpickerTermino.ToString();
            txtObservaciones.Text = "";
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        

        

        private void btnSalirGestionContrato_Click(object sender, RoutedEventArgs e)
        {
            MainWindow v1 = new MainWindow();
            this.Close();
            v1.ShowDialog();
        }


        private void btnListaContratos_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
            listar();
        }

        private void gestionContrato_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = new OracleCommand("SELECT RUT_PRO FROM PRODUCTOR", con);
            OracleDataReader registro = cmd.ExecuteReader();
            while (registro.Read())
            {
                cboNombreProductor.Items.Add(registro["RUT_PRO"].ToString());

            }

        }

        private void cboNombreCliente_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("SELECT RUT_CLI FROM CLIENTE", con);

            OracleDataReader re = cmd1.ExecuteReader();
            while (re.Read())
            {

                cboNombreCliente.Items.Add(re["RUT_CLI"].ToString());

            }
        }




        private void dgvListado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtidContrato.Text = dr["CODIGO"].ToString();
                cboNombreCliente.SelectedValue = dr["RUT_CLI"].ToString();
                cboNombreProductor.SelectedValue = dr["RUT_PRO"].ToString();
                DpickerTermino.SelectedDate = Convert.ToDateTime(dr["TERMINO"]);
                txtObservaciones.Text = dr["OBS"].ToString();

                btnAgregar.IsEnabled = false;
                btnTerminar.IsEnabled = true;
                btnActualizar.IsEnabled = true;


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

        private void enviarCorreo(string to, string idcontrato, string rutcliente, string rutpro, string termino, string obs)
        {
            
            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add(to); //Correo destino?
            correo.Subject = "Informacion"; //Asunto
            correo.Body = "Codigo Contrato : "+ idcontrato + "\n  "; //Mensaje del correo
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com"; //Host del servidor de correo
            smtp.Port = 25; //Puerto de salida
            smtp.Credentials = new System.Net.NetworkCredential("feriavirtualmg1@gmail.com", "hantmgmsgkvsljkm");//Cuenta de correo
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtp.EnableSsl = true;//True si el servidor de correo permite ssl

            try
            {
                smtp.Send(correo);
                MessageBox.Show("Correo enviado exitosamente");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnenviarCorreo_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = con.CreateCommand();
            
            cmd.CommandText = "SELECT * FROM CLIENTE WHERE RUT_CLI = "+ "'"+ cboNombreCliente.SelectedItem +"'"; //query
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            string correo = dt.Rows[0]["CORREO"].ToString();
            dr.Close();
            //corremos el metodo 
            enviarCorreo(correo, txtidContrato.Text, cboNombreCliente.SelectedItem.ToString(), cboNombreProductor.SelectedItem.ToString(), DpickerTermino.Text, txtObservaciones.Text);
        }

        private void txtidContrato_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(ID_CONTRATO)+1 as ID_CONTRATO from CONTRATO", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidContrato.Text = dt.Rows[0][0].ToString();
        }

        
    }
        
}

