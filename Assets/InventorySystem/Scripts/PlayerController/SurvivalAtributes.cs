using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalAtributes : MonoBehaviour
{
    #region - Singleton Pattern -
    public static SurvivalAtributes Instance;
    private void Awake() => Instance = this;
    #endregion

    private PlayerController playerAsset;

    [Range(0, 150)] public float currentHealth = 100;
    public float maxHealth = 150;

    #region - Stamina Data -
    [Header("Stamina")]
    [Range(0, 100)] public float maxStamina = 100;
    [Range(0, 100)] public float currentStamina;
    [Range(0, 3)] public float staminaLossTax = 1f;
    [Range(0, 10)] public float staminaGainTax = 3f;
    #endregion

    #region - Thirst Data -
    [Header("Thirst")]
    [Range(0, 100)] public float currentThrist = 5f;
    [Range(0, 100)] public float maxThrist = 100f;
    [Range(0, 1)] public float thristLifeLossTax = 0.07f;
    [Range(0, 1)] public float thirstLossTax = 0.1f;
    #endregion

    #region - Hungry -
    [Header("Hungry")]
    [Range(0, 100)] public float currentHungry;
    [Range(0, 100)] public float maxHungry = 100f;
    [Range(0, 1)] public float hungryLifeLossTax = 0.07f;
    [Range(0, 1)] public float hungryLossTax = 0.09f;
    #endregion

    #region - Cold Level -
    [Header("Cold Level")]
    [Range(0, 100)] public float currentColdLevel;
    [Range(0, 100)] public float maxColdLevel = 100;
    [Range(0, 2)] public float coldLevelLossTax = 0.7f;
    [Range(0, 2)] public float coldLevelGainTax = 0f;
    [Range(0, 1)] public float coldLevelLifeLossTax = 0.1f;
    public bool OnSnow;
    public bool OnFirePlace;
    #endregion

    #region - Sliders Region -
    [Header("Sliders")]
    public Slider coldLevelSlider;
    public Slider hungrySlider;
    public Slider thirstSlider;
    public Slider staminaSlider;
    public Slider lifeSlider;
    #endregion

    #region - Texts Region -
    [Header("Texts")]
    public TextMeshProUGUI coldLevelText;
    public TextMeshProUGUI thristText;
    public TextMeshProUGUI hungryText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI lifeText;
    #endregion

    private void Start() => playerAsset = GetComponent<PlayerController>();
    void Update()
    {
        Stamina();
        ColdLevel();
        Thirst();
        Hungry();
        SlidersTextUpdate();
    }
    void ColdLevel()
    {
        if (currentColdLevel <= 0) currentColdLevel = 0;
        if (currentColdLevel >= maxColdLevel) currentHealth -= coldLevelLifeLossTax * Time.deltaTime;
        if (currentColdLevel < maxColdLevel && !OnSnow) currentColdLevel += coldLevelLossTax * Time.deltaTime;
        else if (currentColdLevel < maxColdLevel && OnSnow) currentColdLevel += coldLevelLossTax * 10 * Time.deltaTime;
        if (OnFirePlace) currentColdLevel -= coldLevelGainTax * Time.deltaTime;
    }
    public void Thirst()
    {
        if (currentThrist <= 0) currentThrist = 0;
        if (currentThrist >= maxThrist) currentHealth -= thristLifeLossTax * Time.deltaTime;
        if (currentThrist < maxThrist && !playerAsset.isRunning) currentThrist += thirstLossTax * Time.deltaTime;
        else if (currentThrist < maxThrist && playerAsset.isRunning) currentThrist += thirstLossTax * 4 * Time.deltaTime;
    }
    public void Hungry()
    {
        if (currentHungry <= 0) currentHungry = 0;
        if (currentHungry < maxHungry && !playerAsset.isRunning) currentHungry += hungryLossTax * Time.deltaTime;
        else if (currentHungry < maxHungry && playerAsset.isRunning) currentHungry += hungryLossTax * 2 * Time.deltaTime;
    }
    public void Stamina()
    {
        if (currentStamina >= maxStamina) currentStamina = maxStamina;
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            playerAsset.canRun = false;
        }
        else if (currentStamina > 0) playerAsset.canRun = true;
        if (playerAsset.isRunning) currentStamina -= staminaLossTax * Time.deltaTime;
        if (!playerAsset.isRunning) currentStamina += staminaGainTax * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
       /* if (gameObject.CompareTag("ColdArea"))
        {
            OnSnow = true;
        }
        else
        {
            OnSnow = false;
        }
        if (gameObject.CompareTag("FirePlace"))
        {
            OnFirePlace = true;
        }
        else
        {
            OnFirePlace = false;
        }*/
    }
    public void SlidersTextUpdate()
    {
        #region - Health -
        lifeText.text = (currentHealth.ToString("F1") + "/100");
        lifeSlider.value = currentHealth;
        #endregion

        #region - Hungry
        hungryText.text = (currentHungry.ToString("F0") + "/" + maxHungry);
        hungrySlider.value = currentHungry;
        hungrySlider.maxValue = maxHungry;
        #endregion

        #region - ColdLevel -
        coldLevelText.text = (currentColdLevel.ToString("F0") + "/" + maxColdLevel);
        coldLevelSlider.value = currentColdLevel;
        thirstSlider.maxValue = maxColdLevel;
        #endregion

        #region - Thirst
        thristText.text = (currentThrist.ToString("F0") + "/" + maxThrist);
        thirstSlider.value = currentThrist;
        thirstSlider.maxValue = maxThrist;
        #endregion

        #region - Stamina -
        staminaText.text = (currentStamina.ToString("F0") + "/" + maxStamina);
        staminaSlider.value = currentStamina;
        staminaSlider.maxValue = maxStamina;
        #endregion
    }
    public void GiveDamage(float Damage)
    {
        if (currentHealth - Damage <= 0)
        {
            currentHealth = 0;
            //Die Behavior
        }
    }
    public void CurePlayer(float cureValue) => currentHealth = (currentHealth + cureValue) > maxHealth ? maxHealth : currentHealth + cureValue; 
}