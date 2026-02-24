using UnityEngine;
using TMPro;

public class BandageItem : InventoryItem
{
    public GameObject damageNum;
    public override void ActivateItem()
    {
        UnitControler unitCon = null;
        if (GameObject.Find("Controller") != null)
        {
            unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
        }
        if (unitCon.unitToMove != null)
        {
            int missingHP = unitCon.unitToMove.GetComponent<UnitStats>().hpMax - unitCon.unitToMove.GetComponent<UnitStats>().hpCurrent;
            missingHP = Mathf.Clamp(missingHP, 0, 5); //Clamps to heal ammount
            if(missingHP <= 0)
            {
                NoActivate();
                return;
            }
            unitCon.unitToMove.GetComponent<UnitStats>().hpCurrent += missingHP;

            GameObject a = Instantiate(damageNum, unitCon.unitToMove.transform.position + new Vector3(0, .25f), unitCon.unitToMove.transform.rotation);
            a.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
            a.transform.GetChild(0).GetComponent<TMP_Text>().text = missingHP.ToString();
            FillSlots(curretPos, 0);
            Destroy(this.gameObject);
        }
        else
        {
            NoActivate();
        }
    }
    private void NoActivate()
    {
        //Debug.Log("No active unit!");
    }
}
