using System;
using bibliotecaFeria;
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
    /// Lógica de interacción para GestionVenta.xaml
    /// </summary>
    public partial class GestionVenta
    {

        OracleConnection con = null;

        public GestionVenta()
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
        private string AUD(String sql_stmt, int state)
        {
            string msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;
            switch (state)
            {
                case 0:
                    msg = "Ingrese los Productos";
                    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);
                    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    cmd.Parameters.Add("FECHA_ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedValue.ToString();


                    

                    break;
                //case 1:
                //    msg = "Venta modificado!";
                //    cmd.Parameters.Add("RUT_CLI", OracleDbType.Varchar2, 20).Value = cboNombreCliente.SelectedValue.ToString();
                //    cmd.Parameters.Add("FECHA_INGRESO", OracleDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //    cmd.Parameters.Add("FECHA_ENVIO", OracleDbType.Date).Value = DpickerFinal.SelectedDate;
                //    cmd.Parameters.Add("ID_PEDIDO", OracleDbType.Int32, 6).Value = int.Parse(txtidventa.Text);

                //    break;
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

        

        private void BtnAgregarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String sql = "INSERT INTO PEDIDO (ID_PEDIDO, FECHA_INGRESO, FECHA_ENVIO, RUT_CLI)" + "VALUES(:ID_PEDIDO, :FECHA_INGRESO, :FECHA_ENVIO, :RUT_CLI )";
                this.AUD(sql, 0);

                ADMDetallePedido verdetalle = new ADMDetallePedido();
                this.Close();
                verdetalle.txtidventa.Text = txtidventa.Text;
                verdetalle.ShowDialog();
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void btnActualizarVenta_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        String sql = "UPDATE PEDIDO SET RUT_CLI = :RUT_CLI," + "FECHA_INGRESO = :FECHA_INGRESO, FECHA_ENVIO = :FECHA_ENVIO " + "WHERE ID_PEDIDO = :ID_PEDIDO";
        //        string respueta = this.AUD(sql, 1);

        //        if (respueta == "Venta modificado!")
        //        {
        //            ADMDetallePedido admi = new ADMDetallePedido();
        //            this.Close();
        //            admi.txtidventa.Text = txtidventa.Text;
        //            admi.ShowDialog();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void btnTerminarVenta_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        String sql = "DELETE FROM PEDIDO " + "WHERE ID_PEDIDO = :ID_PEDIDO";
        //        string respuesta = this.AUD(sql, 2);
        //        this.limpiar();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void btnSalirGestionVenta_Click(object sender, RoutedEventArgs e)
        {
            VisualizarVentas volver = new VisualizarVentas();
            this.Close();
            volver.ShowDialog();
        }

        private void limpiar()
        {
            txtidventa.Text = "";
            cboNombreCliente.Text = "";
            DpickerFinal.ToString();
        }
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
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

        private void cboNombreCliente_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd3 = new OracleCommand("SELECT RUT_CLI, NOMBRE FROM CLIENTE", con);

            OracleDataReader rw = cmd3.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rw);
            var data = (dt as System.ComponentModel.IListSource).GetList();

            cboNombreCliente.ItemsSource = data;

            cboNombreCliente.DisplayMemberPath = dt.Columns["NOMBRE"].ToString();
            cboNombreCliente.SelectedValuePath = dt.Columns["RUT_CLI"].ToString();
        }

        

        private void txtidventa_Loaded(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd1 = new OracleCommand("select max(id_pedido)+1 as id_pedido from pedido", con);
            OracleDataReader re = cmd1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(re);
            txtidventa.Text = dt.Rows[0][0].ToString();
        }

        




        //private void enviarCorreo(string to, string idventa, string nombrecli, string html, string canpro, string envio)
        //{

        //    System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
        //    correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
        //    correo.To.Add(to); //Correo destino?
        //    correo.Subject = "Boleta Cliente Feria Virtual"; //Asunto
        //    correo.Body = "Codigo de la Venta : " + idventa + "  Nombre del cliente : " + nombrecli + " " + html + " " + canpro + " Fecha de Envio : " + envio ; //Mensaje del correo
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

        //private void btn_ClickEnviarCorreo(object sender, RoutedEventArgs e)
        //{
        //    OracleCommand cmd = con.CreateCommand();

        //    cmd.CommandText = "SELECT * FROM CLIENTE WHERE RUT_CLI = " + "'" + cboNombreCliente.SelectedValue + "'"; //query
        //    cmd.CommandType = CommandType.Text;
        //    OracleDataReader dr = cmd.ExecuteReader();
        //    DataTable dt = new DataTable();
        //    dt.Load(dr);
        //    string correo = dt.Rows[0]["CORREO"].ToString();
        //    dr.Close();

        //    OracleCommand cmd2 = con.CreateCommand();

        //    cmd2.CommandText = "SELECT * FROM detalle_pedido d inner join pedido pe on pe.id_pedido = d.id_pedido inner join producto p on p.id_producto = d.id_producto where pe.id_pedido =" + "'" + txtidventa.Text + "'";
        //    cmd2.CommandType = CommandType.Text;
        //    OracleDataReader dr2 = cmd2.ExecuteReader();
        //    DataTable dt2 = new DataTable();
        //    dt2.Load(dr2);

        //    string html = "";

        //    html += "<table style='border: white 5px solid; width:500px'>";
        //    html += "<thead><tr><td>Cantidad</td><td>Nombre Producto</td><td>Valor</td></tr></thead>";
        //    html += "<tbody>";
        //    for (int i = 0; i < dt2.Rows.Count; i++)
        //    {
        //        html += "<tr style='border: white 5px solid;'><td>" + dt2.Rows[i]["CANTIDAD"].ToString() + "</td>";
        //         html += "<td>" + dt2.Rows[i]["NOMBRE_PRODUCTO"].ToString() + "</td><td>" + dt2.Rows[i]["VALOR"].ToString() + "</td></tr>";
        //    }
        //    html += "</tbody>";
        //    html += "</table>";


        //    //metodo enviar correo
        //    enviarCorreo(correo, txtidventa.Text, cboNombreCliente.Text, html, txtcantidadProducto.Text, dt2.Rows[0]["FECHA_ENVIO"].ToString());

        //}



    }
}
