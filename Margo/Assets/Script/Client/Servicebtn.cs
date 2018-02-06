using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Servicebtn : MonoBehaviour
{
    public GameObject panelprefab;
    public GameObject paymentprefab;
    public GameObject easypayingprefab;
    public GameObject normalpayingprefab;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void servicebtn1()
    {
        GameObject menu = gameObject.transform.parent.gameObject;
        GameObject go = Instantiate(panelprefab, menu.transform) as GameObject;
    }
    public void servicebtn2()
    {
        GameObject payment = gameObject.transform.parent.gameObject;
        GameObject go = Instantiate(paymentprefab, payment.transform) as GameObject;
    }
    public void paybtn1()
    {
        GameObject easypay = gameObject.transform.parent.gameObject;
        GameObject go = Instantiate(easypayingprefab, easypay.transform) as GameObject;
    }

    public void paybtn2()
    {
        GameObject normalpay = gameObject.transform.parent.gameObject;
        GameObject go = Instantiate(normalpayingprefab, normalpay.transform) as GameObject;
    }
}