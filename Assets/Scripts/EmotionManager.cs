using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour {

    public static EmotionData lastEmotionData;
    public Light[] lights;
    Color stressColor = Color.red;
    Color calmColor = Color.green;
    Color focusColor = Color.white;

    static Color currentStateColor;

	// Use this for initialization
	void Start () {
        currentStateColor = focusColor;
    }
	
	// Update is called once per frame
	void Update () {
        lastEmotionData = TCPServer.Emotion;

		if(lastEmotionData == null)
        {
            return;
        }

        if(lastEmotionData.emotionType == Emotions.BORED.ToString())
        {
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

        currentStateColor = lightsColor;
    }

    public static bool isEmotionDataReady()
    {
        return lastEmotionData != null;
    }

    public static void applyAura(GameObject enemy)
    {
        if (!isEmotionDataReady() || lastEmotionData.emotionType == Emotions.FOCUSED.ToString())
        {
            return;
        }

        Transform aura = enemy.transform.Find("EmotionAura");
        aura.gameObject.SetActive(true);
        Light auraLight = aura.GetComponent<Light>();
        auraLight.color = currentStateColor;
    }
}
