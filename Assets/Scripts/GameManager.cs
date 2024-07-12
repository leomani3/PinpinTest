using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pinpin
{
    public class GameManager : MonoBehaviour
    {
        public static void CheckWood(int amount)
        {
            Debug.Log("You got " + amount + " wood so far.");
        }
    }
}
