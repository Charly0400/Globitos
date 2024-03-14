using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon_Random : MonoBehaviour
{
    public Timer timer;
    public int currentLevel = 1; 
    public int minBalloonsPerLevel = 6; 
    public int maxBalloonsPerLevel = 15;
    public int balloonsPerLevel = 0; 

    private int balloonsDestroyed = 0; 

    public GameObject[] balloonPrefabs;
    public float spawnRadius = 5f; // Radio de la esfera de spawn
    public LayerMask spawnOverlapMask; // Capa para detectar colisiones al spawnear

    void Start()
    {
        StartLevel(currentLevel);
    }

    void Update()
    {
        // Verificar si no quedan globos en la escena
        GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");
        if (balloons.Length == 0)
        {
            // No quedan globos, cargar el siguiente nivel
            LoadNextLevel();
            timer.ResetTimer();
        }
    }

    void StartLevel(int level)
    {
        ResetLevel(); 
        balloonsPerLevel = Random.Range(minBalloonsPerLevel, maxBalloonsPerLevel + 1); 
        SpawnBalloons();
    }

    void ResetLevel()
    {
        balloonsDestroyed = 0;
    }

    void SpawnBalloons()
    {
        foreach (GameObject balloonPrefab in balloonPrefabs)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                Vector3 randomPosition = transform.position + randomDirection * spawnRadius;
                randomPosition = AvoidOverlap(randomPosition); // Evitar superposiciones

                Instantiate(balloonPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    GameObject GetRandomBalloonPrefab()
    {
        // Retorna un prefab de globo aleatorio de la lista de prefabs
        int randomIndex = Random.Range(0, balloonPrefabs.Length);
        return balloonPrefabs[randomIndex];
    }

    Vector3 AvoidOverlap(Vector3 position)
    {
        // Evitar superposiciones al spawnear los globos
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, spawnOverlapMask);
        int maxAttempts = 100;
        int attempts = 0;
        while (colliders.Length > 0 && attempts < maxAttempts)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            position = transform.position + randomDirection * spawnRadius;
            colliders = Physics.OverlapSphere(position, 0.1f, spawnOverlapMask);
            attempts++;
        }
        return position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void BalloonDestroyed()
    {
        // Método llamado cuando se destruye un globo
        balloonsDestroyed++;
        if (balloonsDestroyed >= balloonsPerLevel)
        {
            // Si se han destruido todos los globos del nivel, cargar el siguiente nivel
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        // Incrementar el nivel actual
        currentLevel++;

        // Reiniciar el nivel actual
        StartLevel(currentLevel);
    }
}