using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingElement : MonoBehaviour {

    public float rotationSpeedY;
    public float rotationSpeedX;
    public float rotationSpeedZ;

    // Update is called once per frame
    void Update () {
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }
}
