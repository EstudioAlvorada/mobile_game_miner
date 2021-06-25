using Assets.Scripts;
using Cidadezinha.Construcoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public List<Construcoes> construcoes { get; set; }
    public List<Valores> valores { get; set; }
    public Config config { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            new DataBase().CriarBanco();
            Instance = this;
            Instance.construcoes = new DataBase().GetAllConstrucoes();
            Instance.valores = new DataBase().GetAllValores();
            Instance.config = new DataBase().GetConfig();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine("AutoSave");
    }

    public int GetConstrucaoNivelByName(string nome)
    {
        return this.construcoes.Where(p => p.tipo == nome).First().numUpgrade;
    }

    internal void SetValores(string tag, float v, float v1)
    {
        if(construcoes.Where(p => p.tipo == tag && p.ativo).FirstOrDefault() != null)
        {
            if (tag != "Casa")
            {
                construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosTotal += (v + construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosAcumulados);
                construcoes.Where(p => p.tipo == "Casa").FirstOrDefault().pontosAcumulados += CalculaArmazenamento("Casa", v1);
            }
            else
            {
                construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosTotal += (v1 + construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosAcumulados);
            }

            construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosAcumulados = 0;
        }
    }

    internal void SetValoresAcumulados(string tag, float v, float v1)
    {
        var obj = construcoes.Where(p => p.tipo == tag).FirstOrDefault();
        if (tag != "Casa")
            construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosAcumulados += CalculaArmazenamento(tag, v);

        obj = construcoes.Where(p => p.tipo == "Casa").FirstOrDefault();
        construcoes.Where(p => p.tipo == "Casa").FirstOrDefault().pontosAcumulados += CalculaArmazenamento("Casa", v1);

    }

    public bool VerificaArmazenamento(string tag)
    {
        var obj = construcoes.Where(p => p.tipo == tag).FirstOrDefault();
        return obj.pontosAcumulados <= valores.First(p => p.tipo == tag && p.nivel == obj.numUpgrade).limiteArmazenamento;
    }

    public Construcoes GetConstrucaoNome(string tag)
    {
        return construcoes.First(p => p.tipo == tag);
    }

    public Valores GetValoresNomeNivel(string tag)
    {
        return valores.First(p => p.tipo == tag && p.nivel == GetConstrucaoNivelByName(tag));
    }

    public float CalculaArmazenamento(string tag, float valor)
    {
        var valores = GetValoresNomeNivel(tag);
        var obj = GetConstrucaoNome(tag);

        return VerificaArmazenamento("Casa") ? obj.pontosAcumulados + valor >= valores.limiteArmazenamento ? (-1 * obj.pontosAcumulados) + valores.limiteArmazenamento : valor : 0;
    }
}
