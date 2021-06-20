using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovimento : MonoBehaviour
{
    private Vector3 touchStart;

    Vector3 toque;
    float groundZ = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);

            var limiteX = (Camera.main.transform.position + direction).x * 2;
            var limiteY = (Camera.main.transform.position + direction).y * 2;


            var valorMaximoX = limiteX <= 4.81 && limiteX >= -7.052 ? true : false;

            var valorMaximoY = limiteY <= 9.45 && limiteY >= -1 ? true : false;

            Camera.main.transform.position += new Vector3(valorMaximoX ? direction.x : 0, valorMaximoY ? direction.y : 0);
            
        }
    }
    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
