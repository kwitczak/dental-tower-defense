using UnityEngine;

public class GameManager : MonoBehaviour {

    public static bool gameIsOver;

    public GameObject gameOverUI;

    void Start()
    {
        gameIsOver = false;
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
}
