using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundsSurvived : MonoBehaviour {

    public Text roundsText;

    void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    // Make numbers animation
    IEnumerator AnimateText ()
    {
        roundsText.text = "0";
        int score = 0;

        yield return new WaitForSeconds(.7f);

        while (score < PlayerStats.Score)
        {
            score+=100;
            roundsText.text = score.ToString();

            yield return new WaitForSeconds(0.01f);
        }
    }
}
