using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public class Triangular : Funcion
    {

        double a;
        double b;
        double c;
        public Triangular(double a, double b, double c)
        {
            if (b < a || c < b) throw new ArgumentException();

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override double MinX { get { return a; } }
        public override double MaxX { get { return c; } }

        public override double Evaluate(double x)
        {
            if (x < a || x > c) return 0;
            else if (x < b) return Incrementa(a, b, x);
            else return Decrementa(b, c, x);
        }
    }
}
