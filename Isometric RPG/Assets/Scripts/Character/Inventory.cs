using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    VisualElement PlayerStatsContainer;
    VisualElement PlayerStatsHolder;
    VisualElement Statsholder;
    ScrollView StatsContainer;
    private VisualElement ObjectContainer;

    private VisualTreeAsset playerStats;
    public bool isStatsOpen;

    Stats stats;
    PlayerCurrentWeapon playerCurrentWeapon;

    void Awake()
    {
        stats = GetComponent<Stats>();
        playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;

        PlayerStatsContainer = root.Q<VisualElement>("PlayerStatsContainer");
        PlayerStatsHolder = PlayerStatsContainer.Q<VisualElement>("PlayerStatsHolder");
        Statsholder = PlayerStatsHolder.Q<VisualElement>("Statsholder");
        StatsContainer = Statsholder.Q<ScrollView>("StatsContainer");
        ObjectContainer = StatsContainer.Q<VisualElement>("unity-content-container");

        playerStats = Resources.Load<VisualTreeAsset>("Stats");
        PopulateStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isStatsOpen = !isStatsOpen;
            PlayerStatsContainer.style.display = isStatsOpen
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }
    }

    public void PopulateStats()
    {
        ObjectContainer.Clear(); // Clear any existing stat elements

        // Create a list of stats
        List<Stat> statsList = new List<Stat>
        {
            new Stat("Damage", stats.baseStats.baseDamage, playerCurrentWeapon.weapon.Damage), // Replace with appropriate current values
            new Stat("Movement Speed", stats.baseStats.baseMovementSpeed, 0), // Replace with appropriate current values
            new Stat(
                "Cooldown Speed",
                stats.baseStats.baseCooldownSpeed,
                playerCurrentWeapon.weapon.abilityCooldown
            ),
            new Stat("Health", stats.baseStats.baseHealth, 0),
            new Stat("Defense", stats.baseStats.baseDefense, 0),
            new Stat(
                "Ability Power",
                stats.baseStats.abilityPower,
                playerCurrentWeapon.weapon.abilityPower
            ),
            new Stat(
                "Ability Cooldown",
                stats.baseStats.abilityCooldown,
                playerCurrentWeapon.weapon.abilityCooldown
            ),
            new Stat(
                "Ability Duration",
                stats.baseStats.abilityDuration,
                playerCurrentWeapon.weapon.abilityDuration
            ),
            new Stat("Energy", stats.baseStats.energy, 0),
            new Stat("Energy Regen Rate", stats.energyRegenRate, 0),
            new Stat(
                "Critical Chance",
                stats.baseStats.criticalChance,
                playerCurrentWeapon.weapon.criticalChance
            ),
            new Stat(
                "Critical Damage",
                stats.baseStats.criticalDamage,
                playerCurrentWeapon.weapon.criticalDamage
            ),
            new Stat(
                "Attack Speed",
                stats.baseStats.attackSpeed,
                playerCurrentWeapon.weapon.attackSpeed
            ),
            new Stat("Range", stats.baseStats.range, playerCurrentWeapon.weapon.range),
            new Stat(
                "Armor Penetration",
                stats.baseStats.armorPenetration,
                playerCurrentWeapon.weapon.armorPenetration
            ),
            new Stat(
                "Dodge Chance",
                stats.baseStats.dodgeChance,
                playerCurrentWeapon.weapon.dodgeChance
            ),
            new Stat(
                "Healing Received",
                stats.baseStats.healingReceived,
                playerCurrentWeapon.weapon.healingReceived
            ),
            new Stat("Life Steal", stats.baseStats.lifeSteal, playerCurrentWeapon.weapon.lifeSteal),
            new Stat("Knockback", stats.baseStats.Knockback, playerCurrentWeapon.weapon.Knockback)
        };
        foreach (var stat in statsList)
        {
            var statElement = playerStats.CloneTree(); // Clone the VisualTreeAsset
            // statElement.Q<Label>("StatName").text = stat.Key;
            // statElement.Q<Label>("StatValue").text = stat.Value.ToString();

            VisualElement Statname = statElement.Q<VisualElement>("StatName");
            Label KeyLabel = Statname.Q<Label>("KeyLabel");
            KeyLabel.text = stat.Name;

            VisualElement StatHolder = statElement.Q<VisualElement>("Statholder");
            Label BaseStat = StatHolder?.Q<Label>("BaseStat");
            Label Addition = StatHolder?.Q<Label>("Addition");

            BaseStat.text = stat.BaseValue.ToString() + "   +";
            Addition.text = stat.CurrentValue.ToString();

            ObjectContainer.Add(statElement);
        }
    }
}

public struct Stat
{
    public string Name;
    public float BaseValue;
    public float CurrentValue;

    public Stat(string name, float baseValue, float currentValue)
    {
        Name = name;
        BaseValue = baseValue;
        CurrentValue = currentValue;
    }
}
