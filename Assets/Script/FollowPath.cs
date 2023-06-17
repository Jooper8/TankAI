using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal; // O objetivo a ser alcançado
    float speed = 5.0f; // Velocidade de movimento
    float accuracy = 1.0f; // Precisão para considerar que o objetivo foi alcançado
    float rotSpeed = 2.0f; // Velocidade de rotação

    public GameObject wpManager; // Referência ao objeto que contém o WPManager
    private GameObject[] wps; // Array de waypoints
    private GameObject currentNode; // Waypoint atual
    private int currentWP = 11; // Índice do waypoint atual na lista de waypoints
    private Graph g; // Referência ao grafo de waypoints

    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints; // Obtém a lista de waypoints do WPManager
        g = wpManager.GetComponent<WPManager>().graph; // Obtém o grafo de waypoints do WPManager
        currentNode = wps[0]; // Define o waypoint atual como o primeiro da lista
    }

    public void GoToHeli()
    {
        g.AStar(currentNode, wps[1]); // Executa o algoritmo A* para encontrar o caminho até o waypoint desejado
        currentWP = 1; // Define o índice do waypoint desejado como o atual
    }

    public void GoToRuin()
    {
        g.AStar(currentNode, wps[6]); // Executa o algoritmo A* para encontrar o caminho até o waypoint desejado
        currentWP = 6; // Define o índice do waypoint desejado como o atual
    }

    private void Update()
    {
        Debug.Log(currentWP); // Imprime o índice do waypoint atual no console
        Debug.Log(currentNode); // Imprime o waypoint atual no console
    }

    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
        {
            return; // Se não há caminho encontrado ou já alcançou o último waypoint, retorna
        }

        currentNode = g.getPathPoint(currentWP); // Define o waypoint atual como o próximo waypoint no caminho

        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++; // Se o objetivo atual foi alcançado (dentro da precisão definida), passa para o próximo waypoint
        }

        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform; // Define o próximo waypoint como o objetivo a ser alcançado
            Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed); // Rotaciona o objeto em direção ao objetivo
        }
    }
}