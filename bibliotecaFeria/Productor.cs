using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class Productor
    {
        #region variables
        private string _rutP;
        public string RutProductor
        {
            get { return _rutP; }
            set
            {
                if(value.Length >= 8 && value.Length <= 9)
                {
                    _rutP = value;
                }
                else
                {
                    throw new Exception("Rut debe contener 8 o 9 digitos");
                }
            }
        }

        private string _nombreP;
        public string NombreProductor
        {
            get { return _nombreP; }
            set
            {
                if (value.Length > 0)
                {
                    _nombreP = value;
                }
                else
                {
                    throw new Exception("Debe contener un Nombre del productor ");
                }
            }
        }

        private string _apellidos;
        public string ApellidosProductor
        {
            get { return _apellidos; }
            set
            {
                if (value.Length > 0)
                {
                    _apellidos = value;
                }
                else
                {
                    throw new Exception("Debe contener un Apellido del productor ");
                }
            }
        }

        private string _direccion;
        public string Direccion
        {
            get { return _direccion; }
            set
            {
                if (!value.Equals(""))
                {
                    _direccion = value;
                }
                else
                {
                    throw new Exception("La dirección no puede ser ingresada en blanco");
                }
            }
        }

        private string _correoP;
        public string CorreoProductor
        {
            get { return _correoP; }
            set
            {
                if (value.Length > 0)
                {
                    _correoP = value;
                }
                else
                {
                    throw new Exception("Debe contener un Nombre del productor ");
                }
            }
        }
        private string _telefono;
        public string Telefono
        {
            get { return _telefono; }
            set
            {
                if (value.Length >= 8 && value.Length <= 12)
                {
                    _telefono = value;
                }
                else
                {
                    throw new Exception("El teléfono debe contener entre 8 a 12 caracteres");
                }
            }
        }
        public DateTime fechaCreacion { get; set; }

        #endregion
        public Productor()
        {
            init();
        }

        private void init()
        {
            RutProductor = "000000000";
            NombreProductor = "JUAN";
            ApellidosProductor = "ARAVENA";
            Direccion = "LAS CLARAS 2222";
            Telefono = "999999999";
            fechaCreacion = DateTime.Now;
            CorreoProductor = "xxxxxxxxxxx";

        }
    }
}
