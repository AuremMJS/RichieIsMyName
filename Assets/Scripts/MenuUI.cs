using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayButtonClicked()
    {
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
}
