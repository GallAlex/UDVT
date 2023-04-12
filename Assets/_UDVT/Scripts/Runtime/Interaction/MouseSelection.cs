using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class enables Selection of GameObjects in the Game Scene through Mouse Clicks
/// </summary>
public class MouseSelection : MonoBehaviour
{

    [SerializeField] private Camera cam;

    private Ray ray;
    private RaycastHit hit;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(GetSelectedObject());
        }
    }

    /// <summary>
    /// Returns the GameObject that is selected by the Mouse
    /// Selection only works if the GameObject has a Collider Component
    /// </summary>
    /// <returns></returns>
    public GameObject GetSelectedObject()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        
        return null;
    }
}
