using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum Direction { UNI, BI } // Enumera��o para definir a dire��o da conex�o entre os n�s
    public GameObject node1; // Primeiro n� da conex�o
    public GameObject node2; // Segundo n� da conex�o
    public Direction dir; // Dire��o da conex�o
}

public class WPManager : MonoBehaviour
{
    public GameObject[] waypoints; // Array de objetos que representam os waypoints
    public Link[] links; // Array de conex�es entre os waypoints
    public Graph graph = new Graph(); // Grafo que representa o mapa de waypoints

    void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp); // Adiciona cada waypoint como um n� no grafo
            }

            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2); // Adiciona uma aresta direcionada do node1 para o node2
                if (l.dir == Link.Direction.BI) // Se a dire��o for bidirecional
                {
                    graph.AddEdge(l.node2, l.node1); // Adiciona uma aresta do node2 para o node1 tamb�m
                }
            }
        }
    }

    void Update()
    {
        graph.debugDraw(); // Desenha visualmente o grafo (para fins de debugging)
    }
}

public class TankMovement : MonoBehaviour
{
    public Transform target; // Alvo a ser seguido (pode ser um waypoint)
    public float moveSpeed = 5f; // Velocidade de movimento do tanque

    private void Update()
    {
        if (target != null)
        {
            // Movimenta o tanque em dire��o ao alvo
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Rotaciona o tanque em dire��o ao alvo
            Vector3 direction = target.position - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 5f * Time.deltaTime);
            }
        }
    }
}
