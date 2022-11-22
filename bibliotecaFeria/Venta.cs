using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class Venta
    {
        #region variables
        private string _idPedido;
        public string idPedido
        {
            get { return _idPedido; }
            set
            {
                if (value.Length > 0)
                {
                    _idPedido = value;
                }
                else
                {
                    throw new Exception("Debe contener un Nombre del productor ");
                }
            }
        }

        public DateTime fechaIngreso { get; set; }
        public DateTime fechEnvio { get; set; }
        public bool vigencia { get; set; }
        public bool despacho { get; set; }

        private string _descripcion;
        public string DescripcionVenta
        {
            get { return _descripcion; }
            set
            {
                if (value.Length > 0)
                {
                    _descripcion = value;
                }
                else
                {
                    throw new Exception("Debe contener un Nombre del productor ");
                }
            }
        }
        #endregion

        public Venta()
        {
            Init();
        }

        private void Init()
        {
            idPedido = "xxxxx";
            fechaIngreso = DateTime.Now;
            fechEnvio = DateTime.Now;
            vigencia = false;
            despacho = true;
            DescripcionVenta = "xxxxx";

        }
    }
}
