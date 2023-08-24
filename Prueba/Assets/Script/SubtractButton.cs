using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using System.Collections.Generic; // Agrega esta línea
using UnityEngine;

public class SubtractButton : MonoBehaviour, IColliderEventHoverEnterHandler, IColliderEventHoverExitHandler
{
    public float moveDistance = 0.02f;
    public float animationDuration = 0.5f;
    public Transform buttonTransform;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isAnimating = false;

    private HashSet<ColliderHoverEventData> hoverEvents = new HashSet<ColliderHoverEventData>();

    private void Start()
    {
        originalPosition = buttonTransform.position;
        targetPosition = originalPosition - Vector3.up * moveDistance;
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        if (hoverEvents.Add(eventData) && hoverEvents.Count == 1 && !isAnimating)
        {
            StartCoroutine(AnimateButton());
        }
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        if (hoverEvents.Remove(eventData) && hoverEvents.Count == 0)
        {
            // Puedes agregar aquí una acción adicional al salir del hover si lo deseas.
        }
    }

    private IEnumerator AnimateButton()
    {
        isAnimating = true;

        Debug.Log("Button hovered: Subtract");

        float elapsedTime = 0f;
        Vector3 initialPosition = buttonTransform.position;

        while (elapsedTime < animationDuration)
        {
            buttonTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            buttonTransform.position = Vector3.Lerp(targetPosition, initialPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
        CounterManager.DecrementCounter();
    }
}
