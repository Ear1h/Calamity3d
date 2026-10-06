using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BaseStatusBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider armorSlider;

    [SerializeField] private TMP_Text healthtext;
    [SerializeField] private TMP_Text armortext;

    [SerializeField] private PlayerPawn player;

    private void Start()
    {

        healthSlider.maxValue = player.GetMaxHealth();
        healthSlider.minValue = 0;

        player.OnHealthChanged += UpdateHealth;

        UpdateHealth(player.GetHealth());
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnHealthChanged -= UpdateHealth;
    }

    // Find Armor point
    public void UpdateHealth(int health)
    {
        healthSlider.value = health;
        healthtext.text = health.ToString();
    }
}