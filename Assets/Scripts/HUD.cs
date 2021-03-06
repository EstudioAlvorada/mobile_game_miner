using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI pontos, madeira, minerio;

    [SerializeField]
    GameObject menu;

    public string ultimoMenu;

    string[] btn = { "BtnCasa", "BtnMadeireira" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pontos.text = "<size=120><sprite name=Casa></size>" + ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Casa").pontosTotal);
        madeira.text = "<size=130><sprite name=Madeireira></size>" + ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Madeireira").pontosTotal);
        minerio.text = "<size=130><sprite name=Mineradora></size>" + ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Mineradora").pontosTotal);

    }

    public void AbrirMenu()
    {
        if (menu.active)
            menu.SetActive(false);
        else
        {
            menu.SetActive(true);
            UpgradePorNome(string.IsNullOrEmpty(ultimoMenu) ? "Casa" : ultimoMenu);
        }
    }

    public void Upgrade()
    {

        var valores = GameManager.Instance.valores.FirstOrDefault(p => p.tipo == ultimoMenu && p.nivel == GameManager.Instance.GetConstrucaoNivelByName(ultimoMenu));

        GameManager.Instance.valores.ForEach(p => Debug.Log(p.valorDinheiro));
        GameManager.Instance.construcoes.ForEach(p => Debug.Log($"{p.numUpgrade}, {p.tipo}"));

        if(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Casa").pontosTotal >= valores.valorDinheiro)
        {
            Debug.Log("Entrou");

            var verifica = true;

            foreach (var i in valores.ValorRecursos)
            {
                verifica = verifica && i.recursoValor <= GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.recursoNome).pontosTotal;

                Debug.Log(verifica);
                Debug.Log(i.recursoNome +", "+ i.recursoValor);
                Debug.Log(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.recursoNome).tipo);
            }

            if (verifica)
            {
                Debug.Log("Entrou");

                GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Casa").pontosTotal -= valores.valorDinheiro;

                foreach (var i in valores.ValorRecursos)
                {
                    GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.recursoNome).pontosTotal -= i.recursoValor;
                }

                GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == ultimoMenu).numUpgrade++;

                new ImagemBLI().SetImagem(ultimoMenu, GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == ultimoMenu).numUpgrade);
            }
        }

        if(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Mineradora").ativo == false && ultimoMenu == "Casa" && GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == ultimoMenu).numUpgrade > 2)
        {
            GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == "Mineradora").ativo = true;
        }

        UpgradePorNome(ultimoMenu);
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

        Vector3 point = obj.transform.position + new Vector3(0f, (obj.GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.2F), 0f);

        obj.GetComponentInChildren<TextMeshProUGUI>().transform.position = point;

        if (GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tag && p.ativo) != null && GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tag && p.ativo).pontosAcumulados > 0)
            obj.GetComponentInChildren<TextMeshProUGUI>().text = $"<sprite name={tag}>" + new HUD().ScoreShow(GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == tag).pontosAcumulados);
        else
            obj.GetComponentInChildren<TextMeshProUGUI>().text = "";

    }
   
    public void UpgradePorNome(string nome)
    {
        var obj = GameManager.Instance.GetConstrucaoNome(nome);

        var panel = GameObject.Find("Canvas Principal/Menu/PanelUpgrade");

        var valorConstrucao = GameManager.Instance.valores.First(p => p.tipo == nome && p.nivel == obj.numUpgrade);

        string txtValor = $"<sprite=3><color=#FFF100> {valorConstrucao.valorDinheiro} </color>\n";

        foreach (var x in valorConstrucao.ValorRecursos)
        {
            var cor = x.recursoNome == "Madeireira" ? "<sprite=1><color=#653C3C>" : x.recursoNome == "Mineradora" ? "<sprite=4><color=#525252>" : "";
            txtValor += $"{cor + x.recursoValor} </color>\n";
        }

        panel.GetComponentInChildren<TextMeshProUGUI>().text = txtValor;

        var butao = GameObject.Find("Canvas Principal/Menu/BtnUpgrade");

        butao.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Upgrade{nome}");

        ultimoMenu = nome;
    }
}
