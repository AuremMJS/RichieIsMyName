using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private LevelsSO levelsSO;

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("Level") - 1;
        GridGenerator.Instance.GenerateGrid(levelsSO.levels[level]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
