using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ordercnttext : MonoBehaviour {
    public int ordercnt;
    public GameObject ordercnttext22;


    // Use this for initialization
    void Start () {
        ordercnt = 0;
	}
	
	// Update is called once per frame
	void Update () {
         
    ordercnttext22.GetComponent<Text>().text = ordercnt.ToString() + "개";

    }

    public void upbtn()
    {
        ordercnt++;
    }
    public void downbtn()
    {
        if (ordercnt > 0)
            ordercnt--;
    }
}
