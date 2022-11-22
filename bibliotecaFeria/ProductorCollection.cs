using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class ProductorCollection
    {
        public static List<Productor> lista; //null

        public ProductorCollection()
        {
            if(lista == null)
            {
                lista = new List<Productor>();
            }
        }

        public bool agregarProductor(Productor nuevoProductor)
        {
            try
            {
                if(buscarProductor(nuevoProductor.RutProductor) == false)
                {
                    lista.Add(nuevoProductor);
                    return true; // el productor se agregó correctamente
                }
                else
                {
                    return false; // no se pudo agregar el productor, por que ya existe en la lista
                }
            }
            catch(Exception ex)
            {
                throw new Exception("error al agregar : " + ex.Message);
            }


        }

        private bool buscarProductor(string rutP)
        {
            try
            {
                foreach (var productor in lista)
                {
                    if (productor.RutProductor == rutP)
                    {
                        return true; //si! encontré el Rut, el productor ya existe en la lista
                    }
                }
                return false; // recorrimos toda lista y nunca encontré el Rut, entonces retorno false 

            }
            catch (Exception ex)
            {

                throw new Exception("error al buscar : " + ex.Message);
            }
        }

        public Productor buscarProductor2(string rutP)
        {
            try
            {
                foreach (var productor in lista)
                {
                    if (productor.RutProductor.Equals(rutP))
                    {
                        return productor;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("error al buscar" + ex.Message);
            }
        }

        public bool eliminarProductor(string rutP)
        {
            try
            {
                foreach (var productor in lista)
                {
                    if (productor.RutProductor == rutP)
                    {
                        lista.Remove(productor);
                        return true; // el productor a sido eliminado
                    }
                }
                return false; //recorrimos la lista y no encontro rut productor.
            }
            catch (Exception ex)
            {
                throw new Exception("error al eliminar : " + ex.Message);
            }
        }

        public bool modificarProductor(Productor productorModificado)
        {
            try
            {
                foreach ( var productor in lista)
                {
                    if (productor.RutProductor == productorModificado.RutProductor)
                    {
                        int indice = lista.IndexOf(productor);
                        lista.RemoveAt(indice); //eliminamos el productor que estaba ya registrado con ese rut
                        lista.Insert(indice, productorModificado);

                        return true; //el productor fué modificado
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("error al modificar : " + ex.Message);
            }
        }

        public List<Productor> listar()
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
