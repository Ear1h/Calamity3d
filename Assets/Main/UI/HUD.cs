using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System;

[System.Serializable]
public class HeartFrame
{
    public Sprite sprite;
    public float delay = 0.1f;
}


public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }
    private int showhealth, realhealth;
    private bool healthInit;
    private bool deathHeartStarted;

    [SerializeField] private Text healthtext;
    [SerializeField] private PlayerPawn player;

    [SerializeField] private Image DeathheartImage;
    [SerializeField] private HeartFrame[] deathFrames;

    [SerializeField] private Image HeartImage;

    [SerializeField] private Image DamageOverlay;
    [SerializeField] private Image SpawnPlayerOverlay;

    private float ColorIntensive = 500f;

    private bool StartPlayer;

    private void Start()
    {
        player.OnHealthChanged += UpdateHealth;
        StartPlayer = true;
        UpdateHealth(player.GetHealth());
    }

    protected virtual void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        if (player != null)
            player.OnHealthChanged -= UpdateHealth;
    }

    private void Update()
    {
        showhealth = SmoothCount(showhealth, realhealth);
        healthtext.text = showhealth.ToString();

        if (PlayerPawn.DamageCount > 0)
        {
            PlayerPawn.DamageCount = Mathf.Max(0f, PlayerPawn.DamageCount - Time.deltaTime * 35f);
        }

        float intensity = PlayerPawn.DamageCount / 1000;

        DamageOverlay.color = new Color(1f, 0f, 0f, intensity);

        if (StartPlayer && ColorIntensive > 0)
        {
            ColorIntensive--;
            SpawnPlayerOverlay.color = new Color(0f, 0f, 0f, ColorIntensive/500);
        }
    }

    public void UpdateHealth(int health)
    {
        realhealth = health;

        if (!healthInit)
        {
            showhealth = health;
            healthtext.text = health.ToString();
            healthInit = true;
        }
    }
   
    public void StartDeathHeart()
    {
        HeartImage.gameObject.SetActive(false);

        if (deathHeartStarted)
            return;

        deathHeartStarted = true;
        StartCoroutine(DeathHeart());
    }

    public IEnumerator DeathHeart()
    {
        foreach (HeartFrame frame in deathFrames)
        {
            DeathheartImage.sprite = frame.sprite;
            yield return new WaitForSeconds(frame.delay);
        }
    }

    public int SmoothCount(int shownval, int realval)
    {
        int step = realval - shownval;
        if (step == 0)
            return realval;

        else
        {
            int sign = step < 0 ? -1 : 1;

            step = Mathf.Clamp(Mathf.Abs(step) / 40 + 1, 1, 3);

            shownval += step * sign;

            if ((sign > 0 && shownval > realval)
                || (sign < 0 && shownval < realval))
            {
                shownval = realval;
            }

            return shownval;
        }

    }
}