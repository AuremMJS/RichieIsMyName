using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private SymbolSpritesSO symbolSpritesSO;
    [SerializeField]
    private TextMeshProUGUI turnsText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Button homeButton;

    private float height, width;
    private GridLayoutGroup grid;
    private CardMB[] cards;

    private int noOfSymbolsAvailable;

    public static GameController Instance;

    public GameData GameData
    {
        get
        {
            return GameDataService.Instance.GameData;
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        grid = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        height = rectTransform.rect.height;
        width = rectTransform.rect.width;

        noOfSymbolsAvailable = Enum.GetValues(typeof(Symbol)).Length;
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }

    private void OnHomeButtonClicked()
    {
        SessionEnd();
        SceneManager.LoadScene(0);
    }

    public void LoadGrid(LevelConfigurationSO level, bool isResumed = false)
    {
        if (!isResumed)
            GenerateGridData(level);
        int rows = level.Rows;
        int cols = level.Columns;
        
        InitGrid(rows, cols);
        CreateCards(rows * cols);

        if (isResumed)
        {
            ResumeGame();
        }
    }

    private void InitGrid(int rows, int cols)
    {
        int maxDim = rows > cols ? rows : cols;
        grid.cellSize = new Vector2(width / maxDim, height / maxDim);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;

    }

    private void CreateCards(int size)
    {
        cards = new CardMB[size];
        List<int> assignedSymbols = new List<int>();
        for (int i = 0; i < size; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            CardMB cardMB = card.GetComponent<CardMB>();
            cards[i] = cardMB;
            cardMB.SetIndex(i);
            int cardPair = GameData.CardPairs[i];
            if (!assignedSymbols.Contains(i) && !assignedSymbols.Contains(cardPair))
            {
                int symbolIndex = assignedSymbols.Count;
                Symbol symbol = (Symbol)GameData.GeneratedSymbols[symbolIndex];
                Sprite sprite = symbolSpritesSO.GetSpriteForSymbol(symbol);
                cards[i].SetSymbolImage(sprite);
                assignedSymbols.Add(i);
            }
            else
            {
                Sprite sprite = cards[cardPair].GetSymbolImage();
                cards[i].SetSymbolImage(sprite);
            }
        }
    }

    private void ResumeGame()
    {
        foreach (CardMB card in cards)
        {
            card.CloseCardImmediate();
        }

        foreach (int openedCard in GameData.OpenedCards)
        {
            cards[openedCard].OpenCard();
        }

        turnsText.text = GameData.Turns.ToString();
        scoreText.text = GameData.Score.ToString();
    }

    private void GenerateGridData(LevelConfigurationSO level)
    {
        int rows = level.Rows;
        int cols = level.Columns;
        InitGameData(rows * cols);
        InitCardPairs();
        
        for (int i = 0; i < rows * cols; i++)
        {
            if (GameData.CardPairs[i] == -1)
            {
                AssignRandomCardPair(i, rows * cols);
                GenerateRandomSymbol();
            }
        }
    }

    private void InitGameData(int size)
    {
        GameData.LevelIndex = PlayerPrefs.GetInt("Level");
        GameData.CardPairs = new int[size];
        GameData.GeneratedSymbols = new List<int>();
        GameData.OpenedCards = new List<int>();
        GameData.PrevOpenedCard = -1;
        GameData.Turns = 0;
        GameData.Score = 0;
    }

    private void InitCardPairs()
    {
        for (int i = 0; i < GameData.CardPairs.Length; i++)
        {
            GameData.CardPairs[i] = -1;
        }
    }

    private void AssignRandomCardPair(int index, int size)
    {
        int random = -1;
        do
        {
            random = UnityEngine.Random.Range(0, size);

        } while (random == index || GameData.CardPairs[random] != -1);
        GameData.CardPairs[index] = random;
        GameData.CardPairs[random] = index;

    }

    private void GenerateRandomSymbol()
    {
        Symbol symbol = Symbol.None;
        do
        {
            symbol = (Symbol)UnityEngine.Random.Range(0, noOfSymbolsAvailable - 1);
        } while (GameData.GeneratedSymbols.Contains((int)symbol));
        GameData.GeneratedSymbols.Add((int)symbol);
    }

    public void EvaluateOpenedCards(int index)
    {
        if (GameData.PrevOpenedCard == -1)
        {
            GameData.PrevOpenedCard = index;
            cards[index].DisableCard();
        }
        else
        {
            GameData.Turns++;
            turnsText.text = GameData.Turns.ToString();
            if (GameData.CardPairs[GameData.PrevOpenedCard] != index)
            {
                CardMismatch(index);
            }
            else
            {
                CardMatch(index);
                CheckForWin();
            }
            GameData.PrevOpenedCard = -1;
        }
    }

    private void CardMismatch(int index)
    {
        cards[GameData.PrevOpenedCard].CloseCard();
        cards[index].CloseCard();
        AudioController.Instance.PlayAudio("Mismatch");

    }

    private void CardMatch(int index)
    {
        cards[GameData.PrevOpenedCard].DisableCard();
        cards[index].DisableCard();
        AudioController.Instance.PlayAudio("Success");
        GameData.Score++;
        scoreText.text = GameData.Score.ToString();
        GameData.OpenedCards.Add(index);
        GameData.OpenedCards.Add(GameData.PrevOpenedCard);
    }

    private void CheckForWin()
    {
        if (GameData.OpenedCards.Count == cards.Count())
        {
            AudioController.Instance.PlayAudio("Victory", 1.0f);
            SessionEnd();
        }
    }

    private void SessionEnd()
    {
        if (GameData.OpenedCards.Count != cards.Length)
        {
            GameDataService.Instance.SaveGameData();
        }
        else
        {
            GameDataService.Instance.DeleteSavedGameData();
        }
    }

    private void OnApplicationQuit()
    {
        SessionEnd();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus == false)
        {
            SessionEnd();
        }
    }
}
