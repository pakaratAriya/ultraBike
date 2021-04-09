using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    public static int LANE_NUMBER = 2;
    public static float LANE_GAP = 2;
    public int laneNumber;
    public List<GameObject> allLaneObjects = new List<GameObject>();

    private void OnEnable()
    {
        PlayerController.onChangeLaneEvent += UpdateLane;
    }

    private void OnDisable()
    {
        PlayerController.onChangeLaneEvent -= UpdateLane;
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            allLaneObjects.Add(child.gameObject);
        }
    }

    public void UpdateLane(int currentLane)
    {
        foreach (GameObject laneObject in allLaneObjects)
        {
            laneObject.layer = LayerMask.NameToLayer(currentLane == laneNumber ? "currentLane" : "notCurrentLane");
        }
    }

}
