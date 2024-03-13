using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel = 1; // Nivel actual
    public int minBalloonsPerLevel = 6; // Cantidad mínima de globos por nivel
    public int maxBalloonsPerLevel = 15; // Cantidad máxima de globos por nivel
    public int balloonsPerLevel = 0; // Cantidad de globos por nivel actual

    private int balloonsDestroyed = 0; // Cantidad de globos destruidos en el nivel actual

    void Start()
    {
        StartLevel(currentLevel);
    }

    void StartLevel(int level)
    {
        ResetLevel(); // Reiniciar el nivel
        balloonsPerLevel = Random.Range(minBalloonsPerLevel, maxBalloonsPerLevel + 1); // Seleccionar aleatoriamente la cantidad de globos para el nivel actual
        SpawnBalloons(); // Generar globos para el nivel actual
    }

    void ResetLevel()
    {
        // Reiniciar la cantidad de globos destruidos
        balloonsDestroyed = 0;
    }

    void SpawnBalloons()
    {
        // Generar los globos para el nivel actual
        for (int i = 0; i < balloonsPerLevel; i++)
        {
            // Instanciar los globos en posiciones aleatorias dentro de la escena
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(1f, 5f), Random.Range(-10f, 10f));
            GameObject balloon = Instantiate(GetRandomBalloonPrefab(), randomPosition, Quaternion.identity);
        }
    }

    GameObject GetRandomBalloonPrefab()
    {
        // Retorna un prefab de globo aleatorio
        // Aquí deberías implementar la lógica para obtener un prefab de globo de manera aleatoria
        // Por ahora, simplemente retornaremos el primer prefab de la lista
        return Resources.Load<GameObject>("BalloonPrefab");
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

    void Update()
    {
        // Verificar si no quedan globos en la escena
        GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");
        if (balloons.Length == 0)
        {
            // No quedan globos, cargar el siguiente nivel
            LoadNextLevel();
        }
    }
}