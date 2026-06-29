using UnityEngine;
using TMPro;

public class MushroomItem : InventoryItem
{
    public GameObject damageNum;
    public MushroomItem()
    {
        itemID = 4;
    }
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
        if (unitCon.unitToMove != null)
        {
            int damage = 3;
            if(unitCon.unitToMove.GetComponent<UnitStats>().hpCurrent <= 3)
            {
                NoActivate();
                return;
            }
            unitCon.unitToMove.GetComponent<UnitStats>().hpCurrent -= damage;
            unitCon.unitToMove.GetComponent<UnitStats>().tempStr += 2;
            unitCon.unitToMove.GetComponent<UnitStats>().attack += 2;

            GameObject a = Instantiate(damageNum, unitCon.unitToMove.transform.position + new Vector3(0, .25f), unitCon.unitToMove.transform.rotation);
            a.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.red;
            a.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
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
