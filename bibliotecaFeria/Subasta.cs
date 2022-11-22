using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bibliotecaFeria
{
    public class Subasta
    {
        #region variables

        private string _idsubasta;
        public string IdSubasta
        {
            get { return _idsubasta; }
            set
            {
                if (value.Length >= 8 && value.Length <= 9)
                {
                    _idsubasta = value;
                }
                else
                {
                    throw new Exception("Rut debe contener 8 o 9 digitos");
                }
            }
        }


        public DateTime CreacionSubasta { get; set; }
        public DateTime TerminoSubasta { get; set; }
    

        #endregion

         public Subasta()
        {
            Init();
        }

        private void Init()
        {
            IdSubasta = "xxxx";
            CreacionSubasta = DateTime.Now;
            TerminoSubasta = DateTime.Now;
        }
    }
}
