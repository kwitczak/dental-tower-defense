using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public string levelToLoad = "MainLevel";
    public SceneFader sceneFader;
    public GameObject ConnectionStatusCanvas;
    public GameObject IPTextBox;
    public GameObject LoadingImage;
    public GameObject WaitingText;
    public GameObject SuccessIcon;

    private Text IPValue;
    private Text WaitText;

    public void Start ()
    {
        IPValue = IPTextBox.GetComponent<Text>();
        WaitText = WaitingText.GetComponent<Text>();
    }

    public void Update ()
    {
        IPValue.text = Network.player.ipAddress;
        if(!TCPServer.connectionEstablished)
        {
            SuccessIcon.SetActive(false);
            LoadingImage.transform.Rotate(0, 0, -2, Space.World);
            WaitText.text = "WAITING FOR CONNECTION...";
        } else
        {
            LoadingImage.SetActive(false);
            SuccessIcon.SetActive(true);
            WaitText.text = "CONNECTION ESTABLISHED!";
        }
    }

    public void Play ()
    {
        sceneFader.FadeTo(levelToLoad);
    }

    public void ConnectionStatusToggle(bool open)
    {
        ConnectionStatusCanvas.SetActive(open);
    }


    public void Quit ()
    {
        Debug.Log("Exciting...");
        Application.Quit();
    } 

    public void playHoverSound()
    {
        SoundManager.getInstance().menuHover.GetComponent<AudioSource>().Play();
    }
}
