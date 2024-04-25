using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private LevelsSO levelsSO;

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("Level") - 1;
        GameController.Instance.LoadGrid(levelsSO.levels[level], PlayerPrefs.GetInt("IsResumed") == 1);
    }
}
