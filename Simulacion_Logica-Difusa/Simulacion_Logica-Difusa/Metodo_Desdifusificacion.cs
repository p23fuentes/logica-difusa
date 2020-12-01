using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public enum MetodoDesdifusificacion
    {
        Centroide,
        PromedioMaximos,
        Biseccion
    }

    public delegate double Definicion(Variable salida, double length);

    public static class MetodosDesdifusificacion
    {
        public static Dictionary<MetodoDesdifusificacion, Definicion> Metodos { get; set; }

        static MetodosDesdifusificacion()
        {
            Metodos = new Dictionary<MetodoDesdifusificacion, Definicion>();
            Metodos[MetodoDesdifusificacion.Centroide] = Centroide;
            Metodos[MetodoDesdifusificacion.PromedioMaximos] = PromedioMaximos;
            Metodos[MetodoDesdifusificacion.Biseccion] = Biseccion;
        }

        public static double Centroide(Variable salida, double length)
        {
            double numerador = 0;
            double denominador = 0; 
            double valorSalida = 0;


            for (double i = salida.MinValue; i <= salida.MaxValue; i += length)
            {
                valorSalida = salida.Evaluate(i);
                numerador += i * valorSalida;
                denominador += valorSalida;
            }

            if (denominador != 0) return numerador / denominador;
            else return 0;
        }
        public static double PromedioMaximos(Variable salida, double length)
        {
            double valor; 
            double mayor = 0; 
            double sum = 0; 
            double cant = 0;

            for (double i = salida.MinValue; i <= salida.MaxValue; i += length)
            {
                valor = salida.Evaluate(i);

                if (valor > mayor)
                { 
                    mayor = valor; 
                    sum = i; 
                    cant = 1; 
                }
                else if (valor == mayor)
                { 
                    sum += i; 
                    cant++; 
                }
            }

            if(cant != 0) return sum / cant;
            else return 0;
        }
        public static double Biseccion(Variable salida, double length) 
        {
            return Biseccion(salida, salida.MinValue, salida.MaxValue, length);
        }
        static double Biseccion(Variable salida, double a, double b, double length)
        {
           double c = (a + b) / 2;
           double area1 = Area(a,c,salida,length);
           double area2 = Area(c,b,salida,length);

           if (Math.Abs(area1 - area2) < 0.1) return c;
           else if (area1 > area2) return Biseccion(salida,a,c,length);
           else return Biseccion(salida, c, b, length);
        }
        static double Area(double a, double b, Variable salida, double length) 
        {
            double area = 0;

            for (double i = a; i <= b; i+=length)
            {
                area += (b - a) * salida.Evaluate(b);
            }

            return area;
        }



    }
}
