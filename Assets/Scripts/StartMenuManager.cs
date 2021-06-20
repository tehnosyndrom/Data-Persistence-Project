using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject recordsBtn;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private AudioSource audioPlayer;
    private float soundVolume = 0.4f;
    [SerializeField] private AudioClip audioSelect;
    private const int minNameLength = 1;
    private const int maxNameLength = 8;
    private static string playerName;
    public string PlayerName
    {
        get
        {
            return playerName;
        }
        set
        {
            string input;
            Regex r = new Regex(@"\s+");
            input = r.Replace(value, @" ");

            if (input.Length < minNameLength || input == " ") playerName = defaultPlayerName;
            else playerName = value;
        }

    }
    private const string defaultPlayerName = "User";


    private void Start()
    {
        nameInput.characterLimit = maxNameLength;
        PlayerName = defaultPlayerName;
    }

    public void OnSelect(GameObject obj)
    {
        audioPlayer.volume = soundVolume;
        audioPlayer.PlayOneShot(audioSelect);
        obj.GetComponent<Animator>().SetTrigger("Selected");

    }

    // load game scene
    public void OnClickStart()
    {
        
        SceneManager.LoadScene("main");
    }

    public void OnClickRecords()
    {

    }

    public void ReadStringInput(string inputText)
    {
        PlayerName = inputText;
        dynamic gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.PlayerName = PlayerName;

        //Debug.Log(gm.PlayerName);
    }
}
