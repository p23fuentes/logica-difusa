using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public enum TipoFuncion
    {
        Triangular,
        Trapezoidal
    }

    public abstract class Funcion
    {
        public abstract double MinX { get; }
        public abstract double MaxX { get; }
        public abstract double Evaluate(double x);

        protected double Incrementa(double a, double b, double x)
        {
            return (x - a) / (b - a);
        }
        protected double Decrementa(double a, double b, double x)
        {
            return (x - b) / (a - b);
        }

    }
}
