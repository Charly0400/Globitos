using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField]
    private float forceMovement = 1f;

    public Transform cameraLook;
    private InputCamera inputCamera;
    [SerializeField]

    //perimetro circulo
    private void Awake()
    {
        inputCamera = new InputCamera();
        inputCamera.Enable();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraLook);


        float horizontalInput = inputCamera.Camera.Movement.ReadValue<Vector2>().x;
        float verticalInput = inputCamera.Camera.Movement.ReadValue<Vector2>().y;

        // Calcular la rotación en los ejes X e Y basada en la entrada
        float rotationX = verticalInput * forceMovement * Time.deltaTime;
        float rotationY = horizontalInput * forceMovement * Time.deltaTime;

        // Rotar la cámara alrededor del punto de mira en los ejes X e Y
        transform.RotateAround(cameraLook.position, Vector3.up, rotationY);
        transform.RotateAround(cameraLook.position, transform.right, -rotationX);
    }
}
