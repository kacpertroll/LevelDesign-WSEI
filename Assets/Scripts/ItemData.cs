using UnityEngine;

/// <summary>
/// ScriptableObject definiujący przedmiot w ekwipunku.
///
/// Jak stworzyć: PPM w Project → Create → Inventory → Item
/// </summary>
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "Przedmiot";
    [TextArea] public string description = "";
    public Sprite icon;
}
