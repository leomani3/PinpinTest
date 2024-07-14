using Pinpin;
using UnityEngine;

[CreateAssetMenu(fileName = "WoodCurrencyData", menuName = "ScriptableObjects/WoodCurrencyData")]
public class WoodCurrencyData : CurrencyData
{
    int _nbWoodHarvested;
    public override void IncreaseCurrency(int amount)
    {
        base.IncreaseCurrency(amount);

        //J'ai fait deux versions du code pour répondre à l'objectif obligatoire #3. La raison est qu'on peut imaginer que le joueur pourra récolter du bois autrement que 1 par 1
        // (upgrade qui améliore les quantités ramassées ou bonus qui donne 40 bois d'un coup par exemple).

        //La première passe à CheckWood() la quantité réel de bois.
        //si le joueur a déjà 6 bois en stock et qu'il en ramasse 10 d'un coup. Alors cette fonction renverra :
        // -"You got 16 wood so far."
        // -"You got 16 wood so far."
        //Le threshold a été franchi 2 fois lorsque les quantité 7 et 14 ont été atteintes. Mais la quantité totale est bien de 16.

        int nbWoodLeft = _nbWoodHarvested + amount;
        while (nbWoodLeft > 0)
        {
            if (nbWoodLeft >= 7)
            {
                GameManager.CheckWood(currencyAmount);
                nbWoodLeft -= 7;
                _nbWoodHarvested = 0;
            }
            else
            {
                _nbWoodHarvested = nbWoodLeft;
                nbWoodLeft = 0;
            }
        }


        //La deuxième passe à CheckWood() la quantité de bois au moment précis où le threshold a été franchi.
        //si le joueur a déjà 6 bois en stock et qu'il en ramasse 10 d'un coup. Alors cette fonction renverra :
        // -"You got 7 wood so far."
        // -"You got 14 wood so far."
        //Le threshold a été franchi 2 fois lorsque les quantité 7 et 14 ont été atteintes. 2 quantités de bois restent alors en "fantôme"

        //int woodBefore = currencyAmount - amount;
        //for (int i = 1; i <= amount; i++)
        //{
        //    _nbWoodHarvested++;
        //    if (_nbWoodHarvested == 7)
        //    {
        //        GameManager.CheckWood(woodBefore + i);
        //        _nbWoodHarvested = 0;
        //    }
        //}

        //La question de la version à choisir est alors à se poser en fonction des données utilisateurs que l'on veut pour de l'analyse.
    }

    public override void Init()
    {
        base.Init();
        _nbWoodHarvested = 0;
    }
}
