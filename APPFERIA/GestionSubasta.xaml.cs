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
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = cboIdVenta.SelectedValue;
                    cmd.Parameters.Add("ID_ESTADO", OracleDbType.Int32, 6).Value = cboEstado.SelectedValue;
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

                // agregar columna correo transportista.
                //enviarcorreo(To, id_subasta)
                if (cboEstado.SelectedValue == "2")
                {
                    string query = "select x.* from(select t.id_transportista, t.nombre, p.valor , c.id_contrato from subasta s ";
                    query += "inner join puja p on s.id_subasta = p.id_subasta ";
                    query += "inner join transportista t on t.id_transportista = p.id_transportista ";
                    query += "inner join contrato_transporte c on c.id_transportista = t.id_transportista ";
                    query += "where s.id_subasta =" + txtidSubasta.Text + " ";
                    query += "order by p.valor asc) x where rownum = 1";




                    OracleCommand cmd = new OracleCommand(query, con);

                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    string correo = dt.Rows[0]["NOMBRE"].ToString();
                   // enviarCorreo(correo,)
                }
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
            cboIdVenta.SelectedItem = "";
            cboEstado.SelectedItem = "";
            

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

        private void txtidSubasta_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(ID_SUBASTA)+1 as ID_SUBASTA from SUBASTA", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidSubasta.Text = dt.Rows[0][0].ToString();
        }

        //private void enviarCorreo(string to, string idcontrato, string rutcliente, string rutpro, string termino, string obs)
        //{

        //    System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
        //    correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
        //    correo.To.Add(to); //Correo destino?
        //    correo.Subject = "Informacion"; //Asunto
        //    correo.Body = "Codigo Contrato : " + idcontrato + "\n  "; //Mensaje del correo
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
    }
}
