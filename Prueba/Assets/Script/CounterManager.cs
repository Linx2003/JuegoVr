using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    public static int collisionCount = 0;
    public Text counterText;

    public static int CollisionCount
    {
        get { return collisionCount; }
        set
        {
            collisionCount = value;
            UpdateCounterText();
        }
    }

    private void Start()
    {
        UpdateCounterText();
    }

    public static void IncrementCounter()
    {
        collisionCount++;
        UpdateCounterText();
        Debug.Log("Counter Incremented. Total Count: " + collisionCount);
    }

    public static void DecrementCounter()
    {
        if (collisionCount > 0)
        {
            collisionCount--;
            UpdateCounterText();
            Debug.Log("Counter Decremented. Total Count: " + collisionCount);
        }
    }

    private static void UpdateCounterText()
    {
        if (instance != null && instance.counterText != null)
        {
            instance.counterText.text = collisionCount.ToString(); // Mostrar solo el número
        }
    }

    private static CounterManager instance;
    private void Awake()
    {
        instance = this;
    }
}
