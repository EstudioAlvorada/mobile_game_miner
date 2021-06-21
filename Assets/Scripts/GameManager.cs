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

    void Awake()
    {
        if (Instance == null)
        {
            new DataBase().CriarBanco();
            Instance = this;
            Instance.construcoes = new DataBase().GetAllConstrucoes();
            Instance.valores = new DataBase().GetAllValores();
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
                construcoes.Where(p => p.tipo == "Casa").FirstOrDefault().pontosAcumulados += v1;
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
        if (tag != "Casa")
            construcoes.Where(p => p.tipo == tag).FirstOrDefault().pontosAcumulados += v;

        construcoes.Where(p => p.tipo == "Casa").FirstOrDefault().pontosAcumulados += v1;

    }

}
