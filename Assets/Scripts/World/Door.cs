using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    public Transform destination;
    private bool playerInRange = false;
    private bool interactionTriggered = false;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && context.performed && !interactionTriggered)
        {
            interactionTriggered = true;
            TeleportPlayer();
            interactionTriggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Door Ready!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("You're too far Away now!");
        }
    }
    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = destination.position;
        }
    }
}
