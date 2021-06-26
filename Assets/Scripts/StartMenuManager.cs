using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button recordsBtn;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private AudioSource audioPlayer;
    private float soundVolume = 0.4f;
    [SerializeField] private AudioClip audioSelect;
    private const int maxNameLength = 8;
    
    private void Start()
    {
        nameInput.characterLimit = maxNameLength;
        startBtn.Select();
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
        SceneManager.LoadScene("records_menu");
    }

    public void ReadStringInput()
    {
        dynamic gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.PlayerName = nameInput.GetComponent<TMP_InputField>().text;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
