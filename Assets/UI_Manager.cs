using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region ClassSetup
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

    // index changes based on the button press and alters the struct values based on which index is used
    public int index;
    
    public GameObject[] gameButton;

    // used for the coroutine for adding clicks per second
    public float repeatTime = 1;
    bool perSecondTick = true;
    #endregion 

    void Start()
    {
        //Upgrageinitialization region contains all the initialisation for each upgrade
        #region UpgradeDetails
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

        //each upgrade button starts off inactive.
        foreach (GameObject inGameButton in gameButton)
        {
            inGameButton.SetActive(false);
        }

        //special text
        upgradeDenier.text = "Beginning Cookie Experience...";
        totalCookies.text = "Total Cookies:" + cookieMoney.ToString();

        // might as well have index as 0.
        index = 0;
    }

    /// <summary>
    /// the single click increaser all non timebased upgrades work on this set up
    /// </summary>
    public void Single()
    {
        index = 0;
        //checks if you have enough money to buy the upgrade
        if(cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            //loops through each button to change colour
            for (int index = 0; index < upgrade.Length; index++)
            {
                TextColourChanger(index);
            }
            upgradeDenier.text = "Cookie Efficiency!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }

    }

    /// <summary>
    /// adds two clicks per buy
    /// </summary>
    public void PlusTwo()
    {
        index = 1;
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            for (int index = 0; index < upgrade.Length; index++)
            {
                TextColourChanger(index);
            }
            upgradeDenier.text = "Cookie Overload!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    /// <summary>
    /// adds 5 clicks per buy
    /// </summary>
    public void PlusFive()
    {
        index = 2;
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            PriceChanger();
            for (int index = 0; index < upgrade.Length; index++)
            {
                TextColourChanger(index);
            }
            upgradeDenier.text = "Cookie INSANITY!";
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    /// <summary>
    /// adds one click per second per buy
    /// </summary>
    public void PerSecond()
    {
        index = 3;
        //checks if you have enough money to buy the upgrade
        if (cookieMoney >= upgrade[index].baseCost + Mathf.Pow(upgrade[index].multiplier, 2))
        {
            //using upgrade[3].multiplier breaks the repeat time float so i made a new number that works
            float number = 0;
            number++;
            PriceChanger();

            //neccessary to change the value that moneytick is using to repeat the function
            StopCoroutine("MoneyTick");

            //checks if the money count has exceeded the buy cost
            for (int index = 0; index < upgrade.Length; index++)
            {
                TextColourChanger(index);
            }

            //repeats per second 1, 0.5, 0.33, ect.
            repeatTime = (1 / number);
            upgradeDenier.text = "Sneaky Cookies...";

            perSecondTick = true;
            StartCoroutine(MoneyTick(repeatTime));
        }
        else
        {
            upgradeDenier.text = "You don't have enough cookies to buy me!";
        }
    }

    //a coroutine to add 1 multiplier time to the money count
    IEnumerator MoneyTick(float repeatTime)
    {
        while (perSecondTick)
        {
            // waits for the repeat time 
            yield return new WaitForSeconds(repeatTime);
            AddSingle();
            for (int index = 0; index < upgrade.Length; index++)
            {
                TextColourChanger(index);
            }
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

        for (int index = 0; index < upgrade.Length; index++)
        {
            TextColourChanger(index);
        }

        for (int index = 0; index < upgrade.Length; index++)
        {
            UpgradeEnabler(upgrade[index].baseCost, index);
        }

        upgradeDenier.text = "MOAR COOKIE!!!";
    }

    //quit button. this could have been a method called by a UI button but it isnt.
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}