using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_Logica_Difusa
{
    public class Handler
    {
        public List<Variable> Variables { get; set; }
        public List<Regla> Reglas { get; set; }
        public MetodoAgregacion MetodoA { get; set; }
        public MetodoDesdifusificacion MetodoD { get; set; }
        public Dictionary<string, double> Entradas { get; set; }
        public Dictionary<string, Dictionary<string, double>> Salidas { get; set; }
        public Dictionary<string, double> SalidaDef { get; set; }
        public Handler(MetodoAgregacion metodoA, List<Variable> variables, List<Regla> reglas, Dictionary<string, double> entradas, MetodoDesdifusificacion metodoD)
        {
            MetodoA = metodoA;
            MetodoD = metodoD;
            Variables = variables;
            Reglas = reglas;
            Entradas = entradas;
            Salidas = new Dictionary<string, Dictionary<string, double>>();         
            SalidaDef = new Dictionary<string, double>();
        }

        public double GetValue(string nombreVariable, string caract, bool not)
        {
            double valor = Entradas[nombreVariable];
            double aux = Variables.First(x => x.Nombre == nombreVariable).ValorLinguist[caract].Evaluate(valor);
            
            if (not) return 1 - aux;
            else return aux;
        }
        public void EvaluarReglas()
        {
            foreach (var regla in Reglas)
            {
                ProcesarRegla(regla);
            }
        }
        private void ProcesarRegla(Regla regla)
        {
            if (Salidas.Keys.Contains(regla.Resultado.Nombre))
            {
                if (Salidas[regla.Resultado.Nombre].Keys.Contains(regla.Resultado.Caract))
                {
                    double nuevoValor = regla.Evaluate(MetodoA, GetValue);
                    double viejoValor = Salidas[regla.Resultado.Nombre][regla.Resultado.Caract];
                    Salidas[regla.Resultado.Nombre][regla.Resultado.Caract] = Math.Max(nuevoValor, viejoValor);
                }
                else
                {
                    Salidas[regla.Resultado.Nombre][regla.Resultado.Caract] = regla.Evaluate(MetodoA, GetValue);
                }
            }
            else
            {
                Salidas[regla.Resultado.Nombre] = new Dictionary<string, double>();
                Salidas[regla.Resultado.Nombre][regla.Resultado.Caract] = regla.Evaluate(MetodoA, GetValue);
            }
        }
        public void Desdifucificacion(double length=2)
        {
            Definicion metodoDesdifusificacion = MetodosDesdifusificacion.Metodos[MetodoD];
            double resultado;

            foreach (var item in Variables.Where(x => x.Tipo == TipoVariable.Salida))
            {
                resultado = metodoDesdifusificacion(item, length);
                SalidaDef[item.Nombre] = resultado;
            }
        }
    }
}
