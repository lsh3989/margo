using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour {

    public static  GPS Instance { set; get; }
    public float latitude;
    public float longitude;
    public float LastGPSChecktime;
    public string GPSmessage;
	// Use this for initialization
	void Start () {
        LastGPSChecktime = -5;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());
        latitude = 0;
        longitude = 0;
	}
	private IEnumerator StartLocationService()
    {
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }
        
        Input.location.Start();
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if(maxWait <=0)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determin device location");
            yield break;
        }
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        yield break;
    }
    // Update is called once per frame
    void Update () {
        if ((Time.fixedTime) - LastGPSChecktime >=5)
        {
            LastGPSChecktime = Time.fixedTime;
            Debug.Log("Check GPS");
            StartCoroutine(StartLocationService());
            GPSmessage = "&Gps|";
            GPSmessage += latitude.ToString();
            GPSmessage += "|";
            GPSmessage += longitude.ToString();

            GameObject.Find("Server").GetComponent<Client>().location(GPSmessage);
        }
    }
}
