using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static bool gameIsOver;
    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public GameObject introductionModal;

    void Start()
    {
        gameIsOver = false;
        introductionModal.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if (gameIsOver)
            return;

        // if (Input.GetKeyDown("e"))
        // {
        //     EndGame();
        // }

		if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
	}

    void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
         
    }

    public void WinLevel()
    {
        gameIsOver = true;
        completeLevelUI.SetActive(true);
        Image filledStar = getStar("Star");
        Image starTwo = getStar("StarTwo");
        Image starThree = getStar("StarThree");

        if (PlayerStats.Lives == 32)
        {
            starThree.sprite = filledStar.sprite;
        }

        if (PlayerStats.Lives > 20)
        {
            starTwo.sprite = filledStar.sprite;
        }


    }

    private Image getStar(string starName)
    {
        return completeLevelUI.transform.Find(starName).GetComponent<Image>();
    }
}
