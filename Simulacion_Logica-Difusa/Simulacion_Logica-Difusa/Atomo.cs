using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public class Atomo
    {
        public string Nombre { get; private set; }
        public string Caract { get; private set; }
        public bool Not { get; set; }

        public Atomo(string nombre, string caracteristica, bool not)
        {
            Nombre = nombre;
            Caract = caracteristica;
            Not = not;
        }
    }
}
