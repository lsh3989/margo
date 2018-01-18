using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modebtn : MonoBehaviour {
    public int modeNum;
    public GameObject ScroolRects;
    
    // Use this for initialization
    void Start () {
        modeNum = ScroolRects.transform.childCount;
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void modebtn1()
    {
        for (int i = 0; i < modeNum; i++)
            ScroolRects.transform.GetChild(i).gameObject.SetActive(false);
        ScroolRects.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void modebtn2()
    {
        for (int i = 0; i < modeNum; i++)
            ScroolRects.transform.GetChild(i).gameObject.SetActive(false);
        ScroolRects.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void modebtn3()
    {
        for (int i = 0; i < modeNum; i++)
            ScroolRects.transform.GetChild(i).gameObject.SetActive(false);
        ScroolRects.transform.GetChild(2).gameObject.SetActive(true);
    }
    public void modebtn4()
    {
        for (int i = 0; i < modeNum; i++)
            ScroolRects.transform.GetChild(i).gameObject.SetActive(false);
        ScroolRects.transform.GetChild(3).gameObject.SetActive(true);
    }
}
