using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionLight : MonoBehaviour {

    private Light light;
    float amplitude = 0.4f;
    float frequency = 0.4f;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        light.intensity += amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * Time.time)
            - Mathf.Sin(2 * Mathf.PI * frequency * (Time.time - Time.deltaTime)));
    }
}
