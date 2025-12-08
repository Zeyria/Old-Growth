using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public RawImage portraitImage;
    public TMP_InputField unitName;
    public TMP_Text unitClass;
    public TMP_Text attackStat;
    public TMP_Text speedStat;
    public TMP_Text sightStat;
    public TMP_Text hpStat;
    public GameObject icons;
    public Sprite questionIcon;
    public Sprite movingIcon;
    public Sprite interactingIcon;
    public TMP_Text iconText;

    public int activeIcon = -1;

    public UnitControler unitCon;
    private void Update()
    {
        GameObject unitToMove = unitCon.unitToMove;
        if (unitToMove != null)
        {
            icons.transform.GetChild(3).gameObject.SetActive(true);
            if (!unitToMove.GetComponent<UnitStats>().spawnsCorpse) { icons.transform.GetChild(2).gameObject.SetActive(false); }
            else { icons.transform.GetChild(2).gameObject.SetActive(true); }
            portraitImage.texture = unitToMove.GetComponent<UnitStats>().portrait;
            portraitImage.color = Color.white;
            if (unitName.text == " ")
            {
                unitName.text = unitToMove.GetComponent<UnitStats>().unitName;
            }
            unitToMove.GetComponent<UnitStats>().unitName = unitName.text;
            unitClass.text = unitToMove.GetComponent<UnitStats>().unitClass;
            attackStat.text = unitToMove.GetComponent<UnitStats>().attack.ToString();
            speedStat.text = unitToMove.GetComponent<UnitStats>().speed.ToString();
            sightStat.text = unitToMove.GetComponent<UnitStats>().sightRange.ToString();
            hpStat.text = unitToMove.GetComponent<UnitStats>().hpCurrent.ToString() + "/" + unitToMove.GetComponent<UnitStats>().hpMax.ToString();
            unitName.interactable = true;

            icons.SetActive(true);
            icons.transform.GetChild(0).GetComponent<Image>().sprite = movingIcon;
            if (unitToMove.GetComponent<UnitStats>().action1.iconSprite != null) { icons.transform.GetChild(1).GetComponent<Image>().sprite = unitToMove.GetComponent<UnitStats>().action1.iconSprite; }
            if (unitToMove.GetComponent<UnitStats>().action2.iconSprite != null) { icons.transform.GetChild(2).GetComponent<Image>().sprite = unitToMove.GetComponent<UnitStats>().action2.iconSprite; }
            icons.transform.GetChild(3).GetComponent<Image>().sprite = interactingIcon;

            if (unitToMove.GetComponent<UnitStats>().isEnemy)
            {
                for (int x = 0; x < icons.transform.childCount - 1; x++)
                {
                    icons.transform.GetChild(x).GetComponent<Image>().sprite = questionIcon;
                }
                icons.transform.GetChild(3).gameObject.SetActive(false);
                attackStat.text = "?";
                speedStat.text = "?";
                sightStat.text = "?";
                hpStat.text = "?/?";
                unitName.interactable = false;
            }
        }
        else
        {
            portraitImage.color = new Color32(0, 0, 0, 0);

            unitName.text = " ";
            unitClass.text = "";
            attackStat.text = "";
            speedStat.text = "";
            sightStat.text = "";
            hpStat.text = "";
            unitName.interactable = false;

            icons.SetActive(false);
        }
        if(activeIcon != -1)
        {
            if(unitToMove != null)
            {
                UnitStats unitStats = unitToMove.GetComponent<UnitStats>();
                if (activeIcon == 0) { iconText.text = "Move up to <color=green>" + unitStats.speed + "</color> tiles."; }
                string action1Description = ReplaceKeyWordsInString(unitStats.action1.description, unitStats, unitStats.action1);
                if (activeIcon == 1) { iconText.text = action1Description; }
                string action2Description = ReplaceKeyWordsInString(unitStats.action2.description, unitStats, unitStats.action2);
                if (activeIcon == 2) { iconText.text = action2Description; }
                if (activeIcon == 3) { iconText.text = "Interact with an object within <color=green>2</color> tiles."; }

                if (unitStats.isEnemy)
                {
                    iconText.text = "Kill or inspect this unit to learn more about it's abilities.";
                }
            }
        }
        if(unitToMove != null)
        {
            if (unitToMove.GetComponent<UnitStats>().isCorpse)
            {
                portraitImage.gameObject.SetActive(false);
                unitName.gameObject.SetActive(false);
                unitClass.gameObject.SetActive(false);
                attackStat.gameObject.SetActive(false);
                speedStat.gameObject.SetActive(false);
                sightStat.gameObject.SetActive(false);
                hpStat.gameObject.SetActive(false);
                icons.gameObject.SetActive(false);
            }
            else
            {
                portraitImage.gameObject.SetActive(true);
                unitName.gameObject.SetActive(true);
                unitClass.gameObject.SetActive(true);
                attackStat.gameObject.SetActive(true);
                speedStat.gameObject.SetActive(true);
                sightStat.gameObject.SetActive(true);
                hpStat.gameObject.SetActive(true);
                icons.gameObject.SetActive(true);
            }
        }
    }
    string ReplaceKeyWordsInString(string original, UnitStats unitStats, ActionClass action)
    {
        string newString = original.Replace("DAMAGE", "<color=red>" + (Mathf.RoundToInt(unitStats.attack * action.damageMult)- 1) + " - " + (Mathf.RoundToInt(unitStats.attack * action.damageMult) + 1) + "</color>");
        newString = newString.Replace("MINRANGE", "<color=green>" + (action.minRange - 1) + "</color>");
        newString = newString.Replace("RANGE", "<color=green>" + (action.range - 1) + "</color>");


        return newString;
    }
}
