using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cidadezinha.Construcoes;
using MarcelloDB;
using MarcelloDB.Platform;
using UnityEngine;

namespace Assets.Scripts
{
    public class DataBase
    {
        IPlatform platform = new MarcelloDB.netfx.Platform();

        public DataBase()
        {
            //string dataPath = $"{Application.dataPath}/Base/";

            //var session = new MarcelloDB.Session(platform, dataPath);

            //var baseFile = session["base.dat"];

            //var construcoes = baseFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

            //foreach (var i in new string[] { "Casa", "Madeireira" })
            //{
            //    if (construcoes.Find(i) == null)
            //    {
            //        construcoes.Persist(new Construcoes() { tipo = i, numUpgrade = 1, pontosTotal = 0, ativo = true, ultimoTempo = 0f, velocidade = 1f });
            //    }
            //}

            //var valores = baseFile.Collection<Valores, int>("valores", cons => cons.id);

            //var teste = new List<ValorRecurso>();

            //valores.Persist(new Valores() { id = 1, tipo = "Casa", nivel = 1, valorDinheiro = 120, ValorRecursos = teste });

            //teste = new List<ValorRecurso>();
            //teste.Add(new ValorRecurso("Madeireira", 70f));

            //valores.Persist(new Valores() { id = 2, tipo = "Casa", nivel = 2, valorDinheiro = 200, ValorRecursos = teste });

            //teste = new List<ValorRecurso>();
            //teste.Add(new ValorRecurso("Madeireira", 150f));
            //teste.Add(new ValorRecurso("Minerio", 90f));

            //valores.Persist(new Valores() { id = 3, tipo = "Casa", nivel = 3, valorDinheiro = 280, ValorRecursos = teste });


            //teste = new List<ValorRecurso>();
            //teste.Add(new ValorRecurso("Madeireira", 240f));
            //teste.Add(new ValorRecurso("Minerio", 210f));
            //teste.Add(new ValorRecurso("Petroleo", 150f));

            //valores.Persist(new Valores() { id = 4, tipo = "Casa", nivel = 4, valorDinheiro = 380, ValorRecursos = teste });

            //teste = new List<ValorRecurso>();
            //teste.Add(new ValorRecurso("Madeireira", 330f));
            //teste.Add(new ValorRecurso("Minerio", 300f));
            //teste.Add(new ValorRecurso("Minerio", 240f));

            //valores.Persist(new Valores() { id = 5, tipo = "Casa", nivel = 5, valorDinheiro = 550, ValorRecursos = teste });

            //foreach (var i in new string[] { "Casa", "Madeireira" })
            //{
            //    if (construcoes.Find(i) == null)
            //    {
            //    }

            //    var teste = construcoes.Find(i);

            //    Debug.Log($"{teste.tipo}");
            //}

            //session.Dispose();

        }

        public void AutoSalvarPontuacoes()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            var session = new MarcelloDB.Session(platform, dataPath);

            var construcoesFile = session["base.dat"];

            var construcoes = construcoesFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

            foreach (var i in new string[] { "Casa", "Madeireira", "Mineradora" })
            {
                var tipo = GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i);

                construcoes.Persist(new Construcoes() { tipo = i, numUpgrade = tipo.numUpgrade, pontosTotal = tipo.pontosTotal, ativo = tipo.ativo, ultimoTempo = 0f, velocidade = 1f });
                
            }

            session.Dispose();
        }

        public void SaveConfig()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            var session = new MarcelloDB.Session(platform, dataPath);

            var baseFile = session["base.dat"];

            var config = baseFile.Collection<Config, int>("config", cons => cons.id);

            config.Persist(new Config() {id = GameManager.Instance.config.id, aberto = GameManager.Instance.config.aberto, ultimoGame = DateTime.Now });

            

            session.Dispose();
        }

        public List<Construcoes> GetAllConstrucoes()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            var session = new MarcelloDB.Session(platform, dataPath);

            var construcoesFile = session["base.dat"];

            var construcoes = construcoesFile.Collection<Construcoes, string>("construcoes", cons => cons.tipo);

            var tudo = construcoes.All.ToList();

            session.Dispose();

            return tudo;
        }


        public List<Valores> GetAllValores()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            var session = new MarcelloDB.Session(platform, dataPath);

            var construcoesFile = session["base.dat"];

            var valores = construcoesFile.Collection<Valores, int>("valores", cons => cons.id);

            var tudo = valores.All.ToList();

            session.Dispose();

            return tudo;
        }

        public Config GetConfig()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            var session = new MarcelloDB.Session(platform, dataPath);

            var construcoesFile = session["base.dat"];

            var config = construcoesFile.Collection<Config, int>("config", cons => cons.id);

            config.All.ToList().ForEach(p => Debug.Log(p.aberto));

            var tudo = config.All.First();

            session.Dispose();

            return tudo;
        }

        public void CriarBanco()
        {
            string dataPath = $"{Application.dataPath}/Base/";

            if(!File.Exists(dataPath + "base.dat"))
            {
                var session = new MarcelloDB.Session(platform, dataPath);

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

                var teste = new List<ValorRecurso>();

                valores.Persist(new Valores() { id = 1, tipo = "Casa", nivel = 1, valorDinheiro = 120, limiteArmazenamento = 20, ValorRecursos = teste });

                teste = new List<ValorRecurso>();
                teste.Add(new ValorRecurso("Madeireira", 70f));

                valores.Persist(new Valores() { id = 2, tipo = "Casa", nivel = 2, valorDinheiro = 200, limiteArmazenamento = 50,ValorRecursos = teste });

                teste = new List<ValorRecurso>();
                teste.Add(new ValorRecurso("Madeireira", 150f));
                teste.Add(new ValorRecurso("Mineradora", 90f));

                valores.Persist(new Valores() { id = 3, tipo = "Casa", nivel = 3, valorDinheiro = 280, limiteArmazenamento = 100, ValorRecursos = teste });


                teste = new List<ValorRecurso>();
                teste.Add(new ValorRecurso("Madeireira", 240f));
                teste.Add(new ValorRecurso("Mineradora", 210f));
                teste.Add(new ValorRecurso("Petroleo", 150f));

                valores.Persist(new Valores() { id = 4, tipo = "Casa", nivel = 4, valorDinheiro = 380, limiteArmazenamento = 150, ValorRecursos = teste });

                teste = new List<ValorRecurso>();
                teste.Add(new ValorRecurso("Madeireira", 330f));
                teste.Add(new ValorRecurso("Mineradora", 300f));
                teste.Add(new ValorRecurso("Petroleo", 240f));

                valores.Persist(new Valores() { id = 5, tipo = "Casa", nivel = 5, valorDinheiro = 550, limiteArmazenamento = 200, ValorRecursos = teste });

                var config = baseFile.Collection<Config, int>("config", cons => cons.id);

                config.Persist(new Config() { id = 1, aberto = false, ultimoGame = DateTime.Now });

                session.Dispose();
            }
        }
    }
}
