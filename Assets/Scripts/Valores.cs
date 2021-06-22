using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Valores
    {
        public Valores()
        {
            ValorRecursos = new List<ValorRecurso>();
        }
        public int id { get; set; }
        public string tipo { get; set; }
        public int nivel { get; set; }
        public float valorDinheiro { get; set; }
        public List<ValorRecurso> ValorRecursos { get; set; } 

    }

    public class ValorRecurso
    {
        public ValorRecurso(string nome, float valor)
        {
            this.recursoNome = nome;
            this.recursoValor = valor;
        }
        public string recursoNome { get; set; }
        public float recursoValor { get; set; }
    }

    public class Config
    {
        public int id { get; set; }
        public bool aberto { get; set; }
        public DateTime ultimoGame { get; set; }
    }
}
