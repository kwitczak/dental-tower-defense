using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour {

    EmotionData lastEmotionData;
    public Light[] lights;
    Color stressColor = Color.red;
    Color calmColor = Color.green;
    Color focusColor = Color.white;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lastEmotionData = TCPServer.Emotion;

		if(lastEmotionData == null)
        {
            return;
        }

        if(lastEmotionData.emotion == Emotions.BORED.ToString())
        {
            Debug.Log("Stress");
            applyStressorReaction();
        } else
        {
            applyFocusReaction();
        }

	}

    private void applyStressorReaction()
    {
        toggleLights(true, stressColor);
    }

    private void applyFocusReaction()
    {
        toggleLights(false, focusColor);
    }

    private void toggleLights(bool enable, Color lightsColor)
    {
        foreach (Light go in lights)
        {
            go.gameObject.SetActive(enable);
            go.color = lightsColor;
        }
         
    }
}
