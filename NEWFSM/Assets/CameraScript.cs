using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseParallax : MonoBehaviour
{
    // Reference to the Cinemachine Virtual Camera
    public CinemachineVirtualCamera virtualCamera;

    // Define the range where the parallax effect should be applied
    public float parallaxRange = 1.0f;

    // Define the speed at which the camera follows the mouse
    public float followSpeed = 5.0f;

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a point in the game world
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(
                mousePosition.x,
                mousePosition.y,
                virtualCamera.transform.position.y - Camera.main.transform.position.z
            )
        );

        // Calculate the target position for the virtual camera
        float targetX = Mathf.Clamp(worldMousePosition.x, -parallaxRange, parallaxRange);
        float targetY = Mathf.Clamp(worldMousePosition.y, -parallaxRange, parallaxRange);
        Vector3 targetPosition = new Vector3(targetX, targetY, virtualCamera.transform.position.z);

        // Smoothly move the virtual camera towards the target position
        virtualCamera.transform.position = Vector3.Lerp(
            virtualCamera.transform.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );
    }

    public Animator animator;

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
