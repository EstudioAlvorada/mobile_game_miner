using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Clicks : MonoBehaviour
{
    string[] construcoes = {"Casa", "Madeireira", "Mineradora" };
    Dictionary<string, Transform> tamanhos = new Dictionary<string, Transform>();

    DataBase dataBase;


    // Start is called before the first frame update
    void Start()
    {
        dataBase = new DataBase();

        foreach(var i in construcoes)
        {
            tamanhos.Add(i, GameObject.FindGameObjectWithTag(i).transform);
        }
    }



    // Update is called once per frame
    void Update()
    {
        click();

        Pontos();

        foreach(var i in construcoes)
        {
            Debug.Log(i);
            HUD.TextoAcumulacao(i);
        }
    }

    void click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Collider2D colisor = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos));

            //casa (ponto:casa)
            if (colisor != null )
            {
                if (construcoes.Contains(colisor.tag) && GameManager.Instance.construcoes.First(p => p.tipo == colisor.tag).ativo)
                {
                    StartCoroutine(Pulsando(colisor.tag));

                    var valores = GetValores(colisor.tag, GameManager.Instance.GetConstrucaoNivelByName(colisor.tag));

                    GameManager.Instance.SetValores(colisor.tag, valores[0], valores[1]);

                    dataBase.AutoSalvarPontuacoes();

                    FindObjectOfType<AudioManager>().Play(colisor.tag);

                }
            }
        }
    }

    public bool Pontos()
    {
        bool retorno = false;

        foreach(var i in GameManager.Instance.construcoes.Where(p => p.numUpgrade > 1))
        {
            if (Time.time - i.ultimoTempo >= i.velocidade)
            {
                GameManager.Instance.construcoes.FirstOrDefault(p => p.tipo == i.tipo).ultimoTempo = Time.time;

                var valores = GetValores(i.tipo, i.numUpgrade);

                GameManager.Instance.SetValoresAcumulados(i.tipo, valores[0], valores[1]);

                Pulsa(i.tipo);

                dataBase.AutoSalvarPontuacoes();

                retorno = true;
            }
        }

        return retorno;
    }

    public List<float> GetValores(string tipo, int nivel)
    {
        var valores = new List<float>();

        switch (nivel)
        {
            case 1:
            switch (tipo)
            {
                case "Casa":
                    valores.Add(0);valores.Add(1);
                    break;
                case "Madeireira":
                    valores.Add(2); valores.Add(0);
                    break;
                case "Mineradora":
                    valores.Add(3); valores.Add(0.4f);
                    break;
            }
            break;
            case 2:
            switch (tipo)
            {
                case "Casa":
                    valores.Add(0);valores.Add(3);
                    break;
                case "Madeireira":
                    valores.Add(4); valores.Add(1);
                    break;
            }
            break;
            case 3:
            switch (tipo)
            {
                case "Casa":
                    valores.Add(0);valores.Add(5);
                    break;
                case "Madeireira":
                    valores.Add(6); valores.Add(2);
                    break;
            }
            break;
            case 4:
            switch (tipo)
            {
                case "Casa":
                    valores.Add(0); valores.Add(7);
                    break;
                case "Madeireira":
                    valores.Add(8); valores.Add(3);
                    break;
            }
            break;
            case 5:
            switch (tipo)
            {
                case "Casa":
                    valores.Add(0); valores.Add(10);
                    break;
                case "Madeireira":
                    valores.Add(11); valores.Add(4);
                    break;
            }
            break;
        }
            
        return valores;
    }


    void Pulsa(string cons)
    {
        var obj = GameObject.FindGameObjectWithTag(cons);

        obj.transform.position = tamanhos.First(p => p.Key == cons).Value.transform.position;

        if (obj != null)
        {

            System.Collections.Hashtable hash =
           new System.Collections.Hashtable();
            hash.Add("amount", new Vector3(0.20f, 0.20f, 0f));
            hash.Add("time", 0.35f);
            hash.Add("delay", -5f);
            hash.Add("ignoretimescale", true);
            iTween.PunchScale(obj, hash);
        }
    }

    IEnumerator Pulsando(string cons)
    {
        var obj = GameObject.FindGameObjectWithTag(cons);

        obj.transform.position = tamanhos.First(p => p.Key == cons).Value.transform.position;
        obj.transform.localScale = tamanhos.First(p => p.Key == cons).Value.transform.localScale;


        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            obj.transform.localScale = new Vector3(
                (Mathf.Lerp(obj.transform.localScale.x, obj.transform.localScale.x + 0.005f, Mathf.SmoothStep(0f,1f, i))),
                (Mathf.Lerp(obj.transform.localScale.y, obj.transform.localScale.y + 0.005f, Mathf.SmoothStep(0f,1f, i))),
                (Mathf.Lerp(obj.transform.localScale.z, obj.transform.localScale.z + 0.005f, Mathf.SmoothStep(0f,1f, i)))
                );

            yield return new WaitForSeconds(0.005f);
        }

        for (float i = 0f; i <= 1f; i += 0.1f)
        {
            obj.transform.localScale = new Vector3(
                (Mathf.Lerp(obj.transform.localScale.x, obj.transform.localScale.x - 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(obj.transform.localScale.y, obj.transform.localScale.y - 0.005f, Mathf.SmoothStep(0f, 1f, i))),
                (Mathf.Lerp(obj.transform.localScale.z, obj.transform.localScale.z - 0.005f, Mathf.SmoothStep(0f, 1f, i)))
                );

            yield return new WaitForSeconds(0.005f);
        }

    }
}
