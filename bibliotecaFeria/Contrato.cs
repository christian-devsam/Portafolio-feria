using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{

    
    public class Contrato
    {
        //creamos las variables

        public int NumeroContrato{ get; set; }

        public string NombreCliente { get; set; }

        public DateTime CreacionContrato { get; set; }

        public DateTime TerminoContrato { get; set; }

        public bool Vigente { get; set; }

        private string _observaciones;

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (!value.Equals(""))
                {
                    _observaciones = value;
                }
                else
                {
                    throw new Exception("Debe ingresar alguna obersación");
                }
            }
        }

        //constructor

        public Contrato()
        {
            Init();
        }

        private void Init()
        {

            NumeroContrato = NumeroContrato++;
            NombreCliente = String.Empty;
            
            CreacionContrato = DateTime.Now;
            TerminoContrato = DateTime.Now;
            Vigente = false;
            Observaciones = "xxxxxxx";
        }

    }
}
