using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointInfo : MonoBehaviour
{
    public Text infoText;
    
    // Start is called before the first frame update
    void Start()
    {
        infoText = transform.Find("FaceCamCanvas").transform.Find("InfoText").GetComponent<Text>();
        infoText.text = transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
