using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftControl : MonoBehaviour
{
    public float commandedSpeed;
    public float commandedAltitude;
    public float commandedHeading;
    public string specialCommand;
    public string commandedDirect;
    Fly fly;
    ATCCommands atcCommands;

    public string collidedRunwayNumber;
    public float collidedRunwayHeading;

    public GameObject waypoints;
    public GameObject newWaypoint;

    public float holdingLegTime;
    public float holdingHeading;
    public bool flyingHolding = false;
    public float AngleTillTurnComplete;


    // Start is called before the first frame update
    void Start()
    {
        fly = gameObject.GetComponent<Fly>();
        waypoints = GameObject.Find("Waypoints");
    }

    // Update is called once per frame
    void Update()
    {
        if (specialCommand == "")
        {
            fly.desiredSpeed = commandedSpeed;
            fly.desiredAltitude = commandedAltitude;

            if (commandedDirect == "")
            {
                fly.desiredHeading = commandedHeading;
            }
            else
            {
                newWaypoint = GameObject.Find(commandedDirect);
                Vector3 newVectorToWaypoint = newWaypoint.transform.position - transform.position;
                fly.desiredHeading = Mathf.Atan2(newVectorToWaypoint.x, newVectorToWaypoint.z) * Mathf.Rad2Deg;
                Debug.DrawLine(transform.position, newWaypoint.transform.position, color: Color.green);
                if (Vector3.Distance(new Vector3(newWaypoint.transform.position.x, 0, newWaypoint.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) < 0.01)
                {                  
                    specialCommand = "Hold";
                }
                
            }
        }

        else if (specialCommand == "Takeoff")
        {

            fly.desiredAltitude = 0;
            fly.desiredHeading = collidedRunwayHeading;
            if (Quaternion.Angle(fly.transform.rotation, fly.newRotation) < fly.errorMargin)
            {
                fly.desiredSpeed = 180;
            }


            if (Mathf.Abs(fly.speed - 180) < fly.errorMargin)
            {
                specialCommand = "After Takeoff";
            }
        }
        else if (specialCommand == "After Takeoff")
        {
            fly.desiredAltitude = 1000;
            if (Mathf.Abs(fly.altitude - 1000) < fly.errorMargin)
            {
                specialCommand = "";
            }
        }
        else if (specialCommand == "Hold")
        {
            holdingHeading = Mathf.Round(fly.desiredHeading);

            if (flyingHolding == false)
            {
                flyingHolding = true;
                StartCoroutine(FlyHoldingPattern(holdingLegTime, holdingHeading));
            }
            
        }
    }


    public void GiveAircraftCommand(float speed, float altitude, float heading, string direct, string specialcommand)
    {
        commandedSpeed = speed;
        commandedAltitude = altitude;
        commandedHeading = heading;
        commandedDirect = direct;
        specialCommand = specialcommand;
    }

    IEnumerator FlyHoldingPattern(float legTime, float inboundHeading)
    {
        //Flies the outbound leg with pre selected heading
        yield return new WaitForSeconds(legTime);

        //Flies the outbound left turn to 180 degree opposite the pre selected heading
        fly.desiredHeading = inboundHeading - 90f;
        yield return new WaitForSeconds(0.01f);
        fly.desiredHeading = inboundHeading - 180f;
        while (Mathf.Abs(Mathf.DeltaAngle(fly.desiredHeading, fly.heading)) > fly.errorMargin) { yield return null; }

        //Flies the downwind leg with 180 degree opposite the pre selected heading
        yield return new WaitForSeconds(legTime);

        //Flies the inbound left turn to the pre selected heading
        fly.desiredHeading = inboundHeading + 90f;
        yield return new WaitForSeconds(0.01f);
        fly.desiredHeading = inboundHeading;
        while (Mathf.Abs(Mathf.DeltaAngle(fly.desiredHeading, fly.heading)) > fly.errorMargin) { yield return null; }

        flyingHolding = false;
    }
}
