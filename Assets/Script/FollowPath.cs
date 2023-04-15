using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{

    Transform goal;
    float speed = 5.0f;
    float accuracy = 1.0f;
    float rotSpeed = 2.0f;

    public GameObject wpManager;
    private GameObject[] wps;
    private GameObject currentNode;
    private int currentWP = 0;
    private Graph g;

    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }   
    public void GoToRuin()
    {
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        Debug.Log(currentNode);
        Debug.Log("Grugma1");
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
        {
            Debug.Log("Grugma2");
            return;
        }
        currentNode = g.getPathPoint(currentWP);
        Debug.Log("Grugma3");
        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            Debug.Log("Grugma4");
            currentWP++;
        }
        Debug.Log("Grugma5");
        if (currentWP < g.getPathLength())
        {
            Debug.Log("Grugma6");
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        }
    }
}