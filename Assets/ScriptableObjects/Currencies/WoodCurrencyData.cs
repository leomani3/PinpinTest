using Pinpin;
using UnityEngine;

[CreateAssetMenu(fileName = "WoodCurrencyData", menuName = "ScriptableObjects/WoodCurrencyData")]
public class WoodCurrencyData : CurrencyData
{
    int _nbWoodHarvested;
    public override void IncreaseCurrency(int amount)
    {
        base.IncreaseCurrency(amount);

        //J'ai fait deux versions du code pour r�pondre � l'objectif obligatoire #3. La raison est qu'on peut imaginer que le joueur pourra r�colter du bois autrement que 1 par 1
        // (upgrade qui am�liore les quantit�s ramass�es ou bonus qui donne 40 bois d'un coup par exemple).

        //La premi�re passe � CheckWood() la quantit� r�el de bois.
        //si le joueur a d�j� 6 bois en stock et qu'il en ramasse 10 d'un coup. Alors cette fonction renverra :
        // -"You got 16 wood so far."
        // -"You got 16 wood so far."
        //Le threshold a �t� franchi 2 fois lorsque les quantit� 7 et 14 ont �t� atteintes. Mais la quantit� totale est bien de 16.

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


        //La deuxi�me passe � CheckWood() la quantit� de bois au moment pr�cis o� le threshold a �t� franchi.
        //si le joueur a d�j� 6 bois en stock et qu'il en ramasse 10 d'un coup. Alors cette fonction renverra :
        // -"You got 7 wood so far."
        // -"You got 14 wood so far."
        //Le threshold a �t� franchi 2 fois lorsque les quantit� 7 et 14 ont �t� atteintes. 2 quantit�s de bois restent alors en "fant�me"

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

        //La question de la version � choisir est alors � se poser en fonction des donn�es utilisateurs que l'on veut pour de l'analyse.
    }

    public override void Init()
    {
        base.Init();
        _nbWoodHarvested = 0;
    }
}
