using UnityEngine;
using UnityEngine.InputSystem;

using System.Collections;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Transform destination;
    private bool playerInRange = false;
    private bool interactionTriggered = false;

    //Referencia al Canvas para el Fade
    public Animator transition;
    public float transitionTime = 1f;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (playerInRange && context.performed && !interactionTriggered)
        {
            interactionTriggered = true;

            StartCoroutine(FadeAndTeleport()); //Inicia el Fade y la Teltransportacion

            //TeleportPlayer();
            //interactionTriggered = false;
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

    private IEnumerator FadeAndTeleport()
    {
        // Activa el Canvas y reproduce la animación
        transition.gameObject.SetActive(true);
        transition.SetTrigger("Start"); // Usa un Trigger en el Animator llamado "Start"

        // Espera a que termine la animación
        yield return new WaitForSeconds(transitionTime);

        // Teletransportar al jugador
        TeleportPlayer();

        // Desactiva el Canvas
        transition.gameObject.SetActive(false);

        interactionTriggered = false;
    }
}
