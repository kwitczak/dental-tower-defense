using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour {

    public static EmotionData lastEmotionData;
    public Light[] lights;
    Color stressColor = Color.red;
    Color calmColor = Color.cyan;
    Color focusColor = Color.white;

    // In seconds
    public static float emotionCooldown = 10;
    public static float emotionLength = 6;
    public static float nextEmotionTime;

    static Color currentStateColor;

	// Use this for initialization
	void Start () {
        currentStateColor = focusColor;
        setNextCooldown();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Emotion update");
        lastEmotionData = TCPServer.Emotion;

        if (lastEmotionData == null)
        {
            Debug.Log("No emotion data yet. Returning.");
            return;
        }

        if (isBored())
        {
            applyStressorReaction();
        }
        else if (isFocused())
        {
            applyFocusReaction();
        }
        else if (isStressed())
        {
            applyCalmReaction();
        }

    }


    // ON WORLD
    private void applyStressorReaction()
    {
        toggleLights(true, stressColor, 0.5f, 50);
    }

    private void applyCalmReaction()
    {
        toggleLights(true, calmColor, 0.5f, 100);
    }

    private void applyFocusReaction()
    {
        toggleLights(false, focusColor, 0f, 1);
    }

    private void toggleLights(bool enable, Color lightsColor, float baseIntensity, float multiplayer)
    {
        foreach (Light light in lights)
        {
            light.gameObject.SetActive(enable);
            light.color = lightsColor;
            if (lastEmotionData == null)
            {
                light.intensity = baseIntensity;
            } else
            {
                light.intensity = baseIntensity + (lastEmotionData.certainty / multiplayer);
            }
            
        }

        currentStateColor = lightsColor;
    }

    // ON ENEMIES
    public static void applyAura(Enemy enemy)
    {
        Transform aura = enemy.transform.Find("EmotionAura");
        if (!isEmotionDataReady())
        {
            aura.gameObject.SetActive(false);
            return;
        }

        aura.gameObject.SetActive(true);
        Light auraLight = aura.GetComponent<Light>();
        auraLight.color = currentStateColor;

        if (isBored())
        {
            enemy.applyStressorReaction(lastEmotionData.certainty);
        }
        else if (isStressed())
        {
            enemy.applyCalmReaction(lastEmotionData.certainty);
        } else
        {
            aura.gameObject.SetActive(false);
            enemy.cleanUpReaction();
        }
    }


    public static bool isEmotionDataReady()
    {
        return lastEmotionData != null;
    }

    public static bool isEmotionCooldownReady()
    {
        return nextEmotionTime <= Time.time;
    }

    public static void setNextCooldown()
    {
        nextEmotionTime = Time.time + emotionCooldown;
    }

    public static bool isBored()
    {
        return lastEmotionIs(Emotions.BORED);
    }

    public static bool isFocused()
    {
        return lastEmotionIs(Emotions.FOCUSED);
    }

    public static bool isStressed()
    {
        return lastEmotionIs(Emotions.STRESSED);
    }

    private static bool lastEmotionIs(Emotions emotion)
    {
        return lastEmotionData.emotionType == emotion.ToString();
    }
}
