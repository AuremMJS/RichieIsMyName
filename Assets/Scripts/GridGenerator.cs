using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private SymbolSpritesSO symbolSpritesSO;

    private float height, width;
    private GridLayoutGroup grid;
    private CardMB[] cards;
    private Dictionary<int, int> cardPairs;
    private List<Symbol> generatedSymbols;
    public static GridGenerator Instance;
    int noOfSymbolsAvailable;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        grid = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        height = rectTransform.rect.height;
        width = rectTransform.rect.width;

        cardPairs = new Dictionary<int, int>();
        generatedSymbols = new List<Symbol>();
        noOfSymbolsAvailable = Enum.GetValues(typeof(Symbol)).Length;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid(LevelConfigurationSO level)
    {
        int rows = level.Rows;
        int cols = level.Columns;
        int maxDim = rows > cols ? rows : cols;
        grid.cellSize = new Vector2(width / maxDim, height / maxDim);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;
        cards = new CardMB[rows * cols];
        for (int i = 0; i < rows * cols; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform);
            CardMB cardMB = card.GetComponent<CardMB>();
            cards[i] = cardMB;
            if (!cardPairs.ContainsKey(i))
            {
                int random = -1;
                do
                {
                    random = UnityEngine.Random.Range(0, rows * cols);

                } while (random == i || cardPairs.ContainsKey(random));
                cardPairs.Add(i, random);
                cardPairs.Add(random, i);
                Symbol symbol = Symbol.None;
                do
                {
                    symbol = (Symbol)UnityEngine.Random.Range(0, noOfSymbolsAvailable - 1);
                } while (generatedSymbols.Contains(symbol));
                generatedSymbols.Add(symbol);
                cards[i].SetSymbolImage(symbolSpritesSO.GetSpriteForSymbol(symbol));
            }
            else
            {
                cards[i].SetSymbolImage(cards[cardPairs[i]].GetSymbolImage());
            }
        }
    }
}
