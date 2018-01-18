using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoombtn : MonoBehaviour {

    public GameObject MakeRoomprefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MakeRoombutton()
    {
        GameObject menu = gameObject.transform.parent.gameObject;
        GameObject go = Instantiate(MakeRoomprefab, menu.transform) as GameObject;
    }
}