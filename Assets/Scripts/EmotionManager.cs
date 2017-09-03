using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour {

    public static EmotionData lastEmotionData;
    public static EmotionData lastReactionEmotion;

    public Light[] lights;
    static Color stressColor = Color.red;
    static Color calmColor = Color.cyan;
    static Color focusColor = Color.white;

    public static int emotionMinimumWave = 1;
    public static float affectiveReactionChance = 0.6f;
    public static float emotionCooldown = 10;
    public static float emotionLength = 6;
    public static float nextEmotionTime;

    static Color currentStateColor;
    public static Emotions currentReaction;
    public static bool affectiveReactionTriggered = false;

	// Use this for initialization
	void Start () {
        currentStateColor = focusColor;
        affectiveReactionTriggered = false;
    }


    // Affective reaction should run for the wave time, at least once per game.
    public void runAffectiveReaction(int waveIndex, bool lastWave)
    {
        if (waveIndex < emotionMinimumWave)
        {
            Debug.Log("Reaction cannot occur that early");
            return;
        }

        Debug.Log("Emotion update");
        lastEmotionData = TCPServer.Emotion;
        //lastEmotionData = new EmotionData();
        //lastEmotionData.heartBeat = 75;
        //lastEmotionData.emotionType = Emotions.STRESSED.ToString();
        //lastEmotionData.certainty = 82;
    
        if (lastEmotionData == null)
        {
            Debug.Log("No emotion data yet. Returning.");
            return;
        }

        float chance = Random.Range(0.0f, 1.0f);
        Debug.Log("Chance of reaction: " + chance);
        if (chance > affectiveReactionChance && !lastWave)
        {
            Debug.Log("Chance above: " + affectiveReactionChance + ", applying FOCUS...");
            applyFocusReaction();
            affectiveReactionTriggered = false;
            return;
        }

        Debug.Log("Running affective reaction!");
        affectiveReactionTriggered = true;

        lastReactionEmotion = lastEmotionData;
        if (isBored())
        {
            Debug.Log("Applying STRESS...");
            applyStressorReaction();
  
        }
        else if (isFocused())
        {
            Debug.Log("Applying FOCUS...");
            applyFocusReaction();
     
        }
        else if (isStressed())
        {
            Debug.Log("Applying CALM...");
            applyCalmReaction();
        }
    }

    // ON WORLD
    private void applyStressorReaction()
    {
        currentReaction = Emotions.BORED;
        toggleLights(true, stressColor, 0.2f, 100);
    }

    private void applyCalmReaction()
    {
        currentReaction = Emotions.STRESSED;
        toggleLights(true, calmColor, 0.2f, 200);
    }

    private void applyFocusReaction()
    {
        currentReaction = Emotions.FOCUSED;
        toggleLights(false, focusColor, 0f, 1);
    }

    private void toggleLights(bool enable, Color lightsColor, float baseIntensity, float multiplier)
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
                light.intensity = baseIntensity + (lastEmotionData.certainty / multiplier);
            }
            
        }

        currentStateColor = lightsColor;
    }

    // ON ENEMIES
    public static void applyAura(Enemy enemy)
    {
        Transform aura = enemy.transform.Find("EmotionAura");
        if (!affectiveReactionTriggered)
        {
            aura.gameObject.SetActive(false);
            return;
        }

        aura.gameObject.SetActive(true);
        Light auraLight = aura.GetComponent<Light>();
        auraLight.color = currentStateColor;

        switch (currentReaction)
        {
            case Emotions.BORED:
                enemy.applyStressorReaction(lastReactionEmotion.certainty);
                break;
            case Emotions.FOCUSED:
                aura.gameObject.SetActive(false);
                enemy.cleanUpReaction();
                break;
            case Emotions.STRESSED:
                enemy.applyCalmReaction(lastReactionEmotion.certainty);
                break;
        }
    }

    public static void applyTurretAura(Turret turret)
    {
        GameObject aura = turret.emotionAura;
        if (!affectiveReactionTriggered)
        {
            aura.SetActive(false);
            turret.cleanUpReaction();
            return;
        }

        aura.SetActive(true);
        Light auraLight = aura.GetComponent<Light>();
        auraLight.color = currentStateColor;

        switch(currentReaction)
        {
            case Emotions.BORED:
                turret.applyStressorReaction(lastReactionEmotion.certainty);
                break;
            case Emotions.FOCUSED:
                aura.gameObject.SetActive(false);
                turret.cleanUpReaction();
                break;
            case Emotions.STRESSED:
                turret.applyCalmReaction(lastReactionEmotion.certainty);
                break;
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
