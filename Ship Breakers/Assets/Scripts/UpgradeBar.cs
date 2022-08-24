using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] Text titleText;
    [SerializeField] Button add;
    [SerializeField] Button subtract;
    [SerializeField] string title;

    int points = 1;
    int maxPoints = 11;
    int diffrence = 10;

    public int Points { get => points; set => points = value; }
    public int MaxPoints { get => maxPoints; set => maxPoints = value; }
    public int Diffrence { get => diffrence; set => diffrence = value; }

    // Start is called before the first frame update
    void Start()
    {
        diffrence = MaxPoints - Points;
        UpdateBar();
        UpdateButtons();
        titleText.text = title;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBar()
    {
        bar.fillAmount = (float) Points / (float) MaxPoints;
    }

    public void Add()
    {
        if (GameManager.instance.UpgradePoints == 0)
        {
            return;
        }
        else
        {
            Points++;
            GameManager.instance.ApplyUpgrades();
            GameManager.instance.AddUpgradePoint(-1);
            UpdateBar();
            UpdateButtons();
        }
    }

    public void Subtract(){
        if(Points == 1){
            return;
        }
        else{
            Points--;
            GameManager.instance.AddUpgradePoint(1);
            GameManager.instance.ApplyUpgrades();
            UpdateBar();
            UpdateButtons();
        }
    }
    public float Percentage(){
        return ((float)points - 1) / (float)diffrence;
    }
    public void UpdateButtons()
    {
        if (GameManager.instance.UpgradePoints > 0)
        {
            add.interactable = true;
        }
        else
        {
            add.interactable = false;
        }
        if (Points > 1)
        {
            subtract.interactable = true;
        }
        else
        {
            subtract.interactable = false;
        }
    }
}
