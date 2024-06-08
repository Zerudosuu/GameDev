using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    #region Basic Stats
    public float BaseHealth;
    public float CurrentHealth;

    public float BaseArmor; // New attribute for base armor
    public float CurrentArmor; // Current armor

    public float BaseMana;
    public float CurrentMana;
    private float experienceThreshold = 100f; // Threshold for gaining a card
    private float accumulatedExperience = 0f;

    public float BaseExperience;
    public float CurrentExperience;

    #endregion

    #region UIToolkit
    VisualElement HealthContainer;
    ProgressBar HealthBar;
    ProgressBar Mana;
    ProgressBar ExperienceBar;

    VisualElement ExperienceRewardContainer;
    VisualElement ExperienceRewardHolder;
    ScrollView ExperienceRewardScroller;

    VisualElement Experiece;

    VisualTreeAsset ExperienceReward;

    #endregion// Add experience progress bar

    private float lerpDuration = 0.5f; // Duration for the health bar to update

    private PlayerRespawn playerRespawn;

    void Start()
    {
        playerRespawn = GetComponent<PlayerRespawn>();

        var root = GameObject.FindObjectOfType<UIDocument>().rootVisualElement;

        HealthContainer = root.Q<VisualElement>("HealthContainer");
        HealthBar = HealthContainer.Q<ProgressBar>("Health");
        Mana = HealthContainer.Q<ProgressBar>("Mana");
        ExperienceBar = HealthContainer.Q<ProgressBar>("Experience"); // Get the experience bar

        ExperienceRewardContainer = root.Q<VisualElement>("ExperienceRewardContainer");
        ExperienceRewardHolder = ExperienceRewardContainer.Q<VisualElement>(
            "ExperienceRewardHolder"
        );
        ExperienceRewardScroller = ExperienceRewardHolder.Q<ScrollView>("ExperienceRewardScroller");

        Experiece = ExperienceRewardScroller.Q<VisualElement>("unity-content-container");

        ExperienceReward = Resources.Load<VisualTreeAsset>("ExperienceReward");

        CurrentMana = BaseMana;
        CurrentHealth = BaseHealth;
        CurrentArmor = BaseArmor;
        accumulatedExperience = 0; // Initialize current experience

        if (HealthBar != null)
        {
            HealthBar.highValue = BaseHealth;
            HealthBar.lowValue = 0;
            HealthBar.value = CurrentHealth;

            Mana.highValue = BaseMana;
            Mana.lowValue = 0;
            Mana.value = CurrentMana;

            ExperienceBar.highValue = experienceThreshold; // Set the experience bar values
            ExperienceBar.lowValue = 0;
            ExperienceBar.value = accumulatedExperience;
        }
        else
        {
            Debug.LogError("HealthBar or ManaBar or ExperienceBar not found!");
        }
    }

    public void TakeDamage(float damage)
    {
        float remainingDamage = damage;

        // Reduce armor first
        if (CurrentArmor > 0)
        {
            if (CurrentArmor >= damage)
            {
                CurrentArmor -= damage;
                remainingDamage = 0;
            }
            else
            {
                remainingDamage -= CurrentArmor;
                CurrentArmor = 0;
            }
        }

        // Apply remaining damage to health
        float newHealth = CurrentHealth - remainingDamage;
        if (newHealth < 0)
            newHealth = 0;

        StartCoroutine(UpdateHealthBar(CurrentHealth, newHealth, lerpDuration));
        CurrentHealth = newHealth;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void ManaReduction(float manasubtraction)
    {
        float newMana = CurrentMana - manasubtraction;
        if (newMana < 0)
            newMana = 0;

        StartCoroutine(UpdateManaBar(CurrentMana, newMana, lerpDuration));
        CurrentMana = newMana;

        print("Mana Reduce");
    }

    public void ExperienceUp(float experience)
    {
        accumulatedExperience += experience;
        float newExperience = CurrentExperience + experience;

        // Check and handle level up
        if (accumulatedExperience >= experienceThreshold)
        {
            accumulatedExperience -= experienceThreshold;
            ExperienceRewardContainer.style.display = DisplayStyle.Flex;
            Time.timeScale = 0;
            newExperience = 0;
            GenerateRandomCards();
        }

        StartCoroutine(UpdateExperienceBar(CurrentExperience, newExperience, lerpDuration));
        CurrentExperience = newExperience;
    }

    private void GenerateRandomCards()
    {
        List<Card> cards = new List<Card>
        {
            new Card
            {
                cardName = "Health Boost",
                description = "Increases your health by 20 points.",
                statModifications = new List<StatModification>
                {
                    new StatModification { statType = StatType.Health, value = 20f }
                }
            },
            new Card
            {
                cardName = "Mana Boost",
                description = "Increases your mana by 10 points.",
                statModifications = new List<StatModification>
                {
                    new StatModification { statType = StatType.Mana, value = 10f }
                }
            },
            new Card
            {
                cardName = "Armor Boost",
                description = "Increases your armor by 5 points.",
                statModifications = new List<StatModification>
                {
                    new StatModification { statType = StatType.Armor, value = 5f }
                }
            }
        };

        // Pick 3 random cards
        List<Card> randomCards = new List<Card>();
        while (randomCards.Count < 3)
        {
            Card randomCard = cards[Random.Range(0, cards.Count)];
            if (!randomCards.Contains(randomCard))
            {
                randomCards.Add(randomCard);
            }
        }

        PresentCardsToPlayer(randomCards);
    }

    private void PresentCardsToPlayer(List<Card> cards)
    {
        // Implement your UI to show the cards and let the player pick one
        // For simplicity, we will just pick the first card in this example

        foreach (var card in cards)
        {
            var statElement = ExperienceReward.CloneTree();
            Button pick = statElement.Q<Button>("Reward");

            Label details = pick.Q<Label>("Details");
            details.text = card.description;

            pick.RegisterCallback<ClickEvent>(evt => ApplyCard(card));

            Experiece.Add(statElement);
        }
    }

    private void ApplyCard(Card card)
    {
        ExperienceRewardContainer.style.display = DisplayStyle.None;
        Time.timeScale = 1;

        foreach (var modification in card.statModifications)
        {
            ApplyStatModification(modification);
        }
        UpdateUI();
    }

    private void ApplyStatModification(StatModification modification)
    {
        switch (modification.statType)
        {
            case StatType.Health:
                BaseHealth += modification.value;
                break;
            case StatType.Mana:
                BaseMana += modification.value;
                break;
            case StatType.Armor:
                BaseArmor += modification.value;
                break;
            case StatType.Damage:
                Stats stats = GetComponent<Stats>();
                stats.baseDamage += modification.value;
                break;
            // Add cases for other stat types here
        }
    }

    private void UpdateUI()
    {
        // Update the health bar
        HealthBar.highValue = BaseHealth;
        HealthBar.value = CurrentHealth;

        // Update the mana bar
        Mana.highValue = BaseMana;
        Mana.value = CurrentMana;

        // Update the experience bar
        ExperienceBar.highValue = experienceThreshold;
        ExperienceBar.value = accumulatedExperience;
    }

    public void Heal(float healing)
    {
        float newHealth = CurrentHealth + healing;
        if (newHealth > BaseHealth)
            newHealth = BaseHealth;

        StartCoroutine(UpdateHealthBar(CurrentHealth, newHealth, lerpDuration));
        CurrentHealth = newHealth;
    }

    public void ManaUp(float mana)
    {
        float newMana = CurrentMana + mana;
        if (newMana > BaseMana)
            newMana = BaseMana;

        if (newMana < 0)
            newMana = 0;

        StartCoroutine(UpdateManaBar(CurrentMana, newMana, lerpDuration));
        CurrentMana = newMana;
    }

    private IEnumerator UpdateHealthBar(float startHealth, float endHealth, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            HealthBar.value = Mathf.Lerp(startHealth, endHealth, elapsed / duration);
            yield return null;
        }

        HealthBar.value = endHealth; // Ensure the final value is set
    }

    private IEnumerator UpdateManaBar(float startMana, float endMana, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Mana.value = Mathf.Lerp(startMana, endMana, elapsed / duration);
            yield return null;
        }

        Mana.value = endMana; // Ensure the final value is set
    }

    private IEnumerator UpdateExperienceBar(
        float startExperience,
        float endExperience,
        float duration
    )
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ExperienceBar.value = Mathf.Lerp(startExperience, endExperience, elapsed / duration);
            yield return null;
        }

        ExperienceBar.value = endExperience; // Ensure the final value is set
    }

    public void ResetHealth()
    {
        CurrentHealth = BaseHealth;
        StartCoroutine(UpdateHealthBar(CurrentHealth, BaseHealth, lerpDuration));
    }

    public void ResetArmor()
    {
        CurrentArmor = BaseArmor;
    }

    public void ResetMana()
    {
        CurrentMana = BaseMana;
        StartCoroutine(UpdateManaBar(CurrentMana, BaseMana, lerpDuration));
    }

    void Die()
    {
        Debug.Log("Player has died.");
        if (playerRespawn != null)
        {
            playerRespawn.Die();
        }
    }
}

[System.Serializable]
public class Card
{
    public string cardName;
    public string description;
    public List<StatModification> statModifications;
}

public class StatModification
{
    public StatType statType;
    public float value;
}

public enum StatType
{
    Health,
    Mana,
    Armor,
    Damage,
    // Add other stat types here
}
