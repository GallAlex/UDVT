using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class enables Movement of the Camera in the Game Scene similiar to the Unity Editor
/// </summary>
public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private float navigationSpeed = 0.1f;
    [SerializeField] private float shiftMultiplier = 2f;
    [SerializeField] private float sensitivity = 0.3f;
    [SerializeField] private float panSensitivity = 0.05f;
    [SerializeField] private float mouseWheelZoomSpeed = 0.1f;

    private Vector3 mousePos;
    private Quaternion currentRot;
    private bool isPanning;

    void Update()
    {
        MousePanning();
        if (isPanning)
        { return; }

        if (Input.GetMouseButton(1))
        {
            Vector3 move = Vector3.zero;
            float speed = navigationSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f;
            if (Input.GetKey(KeyCode.W))
                move += Vector3.forward * speed;
            if (Input.GetKey(KeyCode.S))
                move -= Vector3.forward * speed;
            if (Input.GetKey(KeyCode.D))
                move += Vector3.right * speed;
            if (Input.GetKey(KeyCode.A))
                move -= Vector3.right * speed;
            if (Input.GetKey(KeyCode.E))
                move += Vector3.up * speed;
            if (Input.GetKey(KeyCode.Q))
                move -= Vector3.up * speed;
            cam.transform.Translate(move);
        }
        if (Input.GetMouseButtonDown(1))
        {
            mousePos = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            currentRot = cam.transform.rotation;
        }

        if (Input.GetMouseButton(1))
        {
            Quaternion rot = currentRot;
            Vector3 dif = mousePos - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += dif * sensitivity;
            cam.transform.rotation = rot;
        }

        MouseWheeling();

    }

    //Zoom with mouse wheel
    void MouseWheeling()
    {
        float speed = 10 * (mouseWheelZoomSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f);

        Vector3 pos = cam.transform.position;
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            pos = pos - (cam.transform.forward * speed);
            cam.transform.position = pos;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            pos = pos + (cam.transform.forward * speed);
            cam.transform.position = pos;
        }
    }


    private float pan_x;
    private float pan_y;
    private Vector3 panComplete;

    void MousePanning()
    {

        pan_x = -Input.GetAxis("Mouse X") * panSensitivity;
        pan_y = -Input.GetAxis("Mouse Y") * panSensitivity;
        panComplete = new Vector3(pan_x, pan_y, 0);

        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            cam.transform.Translate(panComplete);
        }


    }

}
