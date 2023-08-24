using HTC.UnityPlugin.ColliderEvent;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IColliderEventHoverEnterHandler, IColliderEventHoverExitHandler
{
    public GameObject carModel; // Arrastra el modelo 3D del carro aquí en el Inspector
    public string animationTriggerName = "PlayAnimation"; // Nombre del trigger de la animación

    public float moveDistance = 0.02f; // Distancia de movimiento vertical del botón
    public float animationDuration = 0.5f; // Duración de la animación del botón
    public float waitBeforeNextIteration = 0.5f; 

    public Text lapCountText; // Referencia al objeto Text para mostrar las vueltas que ha dado
    public Text totalLapsText; // Referencia al objeto Text para mostrar el total de vueltas

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isAnimating = false;
    private bool isAnimationRunning = false; // Booleano para verificar si la animación está en curso
    private int initialCounterValue;
    private int animationCount = 1;

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        if (!isAnimating) // Verificar si no está en animación
        {
            initialCounterValue = CounterManager.CollisionCount; // Guardar el valor inicial del contador
            Debug.Log("Initial Counter Value: " + initialCounterValue); // Agregar este log para verificar el contador inicial
            
            // Reiniciar el contador del CounterManager
            CounterManager.CollisionCount = 0;
            StartCoroutine(PlayButtonAndCarAnimation());
        }
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // Puedes agregar aquí una acción adicional al salir del hover si lo deseas.
    }

    private IEnumerator PlayButtonAndCarAnimation()
    {
        isAnimating = true;
        animationCount = 1;

        // Animación del botón de arriba hacia abajo
        originalPosition = transform.position;
        targetPosition = originalPosition - Vector3.up * moveDistance;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Animación del carro
        Animator carAnimator = carModel.GetComponent<Animator>();
        if (carAnimator != null)
        {
            while (animationCount <= initialCounterValue) // Repetir hasta alcanzar el número deseado de repeticiones
            {
                // Actualizar el texto de las vueltas
                lapCountText.text = animationCount.ToString();
                totalLapsText.text = initialCounterValue.ToString();

                Debug.Log("Repeating animation, count: " + animationCount);
                carAnimator.SetTrigger(animationTriggerName);
                yield return StartCoroutine(WaitForAnimationToEnd(carAnimator)); // Esperar a que termine la animación
                animationCount++;

                yield return new WaitForSeconds(waitBeforeNextIteration); // Esperar antes de la siguiente iteración
            }
        }

        // Restaurar los textos a 0 después de las repeticiones
        lapCountText.text = "0";
        totalLapsText.text = "0";

        Debug.Log("Animation Count: " + animationCount);

        isAnimating = false;
    }

    private IEnumerator WaitForAnimationToEnd(Animator animator)
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }
}
