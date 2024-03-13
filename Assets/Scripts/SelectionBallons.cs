using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBallons : MonoBehaviour
{
    public int objectsToSelect = 3; // Número de objetos que se deben seleccionar para ser eliminados

    private List<GameObject> selectedObjects = new List<GameObject>();

    void Update()
    {
        // Detectar clics del mouse
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Hacer un raycast para detectar objetos con un collider
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // Verificar si el objeto no está en la lista de objetos seleccionados
                if (!selectedObjects.Contains(hitObject))
                {
                    // Agregar el objeto a la lista de seleccionados
                    selectedObjects.Add(hitObject);

                    // Si hemos seleccionado el número necesario de objetos, verificar si tienen el mismo tag
                    if (selectedObjects.Count == objectsToSelect)
                    {
                        CheckAndDestroy();
                        // Limpiar la lista de objetos seleccionados después de eliminarlos
                        selectedObjects.Clear();
                    }
                }
            }
        }
    }

    void CheckAndDestroy()
    {
        // Verificar si todos los objetos seleccionados tienen el mismo tag
        if (selectedObjects.Count < objectsToSelect)
            return;

        string tagToMatch = selectedObjects[0].tag;
        foreach (GameObject obj in selectedObjects)
        {
            if (obj.tag != tagToMatch)
            {
                return; // Si los tags no coinciden, salimos de la función sin hacer nada
            }
        }

        // Si los tags coinciden, eliminamos los objetos seleccionados
        foreach (GameObject obj in selectedObjects)
        {
            Destroy(obj);
        }
    }
}
