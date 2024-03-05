using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawElementUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text levelText;

    public void Setup(Item item)
    {
        iconImage.sprite = item.iconSprite;
        nameText.text = item.itemName;
        descriptionText.text = item.description;

        bool isNewItem = item.level == 1;
        levelText.text = isNewItem ? "신규 무기!" : $"레벨 : {item.level}";
        levelText.color = isNewItem ? Color.blue : Color.black;
    }
}
