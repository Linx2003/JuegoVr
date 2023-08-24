using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using UnityEngine;

public class AddButton : MonoBehaviour, IColliderEventHoverEnterHandler, IColliderEventHoverExitHandler
{
    public float moveDistance = 0.02f;
    public float animationDuration = 0.5f;
    public Transform buttonTransform;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isAnimating = false;

    private void Start()
    {
        originalPosition = buttonTransform.position;
        targetPosition = originalPosition - Vector3.up * moveDistance;
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        if (!isAnimating) // Verificar si no está en animación
        {
            StartCoroutine(AnimateButton());
        }
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // Puedes agregar aquí una acción adicional al salir del hover si lo deseas.
    }

    private IEnumerator AnimateButton()
    {
        isAnimating = true;

        Debug.Log("Button hovered: Add");

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
        CounterManager.IncrementCounter();
    }
}
