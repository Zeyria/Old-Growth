using UnityEngine;
using TMPro;

public class PotionItem : InventoryItem
{
    public GameObject damageNum;
    public override void ActivateItem()
    {
        UnitControler unitCon = null;
        if(GameObject.Find("Controller") != null)
        {
            unitCon = GameObject.Find("Controller").GetComponent<UnitControler>();
        }
        if(unitCon.unitToMove != null)
        {
            if (unitCon.unitToMove.GetComponent<UnitStats>().actionPointCurrent < unitCon.unitToMove.GetComponent<UnitStats>().actionPointMax)
            {
                unitCon.unitToMove.GetComponent<UnitStats>().actionPointCurrent++;
                GameObject a = Instantiate(damageNum, unitCon.unitToMove.transform.position + new Vector3(0, .25f), unitCon.unitToMove.transform.rotation);
                a.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.yellow;
                a.transform.GetChild(0).GetComponent<TMP_Text>().text = "1";
                FillSlots(curretPos, 0);
                Destroy(this.gameObject);
            }
            else
            {
                NoActivate();
            }
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
