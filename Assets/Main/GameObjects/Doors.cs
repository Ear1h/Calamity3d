using System.Collections;
using TMPro;
using UnityEngine;

// Classic Doom Door
public class DoorController: MonoBehaviour
{
    /*
     * 
     * Создание дверей в стиле Quake/Doom
     * 
     * 1. Необходимо добавить следующие свойства и флаги
     * 1.1. Lip - насколько метров должен подниматься дверь. Если lip = 0 - дверь поднимается на N позицию от своего размера
     * 1.2. Delay - задержка, через которую дверь открывается спустя N времени
     * 1.3. KeyType - необходимый ключ для активации двери.
     * 1.4. Скорость открытия двери. Speed
     * 1.5. Урон 
     * 
     * 2. Флаги
     * 2.1 Start Open - дверь открывается с самого начала
     * 2.2. Lerp - дверь открывается плавно со старта и около конца
     * 2.3 Toggle
     */

    public bool isDoorOpen = false;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Vector3 SlideUp = Vector3.up;
    [SerializeField] private float lip = 2.0f;

    private Vector3 StartPosition;
    private Vector3 Up;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        Up = transform.up;
        StartPosition = transform.position;

        if (lip == 0)
        {
            var lip_mesh = GetComponent<Collider>();
            lip = lip_mesh.bounds.size.x;
        }
    }

    public void Open(Vector3 UserPosition)
    {
        if (!isDoorOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            
            AnimationCoroutine = StartCoroutine(DoSlidingOpen());
           
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + lip * SlideUp;
        Vector3 startPosition = transform.position;

        float time = 0;
        isDoorOpen = true;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isDoorOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingClose());
        }
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;
        float time = 0;

        isDoorOpen = false;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
