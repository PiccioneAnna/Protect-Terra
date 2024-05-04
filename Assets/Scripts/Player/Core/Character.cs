using System;
using UnityEngine;
using TMPro;
using UI;
using System.Collections;

// Class handles movement, actions, and main references to other systems
namespace Player
{
    public class Character : MonoBehaviour
    {
        #region Fields

        public int level = 1;

        [Header("Stats")]
        public Stat hp;
        public Stat stamina;
        public Stat hunger;

        [Header("States")]
        public bool isDead;
        public bool isExhausted;
        public bool isStarving;

        [Header("Status Bars")]
        [SerializeField] StatusBar hpBar;
        [SerializeField] StatusBar staminaBar;
        [SerializeField] StatusBar hungerBar;

        public TextMeshProUGUI hpValue;
        public TextMeshProUGUI staminaValue;
        public TextMeshProUGUI hungerValue;

        [Header("Color Theory")]
        [SerializeField] Color damageColor = Color.red;
        private SpriteRenderer spriteRenderer;

        #endregion

        #region Runtime
        private void Start()
        {
            //hp.currVal = hp.maxVal;
            //stamina.currVal = stamina.maxVal;
            //hunger.currVal = hunger.maxVal;

            //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            //UpdateHPBar();
            //UpdateStaminaBar();
            //UpdateHungerBar();
        }
        #endregion

        #region Update Status Bars
        private void UpdateHPBar()
        {
            hpBar.Set(hp.currVal, hp.maxVal);

            hpValue.text = FormatStatValue(hp);
            hpValue.transform.parent.gameObject.SetActive(DisplayValue(hp));
        }
        private void UpdateStaminaBar()
        {
            staminaBar.Set(stamina.currVal, stamina.maxVal);

            staminaValue.text = FormatStatValue(stamina);
            staminaValue.transform.parent.gameObject.SetActive(DisplayValue(stamina));
        }
        private void UpdateHungerBar()
        {
            hungerBar.Set(hunger.currVal, hunger.maxVal);

            hungerValue.text = FormatStatValue(hunger);
            hungerValue.transform.parent.gameObject.SetActive(DisplayValue(hunger));
        }

        #endregion

        #region HP

        public void TakeDamage(int amount)
        {
            //Visual appearence of taking damage
            spriteRenderer.color = damageColor;
            StartCoroutine(Whitecolor());

            hp.Subtract(amount);
            if (hp.currVal <= 0)
            {
                isDead = true;
            }

            UpdateHPBar();
        }

        public void Heal(float amount)
        {
            hp.Add(amount);
            UpdateHPBar();
        }

        public void FullHeal()
        {
            hp.SetToMax();
            UpdateHPBar();
        }

        #endregion

        #region Stamina

        public void GetTired(int amount)
        {
            stamina.Subtract(amount);
            if (stamina.currVal <= 0)
            {
                isExhausted = true;
            }
            UpdateStaminaBar();
        }

        public void Rest(float amount)
        {
            stamina.Add(amount);
            if (stamina.currVal >= 0)
            {
                isExhausted = false;
            }
            UpdateStaminaBar();
        }

        public void FullRest()
        {
            isExhausted = false;
            stamina.SetToMax();
            UpdateStaminaBar();
        }

        #endregion

        #region Hunger

        public void GetHungry(int amount)
        {
            hunger.Subtract(amount);
            if (hunger.currVal <= 0)
            {
                isStarving = true;
            }
            UpdateHungerBar();
        }

        public void Eat(float amount)
        {
            hunger.Add(amount);
            if (hunger.currVal >= 0)
            {
                isStarving = false;
            }
            UpdateHungerBar();
        }

        public void FullMeal()
        {
            isStarving = false;
            hunger.SetToMax();
            UpdateHungerBar();
        }

        #endregion

        #region Helper Methods
        IEnumerator Whitecolor()
        {
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
        }

        private String FormatStatValue(Stat stat)
        {
            return $"{(int)stat.currVal} / {stat.maxVal}";
        }

        private bool DisplayValue(Stat stat)
        {
            return stat.currVal < stat.maxVal;
        }
        #endregion

    }
}