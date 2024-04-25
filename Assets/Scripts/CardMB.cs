using UnityEngine;
using UnityEngine.UI;

public class CardMB : MonoBehaviour
{
    [SerializeField]
    private Image symbolImage;

    private Animator animator;
    private Button button;
    private int index;

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
        AudioController.Instance.PlayAudio("FlipCard");
        GridGenerator.Instance.EvaluateOpenedCards(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
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

    public void CloseCard()
    {
        button.interactable = true;
        animator.SetTrigger("FlipClose");
    }

    public void OpenCard()
    {
        button.interactable = false;
        animator.Play("Flip");
    }

    public void CloseCardImmediate()
    {
        button.interactable = true;
        animator.Play("FlipClose");
    }

    public void DisableCard()
    {
        button.interactable = false;
    }
}
