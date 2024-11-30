using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAircraftInfo : MonoBehaviour
{


    //Creates the variable to store the plane game object
    GameObject aircraft;

    //Creates the variables needed for the UI constant scaling
    float InitialCameraSize;
    Vector3 initialUiScale;

    //Creates the variable to store the fly & follow script
    Fly fly;
    AircraftControl aircraftControl;
    SelectAircraft selectAircraft;

    //Creates the Display variables
    GameObject ATCDisplay;
    GameObject infoText;

    public string callsign;
    float altitude;
    float heading;
    float speed;
    float desiredAltitude;
    float desiredHeading;
    float desiredSpeed;
    string displayedAltitude;
    string displayedHeading;
    string displayedSpeed;
    float errorMargin;
    string direct;
    string specialCommandPhase;

    void Start()
    {
        //Stores the aircraft and script
        aircraft = transform.parent.gameObject;
        fly = aircraft.GetComponent<Fly>();
        aircraftControl = aircraft.GetComponent<AircraftControl>();
        selectAircraft = aircraft.GetComponent<SelectAircraft>();

        //Assigns the display texts to the Display variables
        ATCDisplay = transform.Find("ATCDisplay").gameObject;
        infoText = ATCDisplay.transform.Find("InfoText").gameObject;

        callsign = "ARX" + System.Convert.ToInt32(Random.Range(100.0f, 9999.0f));
        aircraft.name = callsign;
        

    }

    void Update()
    {
        altitude = Mathf.Round(fly.altitude);
        heading = Mathf.Round(fly.heading);
        speed = Mathf.Round(fly.speed);
        desiredAltitude = Mathf.Round(fly.desiredAltitude);
        desiredHeading = Mathf.Round(fly.desiredHeading) % 360f;
        desiredSpeed = Mathf.Round(fly.desiredSpeed);
        errorMargin = fly.errorMargin;
        direct = aircraftControl.commandedDirect;

        if (Mathf.Abs(altitude - desiredAltitude) > errorMargin)
        {
            if (altitude > desiredAltitude) {displayedAltitude = altitude + "FT" + "⇓" + desiredAltitude + "FT"; }
            else if (altitude < desiredAltitude) {displayedAltitude = altitude + "FT" + "⇑" + desiredAltitude + "FT"; }
        }
        else {displayedAltitude = altitude + "FT";}

        if (Mathf.Abs(speed - desiredSpeed) > errorMargin)
        {
            if (speed > desiredSpeed) { displayedSpeed = speed + "KTS" + "⇓" + desiredSpeed + "KTS"; }
            else if (speed < desiredSpeed) { displayedSpeed = speed + "KTS" + "⇑" + desiredSpeed + "KTS"; }
        }
        else { displayedSpeed = speed + "KTS"; }

        if (Quaternion.Angle(fly.transform.rotation, fly.newRotation) > errorMargin)
        {
            displayedHeading = heading + "°" + "⇒" + desiredHeading + "°";
        }
        else { displayedHeading = heading + "°"; }
        if (direct != "") { displayedHeading += "⇒" + direct; }

        if (aircraftControl.specialCommand == "Takeoff")
        {
            specialCommandPhase = "TAKING OFF";
        }
        else if (aircraftControl.specialCommand == "After Takeoff")
        {
            specialCommandPhase = "AFTER TAKEOFF";
        }
        else if (aircraftControl.specialCommand == "Hold")
        {
            specialCommandPhase = "HOLDING " + aircraftControl.commandedDirect + " " + aircraftControl.holdingHeading % 360f + "°";
        }
        else
        {
            specialCommandPhase = "";
        }

        infoText.GetComponent<Text>().text = callsign + "\n" + displayedAltitude + "\n" + displayedSpeed + "\n" + displayedHeading + "\n" + specialCommandPhase;

        if (selectAircraft.isSelected == true)
        {
            infoText.GetComponent<Text>().fontStyle = FontStyle.BoldAndItalic;
        }
        else
        {
            infoText.GetComponent<Text>().fontStyle = FontStyle.Bold;
        }

        
    }
}
