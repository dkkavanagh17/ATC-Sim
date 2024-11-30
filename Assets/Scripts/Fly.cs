using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float speed;
    public float altitude;
    public float heading;

    public float desiredSpeed;
    public float desiredAltitude;
    public float desiredHeading;

    public float altitudeRatio = 1000;
    public float speedRatio = 100;
    public float errorMargin;
    public float pitch;
    public Quaternion newRotation;
    public float accelerationRate;
    public float turnRate;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        altitude = Mathf.Round(transform.position.y * altitudeRatio);
        heading = Mathf.Round(transform.eulerAngles.y);

        if (Mathf.Abs(altitude - desiredAltitude) > errorMargin)
        {
            if (altitude > desiredAltitude) { pitch = -3f; }
            else if (altitude < desiredAltitude) { pitch = 3f; }
        }
        else
        {
            pitch = 0;
        }

        newRotation = Quaternion.Euler(-pitch, desiredHeading, transform.rotation.z);
        transform.position += transform.forward * speed * Time.deltaTime / speedRatio;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnRate * Time.deltaTime);
        speed = Mathf.MoveTowards(speed, desiredSpeed, Time.deltaTime * accelerationRate);
    }
}
