using UnityEngine;
using TMPro;

public class FoodItem : InventoryItem
{
    public GameObject damageNum;
    public override void ActivateItem()
    {
        UnitControler unitCon = null;
        if (GameObject.Find("Controller") != null)
        {
            unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
        }
        else
        {
            NoActivate();
            return;
        }
        foreach(GameObject ally in unitCon.allyUnits)
        {
            if(ally == null) { continue; }
            int missingHP = ally.GetComponent<UnitStats>().hpMax - ally.GetComponent<UnitStats>().hpCurrent;
            missingHP = Mathf.Clamp(missingHP, 0, 3); //Clamps to heal ammount
            ally.GetComponent<UnitStats>().hpCurrent += missingHP;

            GameObject a = Instantiate(damageNum, ally.transform.position + new Vector3(0, .25f), ally.transform.rotation);
            a.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
            a.transform.GetChild(0).GetComponent<TMP_Text>().text = missingHP.ToString();

        }
        FillSlots(curretPos, 0);
        Destroy(this.gameObject);
    }
    private void NoActivate()
    {
        Debug.Log("No active controller!");
    }
}
