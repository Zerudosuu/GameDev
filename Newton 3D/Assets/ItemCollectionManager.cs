using System.Collections.Generic;
using UnityEngine;

public class ItemCollectionManager : MonoBehaviour
{
    public List<string> collectibleItems;

    private ItemManager iManager; // List to store collectible item names

    // Method to collect an item
    void Start()
    {
        iManager = GameObject.FindAnyObjectByType<ItemManager>();

        foreach (string item in iManager.itemDatabases[currentIndex].items)
        {
            collectibleItems.Add(item);
        }
    }

    public void CollectItem(string itemName)
    {
        if (collectibleItems.Contains(itemName))
        {
            collectibleItems.Remove(itemName);
            Debug.Log("Collected: " + itemName);

            // Add your collection event handling here
        }
    }

    // Example collision detection method
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a collectible item
        if (other.CompareTag("Collectible"))
        {
            // Collect the item
            CollectItem(other.gameObject.name);

            // Optionally, you can also destroy the collected object
            Destroy(other.gameObject);
        }
    }
}
