using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionBallons : MonoBehaviour
{
    Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    public int objectsToSelect = 3; // Número de objetos que se deben seleccionar para ser eliminados
    public float highlightIntensity = 2f; // Intensidad de la luz al seleccionar

    private List<GameObject> selectedObjects = new List<GameObject>();

    private InputCamera _inputCamera;
    private void Awake()
    {
        _inputCamera = new InputCamera();
        _inputCamera.Enable();
    }
    void Start()
    {
        _inputCamera.MouseClick.LeftClick.performed += LeftClick_performed;
    }

    private void LeftClick_performed(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hacer un raycast para detectar objetos con un collider
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Si el objeto ya está seleccionado, deselecciónalo
            if (selectedObjects.Contains(hitObject))
            {
                DeselectObject(hitObject);
            }
            else // De lo contrario, selecciónalo
            {
                SelectObject(hitObject);
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        // Agregar el objeto a la lista de seleccionados
        selectedObjects.Add(obj);

        // Iluminar el objeto seleccionado
        HighlightObject(obj);

        // Si hemos seleccionado el número necesario de objetos, verificar si tienen el mismo nombre
        if (selectedObjects.Count == objectsToSelect)
        {
            CheckAndDestroy();
            // Limpiar la lista de objetos seleccionados después de eliminarlos
            selectedObjects.Clear();
        }
    }

    void DeselectObject(GameObject obj)
    {
        selectedObjects.Remove(obj);
        UnhighlightObject(obj);
    }

    void CheckAndDestroy()
    {
        // Crear una copia de la lista de objetos seleccionados
        List<GameObject> objectsToRemove = new List<GameObject>(selectedObjects);

        // Verificar si se han seleccionado suficientes objetos
        if (objectsToRemove.Count < objectsToSelect)
            return;

        // Obtener el nombre del primer objeto seleccionado
        string nameToMatch = objectsToRemove[0].name;

        // Verificar si todos los objetos seleccionados tienen el mismo nombre
        foreach (GameObject obj in objectsToRemove)
        {
            if (obj.name != nameToMatch)
            {
                // Si los nombres no coinciden, deseleccionar todos los objetos y salir de la función
                foreach (GameObject selectedObj in objectsToRemove)
                {
                    DeselectObject(selectedObj);
                }
                return;
            }
        }

        // Si todos los objetos tienen el mismo nombre, eliminarlos
        foreach (GameObject obj in objectsToRemove)
        {
            Destroy(obj);
        }

        // Limpiar la lista de objetos seleccionados después de eliminarlos
        selectedObjects.Clear();
    }
    void HighlightObject(GameObject obj)
    {
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