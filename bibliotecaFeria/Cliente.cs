using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class Cliente
    {
        #region VARIABLES
        //Declaracion de variables
        private string _rut;

        public string Rut
        {
            get { return _rut; }
            set
            {
                if (value.Length > 8 && value.Length < 11)
                {
                    _rut = value;
                }
                else
                {
                    throw new Exception("El rut debe contener entre 9 y 10 caracteres");
                }
            }
        }
        private string _nombreContacto;

        public string Nombre
        {
            get { return _nombreContacto; }
            set
            {
                if (!value.Equals(""))
                {
                    _nombreContacto = value;
                }
                else
                {
                    throw new Exception("El nombre no puede ser ingresado en blanco");
                }

            }
        }
        private string _apellidoContacto;

        public string Apellido
        {
            get { return _apellidoContacto; }
            set
            {
                if (!value.Equals(""))
                {
                    _apellidoContacto = value;
                }
                else
                {
                    throw new Exception("El Apellido no puede ser ingresado en blanco");
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
        

        private string _emailContacto;
        public string Correo
        {
            get { return _emailContacto; }
            set
            {
                if (!value.Equals(""))
                {
                    _emailContacto = value;
                }
                else
                {
                    throw new Exception("El email no puede ser ingresado en blanco");
                }
            }
        }
        private string _comuna;
        public string Comuna
        {
            get { return _comuna; }
            set
            {
                if (!value.Equals(""))
                {
                    _comuna = value;
                }
                else
                {
                    throw new Exception("La comuna no puede ser ingresado en blanco");
                }

            }
        }
        #endregion
        //Constructor
        public Cliente()
        {
            this.Init(); //generar metodo
        }

        private void Init()
        {
            Rut = "0000000000"; //tiene regla negocio
            Nombre = "xxxxxxxxxx";
            Apellido = "xxxxxxxxx";
            Direccion = "xxxxxxxxxx";
            Telefono = "11111111"; //tiene regla negocio
            Correo = "xxxxx@xxxx.xxx";
            Comuna = "xxxxxxx";
        }

        public static implicit operator string(Cliente v)
        {
            throw new NotImplementedException();
        }
    }
}
