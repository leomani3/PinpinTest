using MyBox;
using System;
using TMPro;
using UnityEngine;

public class BuyZonePet : BuyZone
{
    [Separator("Pet buy zone")]
    [SerializeField] private PetData petData;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Transform petSpawn;

    protected override void UpdateText()
    {
        base.UpdateText();
        int minutes = TimeSpan.FromSeconds(petData.timeAlive).Minutes;
        int seconds = TimeSpan.FromSeconds(petData.timeAlive).Seconds;

        if (seconds > 10)
        {
            timerText.text = minutes + ":" + seconds;
        }
        else
        {
            timerText.text = minutes + ":0" + seconds;
        }

    }

    public override void Reset()
    {
        base.Reset();
        buyable.transform.position = petSpawn.position;
        buyable.transform.rotation = petSpawn.rotation;
        UpdateText();
    }
}