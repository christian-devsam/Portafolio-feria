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


using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GestionSubasta.xaml
    /// </summary>
    public partial class GestionSubasta
    {
        OracleConnection con = null;
        EnvioCorreo enviarCorreo = new EnvioCorreo();
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
                if (cboEstado.SelectedValue.ToString() == "2")
                {
                    string query = "select x.* from(select t.id_transportista, t.nombre, p.valor , c.id_contrato, t.correo from subasta s ";
                    query += "inner join puja p on s.id_subasta = p.id_subasta ";
                    query += "inner join transportista t on t.id_transportista = p.id_transportista ";
                    query += "inner join contrato_transporte c on c.id_transportista = t.id_transportista ";
                    query += "where s.id_subasta = " + " '" + txtidSubasta.Text + "' ";
                    query += "order by p.valor asc) x where rownum = 1";
                    //query para rescatar correo de transportista que puja mas bajo
                    OracleCommand cmd = new OracleCommand(query, con);

                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    string correotrans = dt.Rows[0]["CORREO"].ToString();




                    // query para contrato_transporte
                    string query1 = "select x.* from(select t.id_transportista, t.NOMBRE||' '||t.APELLIDO as NOMBRE, p.valor , c.id_contrato, t.correo, seg.empresa, ped.fecha_envio, ped.id_pedido from subasta s ";
                    query1 += "inner join puja p on s.id_subasta = p.id_subasta ";
                    query1 += "inner join transportista t on t.id_transportista = p.id_transportista ";
                    query1 += "inner join contrato_transporte c on c.id_transportista = t.id_transportista ";
                    query1 += "inner join seguro seg on seg.id_seguro = c.id_seguro ";
                    query1 += "inner join pedido ped on ped.id_pedido = s.id_pedido ";
                    query1 += "where s.id_subasta = " + " '" + txtidSubasta.Text + "' ";
                    query1 += "order by p.valor asc) x where rownum = 1";

                    OracleCommand cms = new OracleCommand(query1, con);
                    cms.CommandType = CommandType.Text;
                    OracleDataReader rider = cms.ExecuteReader();
                    DataTable tb = new DataTable();
                    tb.Load(rider);
                    //string nombretrans = tb.Rows[0]["NOMBRE"].ToString();
                    //string price = tb.Rows[0]["VALOR"].ToString();
                    //string entrega = tb.Rows[0]["FECHA_ENVIO"].ToString();

                    string html1 = "";

                    html1 += "<table style='border: white 5px solid; width:500px'>";
                    html1 += "<thead><tr><td>ID Contrato</td><td>Nombre Transportista</td><td>Valor Envio</td><td>Fecha Envio</td><td>Empresa aseguradora</td></tr></thead>";
                    html1 += "<tbody>";

                    html1 += "<tr style='border: white 5px solid;'><td>" + tb.Rows[0]["ID_CONTRATO"].ToString() + "</td>";
                    html1 += "<td style='border: 1px solid #dddddd padding: 8px'>" + tb.Rows[0]["NOMBRE"].ToString() + "</td><td style='border: 1px solid #dddddd padding: 8px'>" + tb.Rows[0]["VALOR"].ToString() + "</td><td style='border: 1px solid #dddddd padding: 8px'>" + tb.Rows[0]["FECHA_ENVIO"].ToString() + "</td>";
                    html1 += "<td style='border: 1px solid #dddddd padding: 8px'>" + tb.Rows[0]["EMPRESA"].ToString() + "</td></tr>";
                    html1 += "</tbody>";
                    html1 += "</table>";

                    if (enviarCorreo.enviarCorreoTransportista(correotrans, cboIdVenta.Text, html1) != null)
                    {
                        MessageBox.Show("Contrato enviado al transportista !");
                    }






                    OracleCommand cmd1 = con.CreateCommand();

                    //query para rescatar correo de cliente
                    cmd1.CommandText = "SELECT C.CORREO as correo FROM CLIENTE C INNER JOIN PEDIDO P ON p.rut_cli = c.rut_cli WHERE P.ID_PEDIDO = " + "'" + cboIdVenta.SelectedValue + "'";
                    cmd1.CommandType = CommandType.Text;
                    OracleDataReader dr2 = cmd1.ExecuteReader();
                    DataTable dt1 = new DataTable();
                    dt1.Load(dr2);
                    string correocli = dt1.Rows[0]["CORREO"].ToString();
                    dr2.Close();
                    //query venta
                    OracleCommand cmd2 = con.CreateCommand();

                    cmd2.CommandText = "SELECT * FROM detalle_pedido d inner join pedido pe on pe.id_pedido = d.id_pedido inner join producto p on p.id_producto = d.id_producto inner join cliente c on pe.rut_cli = c.rut_cli where pe.id_pedido =" + "'" + cboIdVenta.Text + "'";
                    cmd2.CommandType = CommandType.Text;
                    OracleDataReader dr3 = cmd2.ExecuteReader();
                    DataTable dt2 = new DataTable();
                    dt2.Load(dr3);

                    string html = "";
                    html += "<head><meta charset='UTF-8' ></head>";
                    html += "<body>";
                    html += "<h1>Boleta Cliente</h1>";
                    html += "<tr><td>&#128151 Codigo de la Venta    :    " + dt2.Rows[0]["ID_PEDIDO"].ToString() + "</td></tr>";
                    html += "<tr><td>Nombre del Cliente    :    " + dt2.Rows[0]["NOMBRE"].ToString() + " " + dt2.Rows[0]["APELLIDO"].ToString() + "</td></tr>";
                    html += "<tr><td>Fecha de Envío        :    " + dt2.Rows[0]["FECHA_ENVIO"].ToString() + "</td></tr>";
                    html += "<table style='border: white 5px solid; width:300px'>";
                    html += "<thead><tr><td scope='col'>Cantidad</td><td>Nombre Producto</td><td>Valor</td></tr></thead>";
                    html += "<tbody>";
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        html += "<tr style='border: white 5px solid;'><td>" + dt2.Rows[i]["CANTIDAD"].ToString() + "</td>";
                        html += "<td>" + dt2.Rows[i]["NOMBRE_PRODUCTO"].ToString() + "</td><td>" + dt2.Rows[i]["VALOR"].ToString() + "</td></tr>";
                    }
                    html += "</tbody>";
                    html += "</table>";
                    html += "<table style='border: white 5px solid; width:200px'>";
                    html += "<tbody>";
                    html += "<tr><td></td></tr>";
                    html += "<tr><td style='border: 1px solid #dddddd padding: 8px'>Sub Total         :    </td><td>" + dt2.Rows[0]["SUB_TOTAL"].ToString() + "</td></tr>";
                    html += "<tr><td style='border: 1px solid #dddddd padding: 8px'>Precio Transporte :    </td><td>" + dt2.Rows[0]["P_TRANSPORTE"].ToString() + "</td></tr>";
                    html += "<tr><td style='border: 1px solid #dddddd padding: 8px'>Comision          :    </td><td>" + dt2.Rows[0]["COMISION_MG"].ToString() + "</td></tr>";
                    html += "<tr><td style='border: 1px solid #dddddd padding: 8px'>Total a Pagar     :    </td><td>" + dt2.Rows[0]["TOTAL_PAGAR"].ToString() + "</td></tr>";
                    html += "</tbody>";
                    html += "</table>";
                    html += "<body>";


                    if (enviarCorreo.enviarCorreoCliente(correocli, html) != null)
                    {
                        MessageBox.Show("Boleta enviada al cliente !");
                    }
                    
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
            v1.txtid.Text = txtidSubasta.Text;
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

        private void enviarCorreoCliente(string to, string idventa, string nombrecli, string html, string canpro, string envio)
        {

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add(to); //Correo destino?
            correo.Subject = "Boleta Cliente Feria Virtual"; //Asunto
            correo.Body = "Codigo de la Venta : " + idventa + "  Nombre del cliente : " + nombrecli + " " + html + " " + canpro + " Fecha de Envio : " + envio; //Mensaje del correo
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
        private void enviarCorreoTransportista(string to, string idventa, string nombrecli, string html, string canpro, string envio)
        {

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress("feriavirtualmg1@gmail.com", "Maipo Grande", System.Text.Encoding.UTF8);//Correo de salida
            correo.To.Add(to); //Correo destino?
            correo.Subject = "Boleta Cliente Feria Virtual"; //Asunto
            correo.Body = "Codigo de la Venta : " + idventa + "  Nombre del cliente : " + nombrecli + " " + html + " " + canpro + " Fecha de Envio : " + envio; //Mensaje del correo
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
        
    }
}
