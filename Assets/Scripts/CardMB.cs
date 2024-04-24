using UnityEngine;
using UnityEngine.UI;

public class CardMB : MonoBehaviour
{
    [SerializeField]
    private Image symbolImage;

    private Animator animator;
    private Button button;

    void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClicked()
    {
        animator.SetTrigger("Flip");
    }

    public void SetSymbolImage(Sprite sprite)
    {
        if (symbolImage != null) 
        {
            symbolImage.sprite = sprite;
        }
    }

    public Sprite GetSymbolImage()
    {
        return symbolImage.sprite;
    }
}
