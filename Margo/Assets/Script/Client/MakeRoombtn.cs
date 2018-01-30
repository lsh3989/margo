using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeRoombtn : MonoBehaviour {

    public GameObject MakeRoomprefab;
    public GameObject Room;
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
        GameObject menu = gameObject.transform.parent.parent.gameObject;
        GameObject go = Instantiate(MakeRoomprefab, menu.transform) as GameObject;

    }
}