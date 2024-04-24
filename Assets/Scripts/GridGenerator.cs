using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    private float height, width;
    private GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        height = rectTransform.rect.height;
        width = rectTransform.rect.width;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(3, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid(int rows, int cols)
    {
        int maxDim = rows > cols ? rows : cols;
        grid.cellSize = new Vector2(width/ maxDim, height/ maxDim);
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = rows;

        for (int i = 0; i < rows * cols; i++)
        {
            Instantiate(cardPrefab, transform);
        }
    }
}
