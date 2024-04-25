using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private LevelsSO levelsSO;

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("Level") - 1;
        GridGenerator.Instance.LoadGrid(levelsSO.levels[level], PlayerPrefs.GetInt("IsResumed") == 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
