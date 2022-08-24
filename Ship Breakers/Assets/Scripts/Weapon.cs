using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform inactiveBullets;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    [Header("Ammo")]
    [SerializeField] int minAmmo = 20;
    [SerializeField] int maxAmmo = 40;
    [SerializeField] UpgradeBar ammoCapacity;
    [SerializeField] Text ammoText;
    int ammo;
    int ammoDiff;

    [Header("Bullet Speed")]
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    float speedDiff;
    [SerializeField] UpgradeBar bulletPower;

    [SerializeField] Button addAmmo;
    [SerializeField] Button subtractAmmo;

    [SerializeField] float maxAngle = 30;

    [SerializeField] int minChance = 3;
    [SerializeField] int maxChance = 15;
    int chanceDiff;
    [SerializeField] UpgradeBar accuracy;
    Bullet[] bullets;

    public int Ammo { get => ammo; set => ammo = value; }

    // Start is called before the first frame update
    void Start()
    {

        speedDiff = maxSpeed - minSpeed;
        chanceDiff = maxChance - minChance;
        ammo = minAmmo;
        ammoDiff = maxAmmo - minAmmo;

        
        UpdateAmmoText();
        UpdateButtons();
        RefreshBullets();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
    public void RefreshBullets()
    {
        bullets = inactiveBullets.GetComponentsInChildren<Bullet>();
    }
    public int CurrentCapacity()
    {
        return minAmmo + (int)(ammoCapacity.Percentage() * ammoDiff);
    }
    public float CurrentBulletSpeed(){
        return minSpeed + (bulletPower.Percentage() * speedDiff);
    }
    public void UpdateAmmoText(){
        ammoText.text = "Ammo: " + ammo.ToString() + "/" + CurrentCapacity().ToString();
    }
    public void AddAmmo(int amount)
    {
        if (ammo + amount >= CurrentCapacity())
        {
            ammo = CurrentCapacity();

        }
        else
        {
            ammo += amount;
        }
        UpdateAmmoText();
    }

    public void AddAmmoUpgradePoint(){
        if(GameManager.instance.UpgradePoints <= 0){
            return;
        }
        else{
            GameManager.instance.AddUpgradePoint(-1);
            AddAmmo(1);
            UpdateAmmoText();
        }
    }

    public void SubtractAmmoUpgradePoint(){
        if(ammo <= 0){
            return;
        }
        else{
            GameManager.instance.AddUpgradePoint(1);
            AddAmmo(-1);
            UpdateAmmoText();
        }
    }

    public void UpdateButtons(){
        if(GameManager.instance.UpgradePoints > 0 || ammo < CurrentCapacity()){
            addAmmo.interactable = true;
        }
        else{
            addAmmo.interactable = false;
        }

        if(ammo > 0){
            subtractAmmo.interactable = true;
        }
        else{
            subtractAmmo.interactable = false;
        }
    }
    public void Fire()
    {
        RefreshBullets();
        Vector3 direction = Vector3.right;
        if(ammo <= 0){
            return;
        }
        else{
            ammo--;
            UpdateAmmoText();
        }
        if (accuracy.Percentage() < 1)
        {
            int chance = minChance + (int)(chanceDiff * accuracy.Percentage());
            int rand = Random.Range(0, minChance + (int)(chanceDiff * accuracy.Percentage()));
            if (rand == 0)
            {
                float angle = Random.Range(-maxAngle, maxAngle);
                Debug.Log(angle);
                direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            }
        }
        if (bullets.Length == 0)
        {
            GameObject b = Instantiate(bullet, inactiveBullets.position, Quaternion.identity);
            b.transform.SetParent(inactiveBullets);
            b.GetComponent<Bullet>().Activate(firePoint.position, direction, CurrentBulletSpeed());
            RefreshBullets();
        }
        else
        {
            bullets[0].Activate(firePoint.position, direction, CurrentBulletSpeed());
            RefreshBullets();
        }
    }
}
