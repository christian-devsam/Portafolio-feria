using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class ClienteCollection
    {
        public static List<Cliente> lista; //null
        ContratoCollection cl = new ContratoCollection();

        public ClienteCollection()
        {
            //iniciamos lista
            if (lista == null)
            {
                lista = new List<Cliente>(); //vacia
            }
        }

        //metodos CRUD( CREATE, READ, UPDATE, DELETE)

        public bool agregarCliente(Cliente nuevoCliente)
        {
            try
            {
                if (buscarCliente(nuevoCliente.Rut) == false)
                {
                    lista.Add(nuevoCliente);
                    return true; // el cliente se agregó correctamente
                }
                else
                {
                    return false; // no se pudo agregar el cliente, por que ya existe en la lista
                }
            }
            catch (Exception ex)
            {

                throw new Exception("error al agregar : " + ex.Message);
            }

        }

        private bool buscarCliente(string rut)
        {
            try
            {
                foreach (var cliente in lista)
                {
                    if (cliente.Rut == rut)
                    {
                        return true; //si! encontré el Rut, el cliente ya existe en la lista
                    }
                }
                return false; // recorrimos toda lista y nunca encontré el Rut, entonces retorno false 

            }
            catch (Exception ex)
            {

                throw new Exception("error al buscar : " + ex.Message);
            }
        }

        public Cliente buscarCliente_2(string rut)
        {
            try
            {
                foreach (var cliente in lista)
                {
                    if (cliente.Rut.Equals(rut))
                    {
                        //este método retorna el cliente completo
                        return cliente;
                    }

                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception("error al buscar" + ex.Message);
            }

        }

        public bool eliminarCliente(string rut)
        {

            try
            {

                foreach (var cliente in lista)
                {
                    if (cliente.Rut == rut)
                    {
                        lista.Remove(cliente);
                        return true; //el cliente ha sido eliminado de la lista
                    }
                }
                return false; // recorrimos toda lista y nunca encontré el rut, entonces retorno false 

            }
            catch (Exception ex)
            {
                throw new Exception("error al eliminar : " + ex.Message);
            }


        }

        public bool modificarCliente(Cliente clienteModificado)
        {

            try
            {

                foreach (var cliente in lista)
                {
                    if (cliente.Rut == clienteModificado.Rut)
                    {

                        lista.Remove(cliente); //eliminamos el cliente que estaba ya registrado con el Rut
                        lista.Add(clienteModificado);

                        return true; //el cliente fué modificado
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception("error al modificar : " + ex.Message);
            }

        }

        public List<Cliente> listar()
        {
            try
            {

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception("error al listar" + ex.Message);
            }
        }

    }
}
