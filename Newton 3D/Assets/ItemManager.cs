using TMPro;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject itemPrefab; // Reference to the prefab for displaying items
    public Transform itemsParent; // Parent object to contain the instantiated items
    public SO_ItemsToCollect[] itemDatabases; // Array of ItemDatabase objects representing collections of items

    public Vector2 itemOffset = new Vector2(0f, -50f); // Offset between each item

    public int currentCollectionIndex = 1; // Index of the current collection being displayed

    void Start()
    {
        // Display the first collection of items
        DisplayCollection(currentCollectionIndex);
    }

    // Function to display items from a specific collection
    void DisplayCollection(int collectionIndex)
    {
        if (collectionIndex >= 0 && collectionIndex < itemDatabases.Length)
        {
            string[] items = itemDatabases[collectionIndex].items;

            // Calculate initial position
            Vector3 spawnPosition = itemsParent.position;

            // Instantiate UI elements for each item
            foreach (string item in items)
            {
                GameObject newItem = Instantiate(itemPrefab, itemsParent);
                TextMeshProUGUI textComponent = newItem.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = item; // Set the text content
                }

                // Set the position of the new item
                newItem.transform.position = spawnPosition;

                // Adjust spawn position for the next item
                spawnPosition += new Vector3(itemOffset.x, itemOffset.y, 0f);
            }
        }
        else
        {
            Debug.LogWarning("Invalid collection index.");
        }
    }

    // Function to switch to the next collection of items
    public void SwitchToNextCollection()
    {
        currentCollectionIndex++;
        if (currentCollectionIndex < itemDatabases.Length)
        {
            // Clear existing items before displaying the next collection
            foreach (Transform child in itemsParent)
            {
                Destroy(child.gameObject);
            }

            // Display the next collection of items
            DisplayCollection(currentCollectionIndex);
        }
        else
        {
            Debug.LogWarning("No more collections available.");
        }
    }
}
