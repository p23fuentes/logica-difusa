using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Simulacion_Logica_Difusa
{
    class Program
    {
        static List<Variable> variables = new List<Variable>();
        static List<Regla> reglas = new List<Regla>();
        static MetodoAgregacion metodoA;
        static MetodoDesdifusificacion metodoD;
        static Dictionary<string, double> entrada = new Dictionary<string, double>();


        static void Main(string[] args)
        {

             // Ejemplo();
              Entradas();

              Handler h = new Handler(metodoA, variables, reglas, entrada, metodoD);
              h.EvaluarReglas();
              h.Desdifucificacion();

              Console.WriteLine("Valores de salida:");
              ImprimeSalida(h.Salidas);
              Console.WriteLine();
              Console.WriteLine("Valores de desdifusificacion:");
              ImprimeDesifus(h.SalidaDef);
              Console.ReadLine();
        }

        private static void ImprimeDesifus(Dictionary<string, double> salidaDesifus)
        {
            foreach (var item in salidaDesifus.Keys)
            {
                Console.Write(item + ": " + salidaDesifus[item].ToString());
                Console.WriteLine();
            }
        }
        private static void ImprimeSalida(Dictionary<string, Dictionary<string, double>> salida)
        {
            foreach (var item in salida.Keys)
            {
                Console.Write(item + ": ");

                foreach (var item2 in salida[item].Keys)
                {
                    Console.Write(item2 + "=" + salida[item][item2].ToString() + ' ');
                }
                  Console.WriteLine();
            }
        }
       
        public static void Entradas() 
        {
            string linea;

            Console.WriteLine("Introdusca el Metodo de Agregacion: Mamdani (M) o Larsen (L)");
            linea = Console.ReadLine();
            if (linea == "M") metodoA = MetodoAgregacion.Mamdani;
            else if (linea == "L") metodoA = MetodoAgregacion.Larsen;
            else throw new Exception("Error en metodo de agregacion.");

            Console.WriteLine("Introdusca el Metodo de Desdifusificacion: Centroide (C), Biseccion (B) o PromedioMaximos (P)");
            linea = Console.ReadLine();
            if (linea == "C") metodoD = MetodoDesdifusificacion.Centroide;
            else if (linea == "B") metodoD = MetodoDesdifusificacion.Biseccion;
            else if (linea == "P") metodoD = MetodoDesdifusificacion.PromedioMaximos;
            else throw new Exception("Error en metodo de desdifusificacion.");

            Console.WriteLine("Introdusca las variables de entrada ($ para terminar):");
            Variables(TipoVariable.Entrada);
            Console.WriteLine("Introdusca las variables de salida ($ para terminar):");
            Variables(TipoVariable.Salida);

            Console.WriteLine("Introdusca las reglas:");
            Reglas();

            Console.WriteLine("Introdusca los valores a evaluar:");
            ValoresEntrada();
        }
        public static void Variables(TipoVariable tipo) 
        {
            while (true)
            {
                Console.WriteLine("Introdusca el nombre de la variable:");
                string  nombre = Console.ReadLine();
                Dictionary<string, Funcion> valores = new Dictionary<string, Funcion>();

                if (nombre=="$") break;

                while (true)
                {
                    Console.WriteLine("Introdusca una caracteristica ($ para terminar):");
                    string linea = Console.ReadLine();

                    if (linea == "$") break;
                    else
                    {
                        string[] temp = linea.Split(new char[] { ' ', ':', ',' });
                        if (temp[0] == "Triangular")
                            valores.Add(temp[1], new Triangular(double.Parse(temp[2]), double.Parse(temp[3]), double.Parse(temp[4])));
                        else
                            valores.Add(temp[1], new Trapezoidal(double.Parse(temp[2]), double.Parse(temp[3]), double.Parse(temp[4]), double.Parse(temp[5])));
                    }
                }
                
                variables.Add(new Variable(nombre, tipo, valores));
            }
      }
        public static void Reglas() 
        {   
            Atomo a;
            while (true)
            {
                Regla regla = new Regla();
                Console.WriteLine("Nueva regla :");
                Console.WriteLine("Introdusca variable: ($ para terminar reglas)");
                string aux = Console.ReadLine();
                if (aux == "$") break;

                string[] atomo = aux.Split();
                int i = atomo[1] == "not" ? 2 : 1;
                a = new Atomo(atomo[0], atomo[i], atomo[1] == "not");
                regla.Condiciones.Add(a);

                while (true)
                {
                    Console.WriteLine("Introdusca relacion: ");
                    string r = Console.ReadLine();
                    Console.WriteLine("Introdusca variable: ");
                    atomo = Console.ReadLine().Split();
                    i = atomo[1] == "not" ? 2 : 1;
                    a = new Atomo(atomo[0], atomo[i], atomo[1] == "not");

                    if (r == "->") { regla.Resultado = a; break; }
                    else
                    {
                        regla.Condiciones.Add(a);
                        switch (r)
                        {
                            case "&": regla.Ops.Add(Operaciones.AND);
                                break;
                            case "|": regla.Ops.Add(Operaciones.OR);
                                break;
                            case "=>": regla.Ops.Add(Operaciones.IMP);
                                break;
                            case "<=>": regla.Ops.Add(Operaciones.DIMP);
                                break;
                            default: throw new Exception("Error en regla");
                        }
                    }
                }
                reglas.Add(regla);
            }                
        }
        public static void ValoresEntrada()
        {
            while (true)
            {
                Console.WriteLine("Nuevo entrada-valor ($ para finalizar):");
                string linea = Console.ReadLine();
                if (linea == "$") break;
                string[] aux = linea.Split(' ');
                entrada[aux[0]] = double.Parse(aux[1]);
            }
        }

        public static void Ejemplo() 
        {
            Dictionary<string,Funcion> v1 = new Dictionary<string,Funcion> ();
            Dictionary<string,Funcion> v2 = new Dictionary<string,Funcion> ();
            Dictionary<string,Funcion> v3 = new Dictionary<string,Funcion> ();

            v1.Add("joven", new Trapezoidal(18, 18, 25, 30));
            v1.Add("adulto", new Triangular(20, 35, 50));
            v1.Add("mayor", new Trapezoidal(40, 60, 70, 70));
            variables.Add(new Variable("edad", TipoVariable.Entrada, v1));

            v2.Add("bajo", new Trapezoidal(0, 0, 10, 20));
            v2.Add("medio", new Triangular(10, 40, 60));
            v2.Add("alto", new Trapezoidal(50, 70, 100, 100));
            variables.Add(new Variable("% de manejo", TipoVariable.Entrada, v2));

            v3.Add("bajo", new Trapezoidal(0, 0, 10, 20));
            v3.Add("medio", new Triangular(10, 30, 45));
            v3.Add("alto", new Trapezoidal(40, 55, 100, 100));
            variables.Add(new Variable("riesgo financiero", TipoVariable.Salida, v3));

            Regla regla1 = new Regla();
            regla1.Condiciones.Add(new Atomo("edad", "joven",false));
            regla1.Condiciones.Add(new Atomo("% de manejo", "bajo", false));
            regla1.Ops.Add(Operaciones.AND);
            regla1.Resultado = new Atomo("riesgo financiero", "medio", false);
            reglas.Add(regla1);

            Regla regla2 = new Regla();
            regla2.Condiciones.Add(new Atomo("edad", "joven", false));
            regla2.Condiciones.Add(new Atomo("% de manejo", "medio", false));
            regla2.Ops.Add(Operaciones.AND);
            regla2.Resultado = new Atomo("riesgo financiero", "alto", false);
            reglas.Add(regla2);

            Regla regla3 = new Regla();
            regla3.Condiciones.Add(new Atomo("edad", "joven", false));
            regla3.Condiciones.Add(new Atomo("% de manejo", "alto", false));
            regla3.Ops.Add(Operaciones.AND);
            regla3.Resultado = new Atomo("riesgo financiero", "alto", false);
            reglas.Add(regla3);

            Regla regla4 = new Regla();
            regla4.Condiciones.Add(new Atomo("edad", "adulto", false));
            regla4.Condiciones.Add(new Atomo("% de manejo", "bajo", false));
            regla4.Ops.Add(Operaciones.AND);
            regla4.Resultado = new Atomo("riesgo financiero", "bajo", false);
            reglas.Add(regla4);

            Regla regla5 = new Regla();
            regla5.Condiciones.Add(new Atomo("edad", "adulto", false));
            regla5.Condiciones.Add(new Atomo("% de manejo", "medio", false));
            regla5.Ops.Add(Operaciones.AND);
            regla5.Resultado = new Atomo("riesgo financiero", "medio", false);
            reglas.Add(regla5);

            Regla regla6 = new Regla();
            regla6.Condiciones.Add(new Atomo("edad", "adulto", false));
            regla6.Condiciones.Add(new Atomo("% de manejo", "alto", false));
            regla6.Ops.Add(Operaciones.AND);
            regla6.Resultado = new Atomo("riesgo financiero", "alto", false);
            reglas.Add(regla6);

            Regla regla7 = new Regla();
            regla7.Condiciones.Add(new Atomo("edad", "mayor", false));
            regla7.Condiciones.Add(new Atomo("% de manejo", "bajo", false));
            regla7.Ops.Add(Operaciones.AND);
            regla7.Resultado = new Atomo("riesgo financiero", "medio", false);
            reglas.Add(regla7);

            Regla regla8 = new Regla();
            regla8.Condiciones.Add(new Atomo("edad", "mayor", false));
            regla8.Condiciones.Add(new Atomo("% de manejo", "medio", false));
            regla8.Ops.Add(Operaciones.AND);
            regla8.Resultado = new Atomo("riesgo financiero", "alto", false);
            reglas.Add(regla8);

            Regla regla9 = new Regla();
            regla9.Condiciones.Add(new Atomo("edad", "mayor", false));
            regla9.Condiciones.Add(new Atomo("% de manejo", "alto", false));
            regla9.Ops.Add(Operaciones.AND);
            regla9.Resultado = new Atomo("riesgo financiero", "alto", false);
            reglas.Add(regla9);
            
             metodoA = MetodoAgregacion.Mamdani;
             metodoD = MetodoDesdifusificacion.Centroide;
             entrada.Add("edad", 25);
             entrada.Add("% de manejo", 50);
        }
    }
}
