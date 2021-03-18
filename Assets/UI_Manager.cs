using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public UpgradeType[] upgrade = new UpgradeType[4];

    //struct for each upgrade. saves space
    public struct UpgradeType
    {
        public int baseCost;
        public int multiplier;
    }

    // each upgrade type
    public UpgradeType single;
    public UpgradeType plusTwo;
    public UpgradeType plusFive;
    public UpgradeType perSecond;

    //your money in game
    public int cookieMoney;

    // UI text arrays
    public Text[] Cost;
    public Text[] timesBought;

    //non-array texts.
    public Text totalCookies;
    public Text upgradeDenier;
    public int index;

    public GameObject[] gameButton;

    public float repeatTime = 1;
    public bool perSecondTick = true;

    void Start()
    {
        //Upgrageinitialization region contains all the initialisation for each upgrade
        #region Upgradeinitialization
        single = new UpgradeType();
        single.baseCost = 15;
        single.multiplier = 0;
        upgrade[0] = single;

        plusTwo = new UpgradeType();
        plusTwo.baseCost = 100;
        plusTwo.multiplier = 0;
        upgrade[1] = plusTwo;

        plusFive = new UpgradeType();
        plusFive.baseCost = 500;
        plusFive.multiplier = 0;
        upgrade[2] = plusFive;

        perSecond = new UpgradeType();
        perSecond.baseCost = 2000;
        perSecond.multiplier = 0;
        upgrade[3] = perSecond;

        #endregion

        //each upgrade button starts off inactive. yes this can be rewritten to be inclusive of each member of the array.
        //this is what youre getting.
        gameButton[0].SetActive(false);
        gameButton[1].SetActive(false);
        gameButton[2].SetActive(false);
        gameButton[3].SetActive(false);

        //special text
        upgradeDenier.text = "Beginning Cookie Experience...";
        totalCookies.text = "Total Cookies:" + cookieMoney.ToString();

        // might as well have index as 0.
        index = 0;
    }

    // the single click increaser
    public void Single()
    {
        index = 0;
        if(cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            TextColourChanger(index);
            upgradeDenier.text = "Cookie Efficiency!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }

    }

    // 2 additional clicks per buy
    public void PlusTwo()
    {
        index = 1;
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            TextColourChanger(index);
            upgradeDenier.text = "Cookie Overload!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    // 5 additional clicks per buy
    public void PlusFive()
    {
        index = 2;
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            TextColourChanger(index);
            upgradeDenier.text = "Cookie INSANITY!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    // 1 click per second per buy
    public void PerSecond()
    {
        index = 3;
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            TextColourChanger(index);
            repeatTime++;
            upgradeDenier.text = "Sneaky Cookies...";
            perSecondTick = true;
            StartCoroutine(MoneyTick(1));
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    //neccessary for the MoneyTick coroutine used in the Persecond buy
    public void AddSingle()
    {
        cookieMoney++;
        totalCookies.text = "Total Cookies:" + cookieMoney.ToString();
    }

    //enables the upgrade when you have enough money to buy it
    public void UpgradeEnabler(int indexNumber, int changer)
    {
        if(cookieMoney > indexNumber)
        {
            gameButton[changer].SetActive(true);
        }
    }

    //changes the colour of the text based on if you can buy the upgrade or not
    public void TextColourChanger(int index)
    {
        if (cookieMoney < PriceDeductor(upgrade[index].baseCost, upgrade[index].multiplier + 1))
        {
            Cost[index].color = Color.red;
        }
        if (cookieMoney >= PriceDeductor(upgrade[index].baseCost, upgrade[index].multiplier + 1))
        {
            Cost[index].color = Color.white;
        }
    }

    //calculates how much money to take off the money count
    public int PriceDeductor(int baseCost, int buyNumber)
    {
        float deduction;
        deduction = baseCost + Mathf.Pow(buyNumber - 1 , 2);
        return (int)deduction;
    }

    //changes the upgrade price to the new one on click
    public void PriceChanger()
    {
        upgrade[index].multiplier++;
        cookieMoney = cookieMoney - PriceDeductor(upgrade[index].baseCost, upgrade[index].multiplier);
        timesBought[index].text = upgrade[index].multiplier.ToString();
        Cost[index].text = "Cost: " + PriceDeductor(upgrade[index].baseCost, upgrade[index].multiplier + 1).ToString();
        totalCookies.text = "Total Cookies:" + cookieMoney.ToString();
    }

    //returns how many cookies the counter has
    public void TotalCookies()
    {
        cookieMoney = cookieMoney + 1 + upgrade[0].multiplier + (2 * upgrade[1].multiplier) + (5 * upgrade[2].multiplier);
        totalCookies.text = "Total Cookies:" + cookieMoney.ToString();
        TextColourChanger(0);
        TextColourChanger(1);
        TextColourChanger(2);
        TextColourChanger(3);


        UpgradeEnabler(upgrade[0].baseCost, 0);
        UpgradeEnabler(upgrade[1].baseCost, 1);
        UpgradeEnabler(upgrade[2].baseCost, 2);
        UpgradeEnabler(upgrade[3].baseCost, 3);
        upgradeDenier.text = "MOAR COOKIE!!!";
    }

    //a coroutine to add 1 every second to the money count
    IEnumerator MoneyTick (float repeatTime)
    {
        while (perSecondTick)
        {
            yield return new WaitForSeconds(repeatTime);
            AddSingle();
            TextColourChanger(0);
            TextColourChanger(1);
            TextColourChanger(2);
            TextColourChanger(3);
        }
    }

    //quit button.
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
