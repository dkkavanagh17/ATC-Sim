using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAircraft : MonoBehaviour
{
    public bool isSelected;
    public GameObject aircraftToSelect;

    public void Start()
    {
        aircraftToSelect = gameObject;
    }

    public void OnMouseDown()
    {
        ATCCommands.instance.selectedAircraft = gameObject;
        isSelected = true;
    }

    public void Update()
    {
        if (ATCCommands.instance.selectedAircraft != gameObject)
        {
            isSelected = false;
        }

    }
}
