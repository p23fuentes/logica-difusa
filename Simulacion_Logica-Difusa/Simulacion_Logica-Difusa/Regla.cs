using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public enum MetodoAgregacion
    {
        Mamdani,
        Larsen
    }

    public enum Operaciones
    {
        AND,
        OR,
        IMP,
        DIMP
    }

    public class Regla
    {
        public List<Atomo> Condiciones { get; set; }
        public List<Operaciones> Ops { get; set; }
        public Atomo Resultado { get; set; }

        public Regla()
        {
            Condiciones = new List<Atomo>();
            Ops = new List<Operaciones>();
        }

        public static double EvaluateAux(MetodoAgregacion metodo, Operaciones op, double v1, double v2)
        {
            if (op == Operaciones.OR)
                return Math.Max(v1, v2);
            else if (op == Operaciones.IMP)
                return EvaluateAux(metodo, Operaciones.OR, 1 - v1, v2);
            else if (op == Operaciones.DIMP)
                return EvaluateAux(metodo, Operaciones.AND, EvaluateAux(metodo, Operaciones.IMP, v1, v2), EvaluateAux(metodo, Operaciones.IMP, v2, v1));
            else
            {
                if (metodo == MetodoAgregacion.Larsen)
                    return v1 * v2;
                else
                    return Math.Min(v1, v2);
            }
        }

        public delegate double GetValue(string nombre, string caract, bool not);
        public double Evaluate(MetodoAgregacion metodo, GetValue getVal)
        {
            double valor = getVal(Condiciones[0].Nombre, Condiciones[0].Caract, Condiciones[0].Not);

            for (int i = 1; i < Condiciones.Count; i++)
            {
                double aux = getVal(Condiciones[i].Nombre, Condiciones[i].Caract, Condiciones[i].Not);
                valor = EvaluateAux(metodo, Ops[i - 1], valor, aux);
            }
            return valor;
        }
    }
}
