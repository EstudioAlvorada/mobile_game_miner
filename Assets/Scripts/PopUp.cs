using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField]
    Canvas popUp;

    DataBase dataBase;

    // Start is called before the first frame update
    void Start()
    {
        dataBase = new DataBase();

        if (!GameManager.Instance.config.aberto)
        {
            ChamaPopUp("Bem vindo ao jogo apogeu. Clica nas coisa ai pra ganhar ponto, quando tiver bastante compra mais coisa pra ganhar mais ponto pra comprar mais coisa. Boa sorte.");
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Collider2D colisor = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos));


            //casa (ponto:casa)
            if (colisor != null)
            {
                if (colisor.tag == "PopUp")
                {
                    popUp.gameObject.SetActive(false);

                    GameManager.Instance.config.aberto = true;

                    dataBase.SaveConfig();

                }
            }
        }
    }

    public void ChamaPopUp(string txt)
    {
        var pasta = GameObject.FindGameObjectWithTag("Pasta PopUp");

        var popUpObj = pasta.transform.Find("PopUp");

        popUpObj.gameObject.SetActive(true);

        popUpObj.GetComponentInChildren<TextMeshProUGUI>().text = txt;
    }


}
