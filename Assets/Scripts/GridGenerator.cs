using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
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

    public static GridGenerator Instance;

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
        int maxDim = rows > cols ? rows : cols;
        grid.cellSize = new Vector2(width / maxDim, height / maxDim);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;
        cards = new CardMB[rows * cols];

        List<int> assignedSymbols = new List<int>();
        for (int i = 0; i < rows * cols; i++)
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

        if (isResumed)
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
    }

    private void GenerateGridData(LevelConfigurationSO level)
    {
        int rows = level.Rows;
        int cols = level.Columns;
        GameData.LevelIndex = PlayerPrefs.GetInt("Level");
        GameData.CardPairs = new int[rows * cols];
        GameData.GeneratedSymbols = new List<int>();
        GameData.OpenedCards = new List<int>();
        GameData.PrevOpenedCard = -1;
        GameData.Turns = 0;
        GameData.Score = 0;

        for (int i = 0; i < rows * cols; i++)
        {
            GameData.CardPairs[i] = -1;
        }

        for (int i = 0; i < rows * cols; i++)
        {
            if (GameData.CardPairs[i] == -1)
            {
                int random = -1;
                do
                {
                    random = UnityEngine.Random.Range(0, rows * cols);

                } while (random == i || GameData.CardPairs[random] != -1);
                GameData.CardPairs[i] = random;
                GameData.CardPairs[random] = i;

                Symbol symbol = Symbol.None;
                do
                {
                    symbol = (Symbol)UnityEngine.Random.Range(0, noOfSymbolsAvailable - 1);
                } while (GameData.GeneratedSymbols.Contains((int)symbol));
                GameData.GeneratedSymbols.Add((int)symbol);
            }
        }
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
                cards[GameData.PrevOpenedCard].CloseCard();
                cards[index].CloseCard();
                AudioController.Instance.PlayAudio("Mismatch");
            }
            else
            {
                cards[GameData.PrevOpenedCard].DisableCard();
                cards[index].DisableCard();
                AudioController.Instance.PlayAudio("Success");
                GameData.Score++;
                scoreText.text = GameData.Score.ToString();
                GameData.OpenedCards.Add(index);
                GameData.OpenedCards.Add(GameData.PrevOpenedCard);
                if (GameData.OpenedCards.Count == cards.Count())
                {
                    AudioController.Instance.PlayAudio("Victory", 1.0f);
                    SessionEnd();
                }
            }
            GameData.PrevOpenedCard = -1;
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
}
