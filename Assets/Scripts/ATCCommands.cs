using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ATCCommands : MonoBehaviour
{
    public static ATCCommands instance;
    public GameObject selectedAircraft;
    public GameObject selectedAircraftPast;
    GameObject commandPanel;
    Button toggleViewAircraft;
    Button deselectAircraft;
    Button giveCommand;
    Text callsign;
    InputField inputSpeed;
    InputField inputAltitude;
    InputField inputHeading;
    InputField inputDirect;

    AircraftControl aircraftControl;
    Fly fly;
    UpdateAircraftInfo updateAircraftInfo;

    public float commandedSpeed;
    public float commandedAltitude;
    public float commandedHeading;
    public string commandedDirect;
    public string specialCommand;

    public GameObject waypoints;
    public GameObject newWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        commandPanel = transform.Find("CommandPanel").gameObject;
        toggleViewAircraft = commandPanel.transform.Find("View Aircraft").GetComponent<Button>();
        toggleViewAircraft.onClick.AddListener(ToggleViewAircraft);
        deselectAircraft = commandPanel.transform.Find("Deselect Aircraft").GetComponent<Button>();
        deselectAircraft.onClick.AddListener(DeselectAircraft);
        giveCommand = commandPanel.transform.Find("Give Command").GetComponent<Button>();
        giveCommand.onClick.AddListener(GiveCommand);

        callsign = commandPanel.transform.Find("Callsign").Find("Text").gameObject.GetComponent<Text>();
        inputSpeed = commandPanel.transform.Find("Speed").Find("InputField").gameObject.GetComponent<InputField>();
        inputAltitude = commandPanel.transform.Find("Altitude").Find("InputField").gameObject.GetComponent<InputField>();
        inputHeading = commandPanel.transform.Find("Heading").Find("InputField").gameObject.GetComponent<InputField>();
        inputDirect = commandPanel.transform.Find("Direct").Find("InputField").gameObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedAircraft != null)
        {
            if (selectedAircraft != selectedAircraftPast)
            {
                selectedAircraftPast = selectedAircraft;

                commandPanel.SetActive(true);
                aircraftControl = selectedAircraft.GetComponent<AircraftControl>();
                fly = selectedAircraft.GetComponent<Fly>();
                updateAircraftInfo = selectedAircraft.transform.Find("FaceCamCanvas").gameObject.GetComponent<UpdateAircraftInfo>();

                callsign.text = updateAircraftInfo.callsign;
                inputSpeed.text = System.Convert.ToString(aircraftControl.commandedSpeed);
                inputAltitude.text = System.Convert.ToString(aircraftControl.commandedAltitude);
                inputHeading.text = System.Convert.ToString(aircraftControl.commandedHeading);
                inputDirect.text = aircraftControl.commandedDirect;
            }

            HandleATCCommands();
            
        }
        else
        {
            commandPanel.SetActive(false);
            if (selectedAircraft != selectedAircraftPast)
            {
                selectedAircraftPast = selectedAircraft;
            }

            
        }
    }

    void ToggleViewAircraft()
    {
        if(toggleViewAircraft.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text == "View Aircraft")
        {
            CamControl.instance.followTransform = selectedAircraft.transform;
            toggleViewAircraft.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "Unview Aircraft";
        }

        else if (toggleViewAircraft.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text == "Unview Aircraft")
        {
            CamControl.instance.followTransform = null;
            toggleViewAircraft.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "View Aircraft";
        }

    }

    void DeselectAircraft()
    {
        CamControl.instance.followTransform = null;
        selectedAircraft = null;
        toggleViewAircraft.gameObject.transform.Find("Text").gameObject.GetComponent<Text>().text = "View Aircraft";
    }

    void GiveCommand()
    {
        commandedSpeed = float.Parse(inputSpeed.text);
        commandedAltitude = float.Parse(inputAltitude.text);
        commandedHeading = float.Parse(inputHeading.text);
        commandedDirect = inputDirect.text;

        if (giveCommand.transform.Find("Text").gameObject.GetComponent<Text>().text == "Give Command")
        {
            if (aircraftControl.specialCommand == "")
            {
                specialCommand = "";
            }
            else if (aircraftControl.specialCommand == "Hold" && inputDirect.text != aircraftControl.commandedDirect)
            {
                specialCommand = "";
            }
            else
            {
                specialCommand = aircraftControl.specialCommand;
            }
        }
        else if (giveCommand.transform.Find("Text").gameObject.GetComponent<Text>().text.Contains("Takeoff"))
        {
            specialCommand = "Takeoff";
        }

        //newWaypoint = waypoints.transform.Find(commandedDirect).gameObject;
        newWaypoint = GameObject.Find(commandedDirect);
        if (newWaypoint)
        {
            Debug.Log("Waypoint Found");
        }
        else
        {
            Debug.Log("Waypoint Not Found");
            inputDirect.text = "";
            commandedDirect = "";
        }

        aircraftControl.GiveAircraftCommand(commandedSpeed, commandedAltitude, commandedHeading, commandedDirect, specialCommand);
    }

    void HandleATCCommands()
    {
        

        //Configures command button to either Give Command, Takeoff
        if (fly.altitude == 0 & fly.speed == 0)
        {
            giveCommand.transform.Find("Text").gameObject.GetComponent<Text>().text = "Takeoff RW" + aircraftControl.collidedRunwayNumber;
        }
        else
        {
            giveCommand.transform.Find("Text").gameObject.GetComponent<Text>().text = "Give Command";
        }
    }
}
