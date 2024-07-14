using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pinpin
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CurrencyData> currencyDatas;

        private void Start()
        {
            foreach (CurrencyData currencyData in currencyDatas)
            {
                if (currencyData != null)
                {
                    currencyData.Init();
                }
            }
        }
        public static void CheckWood(int amount)
        {
            Debug.Log("You got " + amount + " wood so far.");
        }
    }
}
