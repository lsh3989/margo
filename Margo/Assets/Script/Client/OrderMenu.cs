using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderMenu : MonoBehaviour {
    public int Menu_Num;
    public Sprite[] MenuImage = new Sprite[3];
    public string[] MenuName = new string[3];
    public GameObject Menu;
    public string ordermessage;
    // Use this for initialization
    void Start() {
        for (int i = 0; i < 3; i++)
        {
            Menu.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = MenuImage[i];
            Menu.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = MenuName[i];
        }
    }
    public void orderbtn()
    {
        ordermessage += "&Order|";
        for (int i = 0; i < 3; i++)
        {
            if (Menu.transform.GetChild(i).GetComponent<Ordercnttext>().ordercnt == 0)
                continue;
            ordermessage += MenuName[i];
            ordermessage += " ";
            ordermessage += Menu.transform.GetChild(i).GetComponent<Ordercnttext>().ordercnt.ToString()+"개";
            ordermessage += " ";
        }
        Debug.Log(ordermessage);
        GameObject.Find("Server").GetComponent<Client>().order(ordermessage);
        Menu.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }


	// Update is called once per frame
	void Update () {
		
	}
}
