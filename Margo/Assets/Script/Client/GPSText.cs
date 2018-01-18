using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSText : MonoBehaviour {
    public Text coordinates;



    // Update is called once per frame
    private void Update () {
        coordinates.text = "Lat:" + GPS.Instance.latitude.ToString() + "\nLon:" + GPS.Instance.longitude.ToString();
	}
}
