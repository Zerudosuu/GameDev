using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    VisualElement ShopContainer;
    ScrollView scrollView;

    VisualTreeAsset ShopItem;

    Button close;

    [SerializeField]
    private Stats playerStats;

    public GameObject player;

    void Awake()
    {
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;

        ShopContainer = root.Q<VisualElement>("Shop");
        scrollView = ShopContainer.Q<ScrollView>("ShopContainer");

        ShopItem = Resources.Load<VisualTreeAsset>("Shop");

        close = ShopContainer.Q<Button>("Closebutton");

        close.RegisterCallback<ClickEvent>(evt => CloseShop());

        PopulateSlots();

        // Start searching for playerStats immediately
        FindPlayerStats();
    }

    void FindPlayerStats()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<Stats>();
            if (playerStats != null)
            {
                print("hotototooasoda");
            }
            else
            {
                Debug.LogWarning("Stats component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found.");
        }
    }

    private void CloseShop()
    {
        ShopContainer.style.display = DisplayStyle.None;
    }

    public void OpenShop()
    {
        ShopContainer.style.display = DisplayStyle.Flex;
    }

    void PopulateSlots()
    {
        foreach (FieldInfo field in typeof(Stats).GetFields())
        {
            // Skip the first two fields by name
            if (field.Name == "baseStats" || field.Name == "playerCoins")
            {
                continue;
            }

            VisualElement shopItem = ShopItem.CloneTree();
            shopItem.Q<Label>("StatsName").text = field.Name;

            Button buyButton = shopItem.Q<Button>("StatBuy");

            // Create a local copy of the loop variable
            string localFieldName = field.Name;

            buyButton.RegisterCallback<ClickEvent>(evt => BuyStat(localFieldName));

            scrollView.Add(shopItem);
        }
    }

    void BuyStat(string statName)
    {
        float upgradeAmount = 2f; // Example upgrade amount

        if (playerStats != null)
            playerStats.BuyStat(statName, 14f, upgradeAmount);
        else
            print("playerstat not found");

        Inventory inventory = GameObject.FindObjectOfType<Inventory>();
        inventory.PopulateStats();
        UpdateUI();
    }

    void UpdateUI()
    {
        scrollView.Clear();
        PopulateSlots();
    }
}
