using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPawn: MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider armorSlider;

    [SerializeField] private TMP_Text healthtext;
    [SerializeField] private TMP_Text armortext;

    [SerializeField] private int MaxPackhealth = 100;
    public int currenthealth;

    [SerializeField] private int MaxArmorGreen  = 100;
    [SerializeField] private int MaxArmorBlue   = 200;
    [SerializeField] private int armortype      = 0; // Armor Type (None, Green, Blue)
    [SerializeField] private int armorpoints    = 0; // Armor Points

    private void Start()
    {
        currenthealth = MaxPackhealth;
        healthSlider.maxValue = MaxPackhealth;
        healthSlider.minValue = 0;

        armorSlider.maxValue = MaxArmorGreen;

        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (armortype > 0 && damage > 0)
        {
            int saved = armortype == 1 ? damage / 3 : damage / 2; 
            if (armorpoints <= saved)
            {
                saved = armorpoints;
                armortype = 0;
            }

            armorpoints -= saved;
            damage -= saved;
        }

        currenthealth -= damage;
        if (currenthealth < 0)
        {
            currenthealth = 0;
        }

        if (currenthealth > MaxPackhealth)
            currenthealth = MaxPackhealth;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currenthealth;
        }

        if (healthtext != null)
        {
            healthtext.text = currenthealth.ToString();
        }

        if (armorSlider != null)
        {
            armorSlider.value = armorpoints;
        }

        if (armortext != null)
        {
            armortext.text = armorpoints.ToString();
        }
    }
}