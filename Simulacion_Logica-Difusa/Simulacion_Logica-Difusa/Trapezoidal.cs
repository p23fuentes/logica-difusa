using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public class Trapezoidal : Funcion
    {
        double a;
        double b;
        double c;
        double d;
        public Trapezoidal(double a, double b, double c, double d)
        {
            if (b < a || c < b || d < c) throw new ArgumentException();

            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
        public override double MinX { get { return a; } }
        public override double MaxX { get { return d; } }
        public override double Evaluate(double x)
        {
            if (x < a || x > d) return 0;
            else if (x < b) return Incrementa(a, b, x);
            else if (x < c) return 1;
            else return Decrementa(c, d, x);
        }
    }
}
