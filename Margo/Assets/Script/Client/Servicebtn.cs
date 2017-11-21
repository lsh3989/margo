using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servicebtn : MonoBehaviour {
    public GameObject panelprefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void servicebtn()
    {
        GameObject menu = GameObject.Find("Canvas");
        GameObject go = Instantiate(panelprefab, menu.transform) as GameObject;
    }
}
