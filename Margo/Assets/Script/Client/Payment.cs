using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payment : MonoBehaviour {

    public int totalprice;
    public int peaplenumber;
    public string paymentmessage;
    public GameObject payment;
    // Use this for initialization
    void Start () {
        GameObject.Find("Server").GetComponent<Client>().pricerequest();
    }
    public void paymentbtn1() // 1/N
    {
        paymentmessage = "&Payment|1";
        Debug.Log(paymentmessage);
        GameObject.Find("Server").GetComponent<Client>().payment(paymentmessage);
        payment.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }
    public void paymentbtn2() // 각자
    {
        paymentmessage = "&Payment|2";
        Debug.Log(paymentmessage);
        GameObject.Find("Server").GetComponent<Client>().payment(paymentmessage);
        payment.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }
    public void paymentbtn3() // 혼자계산
    {
        paymentmessage = "&Payment|3";
        Debug.Log(paymentmessage);
        GameObject.Find("Server").GetComponent<Client>().payment(paymentmessage);
        payment.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }
    public void cancelbtn()
    {
        payment.transform.parent.GetComponent<RectTransform>().position = new Vector3(7000, 0, 0);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
