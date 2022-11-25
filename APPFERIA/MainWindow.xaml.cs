using bibliotecaFeria;
using ControlzEx.Theming;
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
using MahApps.Metro.Controls;

namespace interfazGrafica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary> 
    public partial class MainWindow
    {
        ClienteCollection coleccion = new ClienteCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {

            MenuPrincipal v1 = new MenuPrincipal();
            this.Close();
            v1.ShowDialog();
        }

        private void Contrato_Click_1(object sender, RoutedEventArgs e)
        {
            GestionContrato v2 = new GestionContrato();
            this.Close();
            v2.ShowDialog();
        }

        

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Inicio login = new Inicio();
            this.Close();
            login.ShowDialog();
        }

        private void AdmProductor_click(object sender, RoutedEventArgs e)
        {
            GestionProductor v3 = new GestionProductor();
            this.Close();
            v3.ShowDialog();
        }

        private void Subasta_click(object sender, RoutedEventArgs e)
        {
            GestionSubasta v4 = new GestionSubasta();
            this.Close();
            v4.ShowDialog();
        }

        private void Venta_click(object sender, RoutedEventArgs e)
        {
            GestionVenta venta = new GestionVenta();
            this.Close();
            venta.ShowDialog();
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

        private void cerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.ShowDialog();
        }
    }
}

