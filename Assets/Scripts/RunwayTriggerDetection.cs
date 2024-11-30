using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunwayTriggerDetection : MonoBehaviour
{
    public string RunwayNumber;
    public float RunwayHeading;

    void OnTriggerStay(Collider colliderObject)
    {
        colliderObject.gameObject.GetComponent<AircraftControl>().collidedRunwayNumber = RunwayNumber;
        colliderObject.gameObject.GetComponent<AircraftControl>().collidedRunwayHeading = RunwayHeading;
    }
}
