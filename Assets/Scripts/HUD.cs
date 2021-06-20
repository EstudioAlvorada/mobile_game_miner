using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI pontos, madeira;

    [SerializeField]
    GameObject menu;

    string[] btn = { "BtnCasa" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pontos.text =ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Casa").pontosTotal);
        madeira.text = ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Madeireira").pontosTotal);

        if(menu.active)
            ValoresBtn();
    }

    public void AbrirMenu()
    {
        if (menu.active)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    }

    public void Upgrade(string tipo)
    {

        var valores = GameManager.Instance.valores.FirstOrDefault(p => p.tipo == tipo && p.nivel == GameManager.Instance.GetConstrucaoNivelByName(tipo));

        GameManager.Instance.valores.ForEach(p => Debug.Log(p.valorDinheiro));
        GameManager.Instance.construcoes.ForEach(p => Debug.Log($"{p.numUpgrade}, {p.tipo}"));

        if(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tipo).pontosTotal >= valores.valorDinheiro)
        {
            var verifica = true;

            foreach (var i in valores.ValorRecursos)
            {
                verifica = verifica && i.recursoValor >= GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.recursoNome).pontosTotal;
            }

            if (verifica)
            {
                GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tipo).pontosTotal -= valores.valorDinheiro;

                foreach (var i in valores.ValorRecursos)
                {
                    GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.recursoNome).pontosTotal -= i.recursoValor;
                }

                GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tipo).numUpgrade++;

                new ImagemBLI().SetImagem(tipo, GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tipo).numUpgrade);
            }
        }


    }


    public string ScoreShow(float Score)
    {
        string result;
        string[] ScoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (Score < 900)
                break;
            else Score = Mathf.Floor(Score / 100f) / 10f;

        if (Score == Mathf.Floor(Score))
            result = Score.ToString() + ScoreNames[i];
        else result = Score.ToString("F1") + ScoreNames[i];


        return result;
    }

    public static void TextoAcumulacao(string tag)
    {
        var obj = GameObject.FindGameObjectWithTag(tag);

        Vector3 point = obj.transform.position + new Vector3(0f, 1f, 0f);

        obj.GetComponentInChildren<TextMeshProUGUI>().transform.position = point;

        if (GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tag).pontosAcumulados > 0)
            obj.GetComponentInChildren<TextMeshProUGUI>().text = new HUD().ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tag).pontosAcumulados);
        else
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "";

    }
   
    public void ValoresBtn()
    {
        foreach(var i in btn)
        {
            var butao = GameObject.Find("Canvas Principal/Menu/" + i);

            var valorConstrucao = GameManager.Instance.valores.First(p => i.Contains(p.tipo) && p.nivel == GameManager.Instance.construcoes.First(x => x.tipo == p.tipo).numUpgrade);

            string txtValor = $"<color=#FFF100> {valorConstrucao.valorDinheiro} </color>";

            foreach(var x in valorConstrucao.ValorRecursos)
            {
                var cor = x.recursoNome == "Madeireira" ? "<color=#653C3C>" : "";
                txtValor += $"{cor + x.recursoValor} </color>";
            }

            butao.GetComponentInChildren<TextMeshProUGUI>().text = txtValor;
        }
    }

}
