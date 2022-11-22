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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;


namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para Inicio.xaml
    /// </summary>
    public partial class Inicio //: MetroWindow
    {
        OracleConnection con = null;

        public Inicio()
        {
            this.abrirConexion();
            InitializeComponent();
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
                throw new Exception("errorrrrr");
            }
        }


        

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM ADMINISTRADOR WHERE USUARIO = :usuario AND CONTRASENIA = :contra";
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(":usuario", txtUser.Text);
            cmd.Parameters.Add(":contra", txtPass.Password);

            OracleDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                MainWindow main = new MainWindow();
                con.Close();
                this.Close();
                main.Show();
            }
        }
    }
}
