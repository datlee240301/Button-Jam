using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ButtonData {
    public Vector2Int gridPosition; 
    public Sprite buttonSprite; 
}

[CreateAssetMenu(fileName = "ButtonConfig", menuName = "ScriptableObjects/ButtonConfig", order = 1)]
public class ButtonConfig : ScriptableObject {
    public List<ButtonData> buttonsData; 
}
