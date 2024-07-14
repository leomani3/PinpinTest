using UnityEngine;

namespace Pinpin
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CurrencyData woodCurrency;
        [SerializeField] private CurrencyData stoneCurrency;

        private void Start()
        {
            woodCurrency.Init();
            stoneCurrency.Init();
        }

        public static void CheckWood(int amount)
        {
            Debug.Log("You got " + amount + " wood so far.");
        }
    }
}
