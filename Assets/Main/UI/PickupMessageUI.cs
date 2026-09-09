using System.Collections;
using TMPro;
using UnityEngine;

public class PickupMessageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    private Coroutine messageCoroutine;

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