using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal; // O objetivo a ser alcan�ado
    float speed = 5.0f; // Velocidade de movimento
    float accuracy = 1.0f; // Precis�o para considerar que o objetivo foi alcan�ado
    float rotSpeed = 2.0f; // Velocidade de rota��o

    public GameObject wpManager; // Refer�ncia ao objeto que cont�m o WPManager
    private GameObject[] wps; // Array de waypoints
    private GameObject currentNode; // Waypoint atual
    private int currentWP = 11; // �ndice do waypoint atual na lista de waypoints
    private Graph g; // Refer�ncia ao grafo de waypoints

    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints; // Obt�m a lista de waypoints do WPManager
        g = wpManager.GetComponent<WPManager>().graph; // Obt�m o grafo de waypoints do WPManager
        currentNode = wps[0]; // Define o waypoint atual como o primeiro da lista
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, wps[1]); // Executa o algoritmo A* para encontrar o caminho at� o waypoint desejado
        currentWP = 1; // Define o �ndice do waypoint desejado como o atual
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, wps[6]); // Executa o algoritmo A* para encontrar o caminho at� o waypoint desejado
        currentWP = 6; // Define o �ndice do waypoint desejado como o atual
    }

    private void Update()
    {
        Debug.Log(currentWP); // Imprime o �ndice do waypoint atual no console
        Debug.Log(currentNode); // Imprime o waypoint atual no console
    }

    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
        {
            return; // Se n�o h� caminho encontrado ou j� alcan�ou o �ltimo waypoint, retorna
        }

        currentNode = g.getPathPoint(currentWP); // Define o waypoint atual como o pr�ximo waypoint no caminho

        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++; // Se o objetivo atual foi alcan�ado (dentro da precis�o definida), passa para o pr�ximo waypoint
        }

        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform; // Define o pr�ximo waypoint como o objetivo a ser alcan�ado
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed); // Rotaciona o objeto em dire��o ao objetivo
        }
    }
}