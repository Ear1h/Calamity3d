using System.Collections;
using TMPro;
using UnityEngine;

public class PickupMessageUI : MonoBehaviour
{
    public static PickupMessageUI Instance { get; private set; }
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
     
    public void ShowMessage(string message, float delay)
    {
        messageText.text = message;

        if (messageCoroutine != null)
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(ClearMessage(delay));
    }

    private IEnumerator ClearMessage(float delay)
    {
        yield return new WaitForSeconds(delay);

        messageText.text = "";
        messageCoroutine = null;
    }
}