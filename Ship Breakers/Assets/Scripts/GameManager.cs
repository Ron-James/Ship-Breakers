using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int ammo = 10;
    [SerializeField] int maxAmmo = 10;
    [SerializeField] int upgradePoints;
    [Header("Ship")]
    [SerializeField] GameObject ship;
    [Header("Text Fields")]
    [SerializeField] Text upPointsText;
    [Header("Upgrade Bars")]
    [SerializeField] GameObject upgradeObjects;
    UpgradeBar[] upgradeBars;

    [Header("Health Stuff")]
    [SerializeField] int maxHealth = 2;
    [SerializeField] GameObject[] hearts;
    [SerializeField] Text healthText;
    [SerializeField] int minHearts;
    int maxHearts;
    public float currentHealth = 2;
    [SerializeField] UpgradeBar healthUpgrade;

    [Header("Win/Lose Screens")]
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject repairInfo;

    [Header("Time Limits")]
    [SerializeField] float maxTime;
    float time;


    public int Ammo { get => ammo; set => ammo = value; }
    public int UpgradePoints { get => upgradePoints; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public GameObject Ship { get => ship; set => ship = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Start is called before the first frame update
    void Start()
    {
        
        maxHearts = hearts.Length;
        UpdateHearts();
        RefreshUpgradeText();
        upgradeBars = upgradeObjects.GetComponentsInChildren<UpgradeBar>();
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
        UpdateUpgradeButtons();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= maxTime){
            Win();
        }
    }

    public void AddHealth(int amount){
        if(currentHealth + amount >= maxHealth){
            currentHealth = maxHealth;
            UpdateHearts();
            return;
        }
        currentHealth += amount;
        if(currentHealth <= 0){
            currentHealth = 0;
            UpdateHearts();
            //load game over
            GameOver();
        }
        else{
            UpdateHearts();
        }
    }
    public void UpdateHealthText(){
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }
    public void AddMaxHearts(int amount){
        if(amount == 0 || maxHealth + amount <= 0){
            return;
        }
        else if(maxHealth + amount >= maxHearts){
            maxHealth = maxHearts;
            UpdateHealthText();
        }
        else{
            maxHealth += amount;
            UpdateHealthText();
        }
    }

    public void UpdateMaxHealth(){
        int diff = maxHearts - minHearts;
        maxHealth = minHearts + (int)(healthUpgrade.Percentage() * diff);
        UpdateHealthText();
    }
    public void UpdateHearts(){
        UpdateHealthText();
        for (int loop = 0; loop < hearts.Length; loop++){
            if(loop <= currentHealth - 1){
                hearts[loop].SetActive(true);
            }
            else{
                hearts[loop].SetActive(false);
            }
        }
    }
    public void ApplyUpgrades(){
        ship.GetComponent<ShipController>().UpdateDrag();
        ship.GetComponent<ShipController>().UpdateVelocity();
        ship.GetComponent<Weapon>().UpdateAmmoText();
        UpdateMaxHealth();
    }
    public void AddUpgradePoint(int amount){
        upgradePoints += amount;
        RefreshUpgradeText();
        UpdateUpgradeButtons();
    }
    public void UpdateUpgradeButtons(){
        for (int loop = 0; loop < upgradeBars.Length; loop++){
            upgradeBars[loop].UpdateButtons();
        }
        ship.GetComponent<Weapon>().UpdateButtons();
    }
    public void RefreshUpgradeText(){
        upPointsText.text = "Upgrade Points: " + UpgradePoints.ToString();
    }

    public void Win(){
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }
    public void GameOver(){
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }

    public void OpenRepairInfo(){
        Time.timeScale = 0;
        repairInfo.SetActive(true);

    }
    public void CloseRepairInfo(){
        Time.timeScale = 1;
        repairInfo.SetActive(false);
    }
}
