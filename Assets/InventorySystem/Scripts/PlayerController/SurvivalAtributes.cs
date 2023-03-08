using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalAtributes : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //ParticlesDatabase - Code Update Version 0.2 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statements a simple Singleton Pattern implementation
    public static SurvivalAtributes Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - Class References -
    private PlayerController playerAsset => GetComponent<PlayerController>();
    #endregion

    #region - Health Values -
    [Range(0, 150)] public float currentHealth = 100;
    public float maxHealth = 150;
    #endregion

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

    #region - Survival Atributes Behavior -
    void Update()
    {
        Stamina();
        ColdLevel();
        Thirst();
        Hungry();
        SlidersTextUpdate();
    }

    #region - Cold Behavior -
    void ColdLevel()//This method calculates all cold level values using the passed time and considering the current walk and run state
    {
        if (currentColdLevel <= 0) currentColdLevel = 0;
        if (currentColdLevel >= maxColdLevel) currentHealth -= coldLevelLifeLossTax * Time.deltaTime;
        if (currentColdLevel < maxColdLevel && !OnSnow) currentColdLevel += coldLevelLossTax * Time.deltaTime;
        else if (currentColdLevel < maxColdLevel && OnSnow) currentColdLevel += coldLevelLossTax * 10 * Time.deltaTime;
        if (OnFirePlace) currentColdLevel -= coldLevelGainTax * Time.deltaTime;
    }
    #endregion

    #region - Thirst Behavior -
    public void Thirst()//This method calculates all thirst values using the passed time and considering the current walk and run state
    {
        if (currentThrist <= 0) currentThrist = 0;
        if (currentThrist >= maxThrist) currentHealth -= thristLifeLossTax * Time.deltaTime;
        if (currentThrist < maxThrist && !playerAsset.isRunning) currentThrist += thirstLossTax * Time.deltaTime;
        else if (currentThrist < maxThrist && playerAsset.isRunning) currentThrist += thirstLossTax * 4 * Time.deltaTime;
    }
    #endregion

    #region - Hungry Behavior -
    public void Hungry()//This method calculates all hungry values using the passed time and considering the current walk and run state
    {
        if (currentHungry <= 0) currentHungry = 0;
        if (currentHungry < maxHungry && !playerAsset.isRunning) currentHungry += hungryLossTax * Time.deltaTime;
        else if (currentHungry < maxHungry && playerAsset.isRunning) currentHungry += hungryLossTax * 2 * Time.deltaTime;
    }
    #endregion

    #region - Stamina Behavior -
    public void Stamina()//This method calculates all stamina values considering the current player state
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
    #endregion

    #endregion

    #region - Cold Area Behavior -
    private void OnTriggerEnter(Collider other)//This method detects if the player is on a snow area and if he is on Fireplace area
    {
        OnSnow = other.transform.CompareTag("ColdArea");
        OnFirePlace = other.transform.CompareTag("FirePlace");
    }
    #endregion

    #region - Text and Sliders UI Update -
    public void SlidersTextUpdate()//This method update all sliders and texts from the Survival Atributes System
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
    #endregion

    #region - Damage and Cure - 
    public void GiveDamage(float Damage)//This method give a certain damage to the player considering his minimum life
    {
        if (currentHealth - Damage <= 0)
        {
            currentHealth = 0;
            //Die Behavior
        }
    }
    public void CurePlayer(float cureValue) => currentHealth = (currentHealth + cureValue) > maxHealth ? maxHealth : currentHealth + cureValue;//This method cure the player considering his maximum life
    #endregion
}