using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class SubastaColleccion
    {
        public static List<Subasta> lista;

        public SubastaColleccion()
        {
            if (lista == null)
            {
                lista = new List<Subasta>();
            }
        }

        public bool modificarSubasta(Subasta SubastaModificada)
        {
            foreach (var subasta in lista)
            {
                if (subasta.IdSubasta == SubastaModificada.IdSubasta)
                {
                    int indice = lista.IndexOf(subasta);
                    lista.RemoveAt(indice);

                    lista.Insert(indice, SubastaModificada);
                    return true;
                }
            }
            return false;
        }

        public bool guardarSubasta(Subasta nuevaSubasta)
        {
            if (existeSubasta(nuevaSubasta.IdSubasta) == false)
            {
                lista.Add(nuevaSubasta);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool existeSubasta(string NombreTrans)
        {
            foreach (var subasta in lista)
            {
                if (subasta.IdSubasta == NombreTrans)
                {
                    return true;
                }
            }
            return false;
        }

        public bool eliminarSubasta(string NombreTrans)
        {
            foreach (var subasta in lista)
            {
                if (subasta.IdSubasta == NombreTrans)
                {

                    lista.Remove(subasta);
                    return true;
                }
            }
            return false;
        }
        public Subasta buscarSubasta(string NombreTrans)
        {
            foreach (var subasta in lista)
            {
                if (subasta.IdSubasta == NombreTrans)
                {
                    return subasta;
                }
            }
            return null;
        }

        public List<Subasta> listar()
        {
            return lista;
        }

    }

}
