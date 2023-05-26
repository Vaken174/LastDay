using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicators : MonoBehaviour
{

    [SerializeField]
    private Image healthBar, foodBar, waterBar;
    
    public float healthAmount = 100;

    public float foodAmount = 100;

    public float waterAmount =100;

    private float UIFoodAmount=100;
    private float UIWaterAmount =100;
    private float UIHealthAmount = 100;


    public float secondsToEptyFood = 60f;
    public float secondsToEptyWater = 30f;
    public float secondsToEptyHealth = 60f;

    private float changeFactor = 100f;

    public bool isInWater = false;

    private void Start()
    {
        healthBar.fillAmount = healthAmount/100;
        foodBar.fillAmount = foodAmount/100;
        waterBar.fillAmount = waterAmount/100;
    }
    private void Update()
    {
        if (isInWater) 
        {
            if (Input.GetKey(KeyCode.E))
                ChangeWaterAmount(50);
        }
        if (foodAmount > 0)
        {
        foodAmount -= 100 / secondsToEptyFood * Time.deltaTime;
        UIFoodAmount = Mathf.Lerp(UIFoodAmount, foodAmount, Time.deltaTime * changeFactor);
        foodBar.fillAmount =UIFoodAmount/100;
        }
        else
        {
            UIFoodAmount = 0;
            foodBar.fillAmount = UIFoodAmount / 100;
        }
        if (waterAmount > 0)
        {
        waterAmount -= 100 / secondsToEptyWater * Time.deltaTime;
        UIWaterAmount = Mathf.Lerp(UIWaterAmount, waterAmount, Time.deltaTime * changeFactor);
        waterBar.fillAmount=UIWaterAmount/100;
        }
        else
        {
            UIWaterAmount = 0;
            waterBar.fillAmount = UIWaterAmount / 100;
        }
        if(foodAmount <=0)
        {
            healthAmount -=100/ secondsToEptyHealth * Time.deltaTime;
        }
        if (waterAmount <= 0) 
        {
            healthAmount -= 100 / secondsToEptyHealth * Time.deltaTime;
        }
        UIHealthAmount = Mathf.Lerp(UIHealthAmount, healthAmount, Time.deltaTime * changeFactor);
        healthBar.fillAmount = UIHealthAmount/100;
    }

    public void ChangeFoodAmount(float changeValues) 
    {
        if(foodAmount + changeValues > 100)
        {
            foodAmount = 100;
        }
        else
        {
        foodAmount += changeValues;
        }
    }
    public void ChangeWaterAmount(float changeValues)
    {
        if (waterAmount + changeValues > 100)
        {
            waterAmount = 100;
        }
        else
        {
            waterAmount += changeValues;
        }
    }
    public void ChangeHealthAmount(float changeValues)
    {
        if(healthAmount + changeValues >100)
        {
            healthAmount = 100;
        }    
        else
        {
            healthAmount += changeValues; 
        }
        
    }
}
