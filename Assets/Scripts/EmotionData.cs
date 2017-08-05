[System.Serializable]
public class EmotionData
{
    public int heartBeat;
    public string emotionType;
    public int certainty;
}

public enum Emotions { STRESSED, FOCUSED, BORED };
