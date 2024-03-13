using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon_Random : MonoBehaviour
{
    public GameObject[] prefabs;
    public float sphereRadius = 5f; // Radio de la esfera
    public LayerMask overlapLayerMask; // Capa para detectar colisiones

    void Start()
    {
        // Iteramos sobre cada prefab
        foreach (GameObject prefab in prefabs)
        {
            // Instanciamos 3 copias del prefab dentro de la esfera
            for (int i = 0; i < 3; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                Vector3 randomPosition = transform.position + randomDirection * sphereRadius;

                // Evitar que se instancien unos sobre otros
                randomPosition = AvoidOverlap(randomPosition);

                Instantiate(prefab, randomPosition, Quaternion.identity);
            }
        }
    }

    Vector3 AvoidOverlap(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, overlapLayerMask); // Detectar colisiones en una pequeña esfera alrededor de la posición

        // Si hay objetos en la posición, buscar una nueva posición aleatoria
        while (colliders.Length > 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            position = transform.position + randomDirection * sphereRadius;
            colliders = Physics.OverlapSphere(position, 0.1f, overlapLayerMask);
        }

        return position;
    }

    void OnDrawGizmosSelected()
    {
        // Dibujamos el gizmo de la esfera en el editor de Unity
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
