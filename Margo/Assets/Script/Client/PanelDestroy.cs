using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void destroy()
    {
        int childcount;
        childcount = gameObject.transform.GetChildCount();
        GameObject.Destroy(gameObject.transform.GetChild(childcount-2));
    }
}
