using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public enum TipoVariable
    {
        Entrada,
        Salida
    }

    public class Variable
    {
        public string Nombre { get; set; }
        public TipoVariable Tipo { get; set; }
        public Dictionary<string, Funcion> ValorLinguist { get; private set; }
        public Variable(string nombre, TipoVariable tipo, Dictionary<string, Funcion> valores)
        {
            Nombre = nombre;
            Tipo = tipo;
            ValorLinguist = valores;
        }

        public double MinValue { get { if (ValorLinguist.Count == 0) throw new InvalidOperationException(); return ValorLinguist.Values.Min(x => x.MinX); } }
        public double MaxValue { get { if (ValorLinguist.Count == 0) throw new InvalidOperationException(); return ValorLinguist.Values.Max(x => x.MaxX); } }

        public double Evaluate(double valor)
        {
          return ValorLinguist.Values.Max(func => func.Evaluate(valor));
        }
    }
}
