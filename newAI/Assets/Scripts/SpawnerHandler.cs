using UnityEngine;

public class SpawnerHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rbPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = 10;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Instantiate(rbPrefab, worldMousePosition, Quaternion.identity);
    }
}
