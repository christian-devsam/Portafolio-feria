using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class VentaColeccion
    {
        public static List<Venta> lista;

        public VentaColeccion()
        {
            if (lista == null)
            {
                lista = new List<Venta>();
            }
        }

        public bool modificarVenta(Venta VentaModificada)
        {
            foreach (var venta in lista)
            {
                if (venta.idPedido == VentaModificada.idPedido)
                {
                    int indice = lista.IndexOf(venta);
                    lista.RemoveAt(indice);

                    lista.Insert(indice, VentaModificada);
                    return true;
                }
            }
            return false;
        }

        public bool guardarVenta(Venta nuevaVenta)
        {
            if (existeVenta(nuevaVenta.idPedido) == false)
            {
                lista.Add(nuevaVenta);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool existeVenta(string idPedido)
        {
            foreach (var venta in lista)
            {
                if (venta.idPedido == idPedido)
                {
                    return true;
                }
            }
            return false;
        }
        public bool eliminarVenta(string idPedido)
        {
            foreach (var venta in lista)
            {
                if (venta.idPedido == idPedido)
                {

                    lista.Remove(venta);
                    return true;
                }
            }
            return false;
        }
        public Venta buscarVenta(string idPedido)
        {
            foreach (var venta in lista)
            {
                if (venta.idPedido == idPedido)
                {
                    return venta;
                }
            }
            return null;
        }
        public List<Venta> listar()
        {
            return lista;
        }
    }
}