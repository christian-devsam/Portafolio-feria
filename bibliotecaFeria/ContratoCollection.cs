using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class ContratoCollection
    {
        public static List<Contrato> lista; //null

        public ContratoCollection()
        {
            if (lista == null)
            {
                lista = new List<Contrato>();
            }
        }
        public bool modificarContrato(Contrato contratoModificado)
        {
            foreach (var contrato in lista)
            {

                if (contrato.NumeroContrato == contratoModificado.NumeroContrato)
                {
                    int indice = lista.IndexOf(contrato);
                    lista.RemoveAt(indice);

                    lista.Insert(indice, contratoModificado);
                    return true;
                }

            }
            return false;
        }
        
        public bool guardarContrato(Contrato nuevocontrato)
        {
            if (existeContrato(nuevocontrato.NumeroContrato) == false)
            {
                lista.Add(nuevocontrato);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool existeContrato(int numeroContrato)
        {
            foreach (var contrato in lista)
            {
                if (contrato.NumeroContrato == numeroContrato)
                {
                    return true;
                }
            }
            return false;
        }
        

        public bool eliminarContrato(int numerocontrato)
        {
            foreach (var contrato in lista)
            {
                if (contrato.NumeroContrato == numerocontrato)
                {

                    lista.Remove(contrato);
                    return true;
                }
            }
            return false;
        }
        public Contrato buscarContrato(int numerocontrato)
        {
            foreach (var contrato in lista)
            {
                if (contrato.NumeroContrato == numerocontrato)
                {
                    return contrato;
                }
            }
            return null;
        }

        public List<Contrato> listar()
        {
            return lista;
        }

        public List<Contrato> filtrarPorTipoCliente(Cliente Nombre)
        {
            List<Contrato> cliente = (from cl in lista
                                     where cl.NombreCliente == Nombre
                                     select cl).ToList();
            return cliente;

        }
    }
}
