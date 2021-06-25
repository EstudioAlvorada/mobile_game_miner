using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    void Start()
    {
        var imagens = new ImagemBLI();

        imagens.CarregaImagens();
        imagens.SetAllImagens();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Imagem
{
    public Imagem(Sprite img, string nome, int nivel)
    {
        this.img = img;
        this.nome = nome;
        this.nivel = nivel;
    }
    public Sprite img { get; set; }
    public string nome { get; set; }
    public int nivel { get; set; }
    
}

public class ImagemBLI
{
    //Imagens
    List<Imagem> imagens = new List<Imagem>();

    List<string> construcoes = new List<string>();

    public void CarregaImagens()
    {
        construcoes.Add("Casa");
        construcoes.Add("Madeireira");
        construcoes.Add("Mineradora");

        imagens.Add(new Imagem(Resources.Load<Sprite>("Casa"), "Casa", 1));
        imagens.Add(new Imagem(Resources.Load<Sprite>("Casa2"), "Casa", 2));
        imagens.Add(new Imagem(Resources.Load<Sprite>("prediotop"), "Casa", 3));
        imagens.Add(new Imagem(Resources.Load<Sprite>("prediotop2"), "Casa", 4));

        imagens.Add(new Imagem(Resources.Load<Sprite>("floresta"), "Madeireira", 1));
        imagens.Add(new Imagem(Resources.Load<Sprite>("floresta2"), "Madeireira", 2));

        imagens.Add(new Imagem(Resources.Load<Sprite>("Montanha"), "Mineradora", 1));

        imagens.Add(new Imagem(Resources.Load<Sprite>("Tela_principal_jogo"), "Background", 1));


    }

    public void SetAllImagens()
    {
        foreach (var i in construcoes)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(i);

            obj.gameObject.GetComponent<SpriteRenderer>().sprite = imagens.Where(p => p.nome == i && p.nivel == GameManager.Instance.GetConstrucaoNivelByName(i)).FirstOrDefault().img;
            obj.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(obj.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x, obj.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        }
    }

    public void SetImagem(string nome, int nivel)
    {
        CarregaImagens();

        Debug.Log(nome + " " + nivel.ToString());

        GameObject obj = GameObject.FindGameObjectWithTag(nome);

        obj.gameObject.GetComponent<SpriteRenderer>().sprite = imagens.Where(p => p.nome == nome && p.nivel == nivel).FirstOrDefault().img;
        obj.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(obj.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x, obj.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

    }

    public Sprite GetImagem(string nome)
    {
        CarregaImagens();

        GameObject obj = GameObject.FindGameObjectWithTag(nome);

        Debug.Log(obj.name);

        return obj.gameObject.GetComponent<SpriteRenderer>().sprite = imagens.Where(p => p.nome == nome).FirstOrDefault().img;
    }

    public Sprite GetmagemUpgrade(string nome)
    {
        CarregaImagens();

        GameObject obj = GameObject.FindGameObjectWithTag(nome);

        return obj.gameObject.GetComponent<SpriteRenderer>().sprite = imagens.Where(p => p.nome == $"Upgrade{nome}").FirstOrDefault().img;

    }
}
