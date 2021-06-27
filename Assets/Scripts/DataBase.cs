using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cidadezinha.Construcoes;
using MarcelloDB;
using MarcelloDB.Collections;
using MarcelloDB.Platform;
using UnityEngine;

namespace Assets.Scripts
{
    public class DataBase
    {
        IPlatform platform = new MarcelloDB.netfx.Platform();

        public string dataPath = $"{Application.dataPath}/Base/";

        public void AutoSalvarPontuacoes()
        {
            using (var session = new MarcelloDB.Session(platform, dataPath))
            {
                var construcoesFile = session["base.dat"];

                var construcoes = construcoesFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

                foreach (var i in new string[] { "Casa", "Madeireira", "Mineradora" })
                {
                    var tipo = GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i);

                    construcoes.Persist(new Construcoes() { tipo = i, numUpgrade = tipo.numUpgrade, pontosTotal = tipo.pontosTotal, ativo = tipo.ativo, ultimoTempo = 0f, velocidade = 1f });

                }
            }

        }

        public void SaveConfig()
        {
            using (var session = new MarcelloDB.Session(platform, dataPath))
            {
                var baseFile = session["base.dat"];

                var config = baseFile.Collection<Config, int>("config", cons => cons.id);

                config.Persist(new Config() { id = GameManager.Instance.config.id, aberto = GameManager.Instance.config.aberto, ultimoGame = DateTime.Now });
            }

        }

        public List<Construcoes> GetAllConstrucoes()
        {
            List<Construcoes> tudo = new List<Construcoes>();
            using (var session = new MarcelloDB.Session(platform, dataPath))
            {
                var construcoesFile = session["base.dat"];

                var construcoes = construcoesFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

                tudo = construcoes.All.ToList();
            }

            return tudo;
        }


        public List<Valores> GetAllValores()
        {
            List<Valores> tudo = new List<Valores>();

            using (var session = new MarcelloDB.Session(platform, dataPath))
            {
                var construcoesFile = session["base.dat"];

                var valores = construcoesFile.Collection<Valores, int>("valores", cons => cons.id);

                tudo = valores.All.ToList();
            }

            return tudo;
        }

        public Config GetConfig()
        {
            var tudo = new Config();

            using (var session = new MarcelloDB.Session(platform, dataPath))
            {
                var construcoesFile = session["base.dat"];

                var config = construcoesFile.Collection<Config, int>("config", cons => cons.id);

                config.All.ToList().ForEach(p => Debug.Log(p.aberto));

                tudo = config.All.First();
            }

            return tudo;
        }

        public void CriarBanco()
        {
            if(!File.Exists(dataPath + "base.dat"))
            {
                using (var session = new MarcelloDB.Session(platform, dataPath))
                {
                    var baseFile = session["base.dat"];

                    var construcoes = baseFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

                    foreach (var i in new string[] { "Casa", "Madeireira", "Mineradora" })
                    {
                        if (construcoes.Find(i) == null)
                        {
                            construcoes.Persist(new Construcoes() { tipo = i, numUpgrade = 1, pontosTotal = 0, pontosAcumulados = 0, ativo = i == "Mineradora" ? false : true, ultimoTempo = 0f, velocidade = 1f });
                        }
                    }

                    var valores = baseFile.Collection<Valores, int>("valores", cons => cons.id);

                    ValoresCasa(valores);
                    ValoresMadeireira(valores);

                    var config = baseFile.Collection<Config, int>("config", cons => cons.id);

                    config.Persist(new Config() { id = 1, aberto = false, ultimoGame = DateTime.Now });
                }
            }
        }

        void ValoresCasa(Collection<Valores,int> valores)
        {
            var teste = new List<ValorRecurso>();

            valores.Persist(new Valores() { id = 1, tipo = "Casa", nivel = 1, valorDinheiro = 150, limiteArmazenamento = 20, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 340));

            valores.Persist(new Valores() { id = 2, tipo = "Casa", nivel = 2, valorDinheiro = 550, limiteArmazenamento = 50, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 810));
            teste.Add(new ValorRecurso("Mineradora", 460));

            valores.Persist(new Valores() { id = 3, tipo = "Casa", nivel = 3, valorDinheiro = 1250, limiteArmazenamento = 100, ValorRecursos = teste });


            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 2200));
            teste.Add(new ValorRecurso("Mineradora", 1500));
            teste.Add(new ValorRecurso("Petroleo", 580));

            valores.Persist(new Valores() { id = 4, tipo = "Casa", nivel = 4, valorDinheiro = 3100, limiteArmazenamento = 150, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 5300));
            teste.Add(new ValorRecurso("Mineradora", 3900));
            teste.Add(new ValorRecurso("Petroleo", 2100));

            valores.Persist(new Valores() { id = 5, tipo = "Casa", nivel = 5, valorDinheiro = 7500, limiteArmazenamento = 200, ValorRecursos = teste });
        }

        void ValoresMadeireira(Collection<Valores, int> valores)
        {
            var teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 90));

            valores.Persist(new Valores() { id = 6, tipo = "Madeireira", nivel = 1, valorDinheiro = 200, limiteArmazenamento = 20, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 470));

            valores.Persist(new Valores() { id = 7, tipo = "Madeireira", nivel = 2, valorDinheiro = 650, limiteArmazenamento = 50, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 940));
            teste.Add(new ValorRecurso("Mineradora", 590));

            valores.Persist(new Valores() { id = 8, tipo = "Madeireira", nivel = 3, valorDinheiro = 1350, limiteArmazenamento = 100, ValorRecursos = teste });


            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 2330));
            teste.Add(new ValorRecurso("Mineradora", 1630));
            teste.Add(new ValorRecurso("Petroleo", 710));

            valores.Persist(new Valores() { id = 9, tipo = "Madeireira", nivel = 4, valorDinheiro = 7600, limiteArmazenamento = 150, ValorRecursos = teste });

            teste = new List<ValorRecurso>();
            teste.Add(new ValorRecurso("Madeireira", 5430));
            teste.Add(new ValorRecurso("Mineradora", 4030));
            teste.Add(new ValorRecurso("Petroleo", 2230));

            valores.Persist(new Valores() { id = 10, tipo = "Madeireira", nivel = 5, valorDinheiro = 7600, limiteArmazenamento = 200, ValorRecursos = teste });
        }
    }
}
