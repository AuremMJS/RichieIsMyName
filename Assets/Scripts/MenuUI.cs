using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Slider difficultSlider;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        if (GameDataService.Instance.IsSavedGameData())
        {
            resumeButton.gameObject.SetActive(true);
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        GameDataService.Instance.GameData = new GameData();
        PlayerPrefs.SetInt("Level", (int)difficultSlider.value);
        PlayerPrefs.SetInt("IsResumed", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }

    void OnResumeButtonClicked()
    {
        GameDataService.Instance.LoadGameData();
        PlayerPrefs.SetInt("Level", GameDataService.Instance.GameData.LevelIndex);
        PlayerPrefs.SetInt("IsResumed", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
