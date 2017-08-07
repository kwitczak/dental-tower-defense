using UnityEngine;

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
    }
}
