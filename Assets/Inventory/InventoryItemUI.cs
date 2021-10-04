using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public InventoryItem InventoryItem;
    public Color SelectedItemColor;
    public Color NormalItemColor; // new Color(0.65f, 0.53f, 0.36f, 1.0f); // #A6895D

    public Image ImageView;
    public Button ButtonView;
    public TextMeshProUGUI TextView;
    public PubSubSender EventSender;

    private bool isSelected = false;

    public static string ItemClickedKey = "inventory-item.clicked";

    private Sprite InventoryIcon
    {
        get
        {
            return InventoryItem.Item.InventoryIcon;
        }
    }

    private string InventoryText
    {
        get
        {
            return $"x{InventoryItem.Quantity}";
        }
    }

    public void Start()
    {
        RefreshUI();
        ButtonView.onClick.AddListener(() => {
            EventSender.Publish(ItemClickedKey, InventoryItem);
        });
    }

    public void ToggleSelection()
    {
        if (isSelected)
        {
            ShowUnselected();
        }
        else
        {
            ShowSelected();
        }
        isSelected = !isSelected;
    }

    public void ShowSelected()
    {
        ImageView.color = SelectedItemColor;
    }

    public void ShowUnselected()
    {
        ImageView.color = NormalItemColor;
    }

    public void RefreshUI()
    {
        if(!InventoryItem)
        {
            Debug.Log("Unable to refresh UI due to missing InventoryItem, aborting");
            return;
        }

        ImageView.color = NormalItemColor;
        TextView.text = InventoryText;
        ButtonView.image.sprite = InventoryIcon;
    }
}
