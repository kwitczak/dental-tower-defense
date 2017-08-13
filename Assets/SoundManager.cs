using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public GameObject boomSound;
    public GameObject enemyDeathSound;
    public GameObject menuHover;
    public GameObject spawn;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More then one SoundManager in scene!");
            return;
        }
        instance = this;
    }

    public static SoundManager getInstance()
    {
        return instance;
    }
}
