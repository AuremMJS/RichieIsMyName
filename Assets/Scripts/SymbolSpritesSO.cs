using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SymbolSprite
{
    public Symbol symbol;
    public Sprite sprite;
}

// Scriptable object to store sprites for each symbol
[CreateAssetMenu(fileName = "SymbolSpritesSO", menuName = "ScriptableObjects/SymbolSpritesSO", order = 1)]
[Serializable]
public class SymbolSpritesSO : ScriptableObject
{
    public SymbolSprite[] symbolSprites;

    public Sprite GetSpriteForSymbol(Symbol symbol)
    {
        SymbolSprite symbolSprite = symbolSprites.FirstOrDefault(sprite => sprite.symbol == symbol);
        return symbolSprite.sprite;
    }
}
