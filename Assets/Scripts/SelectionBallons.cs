using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBallons : MonoBehaviour
{
    public int objectsToSelect = 3; // N�mero de objetos que se deben seleccionar para ser eliminados
    public float highlightIntensity = 2f; // Intensidad de la luz al seleccionar

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

                // Si el objeto ya est� seleccionado, deselecci�nalo
                if (selectedObjects.Contains(hitObject))
                {
                    DeselectObject(hitObject);
                }
                else // De lo contrario, selecci�nalo
                {
                    SelectObject(hitObject);
                }
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        // Agregar el objeto a la lista de seleccionados
        selectedObjects.Add(obj);

        // Iluminar el objeto seleccionado
        HighlightObject(obj);

        // Si hemos seleccionado el n�mero necesario de objetos, verificar si tienen el mismo tag
        if (selectedObjects.Count == objectsToSelect)
        {
            CheckAndDestroy();
            // Limpiar la lista de objetos seleccionados despu�s de eliminarlos
            selectedObjects.Clear();
        }
    }

    void DeselectObject(GameObject obj)
    {
        // Quitar el objeto de la lista de seleccionados
        selectedObjects.Remove(obj);

        // Restaurar el color original del objeto
        UnhighlightObject(obj);
    }

    void CheckAndDestroy()
    {
        // Crear una copia de la lista de objetos seleccionados
        List<GameObject> objectsToRemove = new List<GameObject>(selectedObjects);

        // Verificar si se han seleccionado suficientes objetos
        if (objectsToRemove.Count < objectsToSelect)
            return;

        // Obtener el tag del primer objeto seleccionado
        string tagToMatch = objectsToRemove[0].tag;

        // Verificar si todos los objetos seleccionados tienen el mismo tag
        foreach (GameObject obj in objectsToRemove)
        {
            if (obj.tag != tagToMatch)
            {
                // Si los tags no coinciden, deseleccionar todos los objetos y salir de la funci�n
                foreach (GameObject selectedObj in objectsToRemove)
                {
                    DeselectObject(selectedObj);
                }
                return;
            }
        }

        // Si todos los objetos tienen el mismo tag, eliminarlos
        foreach (GameObject obj in objectsToRemove)
        {
            Destroy(obj);
        }

        // Limpiar la lista de objetos seleccionados despu�s de eliminarlos
        selectedObjects.Clear();
    }

    Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>(); // Variable para guardar los colores originales

    void HighlightObject(GameObject obj)
    {
        // Aumentar temporalmente la intensidad de la luz del objeto seleccionado
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            originalColors[obj] = material.color; // Guardar el color original
            Color originalColor = originalColors[obj];
            material.color = originalColor * highlightIntensity;
        }
    }

    void UnhighlightObject(GameObject obj)
    {
        // Restaurar el color original del objeto
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            if (originalColors.ContainsKey(obj)) // Verificar si el objeto tiene un color original guardado
            {
                Color originalColor = originalColors[obj];
                material.color = originalColor; // Restaurar el color original
                originalColors.Remove(obj); // Eliminar el color original guardado
            } 
        }
    }
}
