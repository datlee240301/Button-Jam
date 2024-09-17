using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// 5, 10 ,15,20, 30,40,50,100
public class ShopItem
{
    public int posPack;
    public int numRuby;
    public Sprite sprite;
    public float price;
}


public class ShopManager :Singleton<ShopManager>
{
 //   public int posPack;
    [SerializeField] private Transform transformSpawn;
    [SerializeField] private Transform shopItemPf;
    [SerializeField] private ShopItem[] items;

    public IAPManager iAPManager;

    private void Start()
    {      
        Debug.Log("ShopManager start");
        iAPManager.SetupBuilder();
    }

    public void EventHandlerBuy(int posPack)
    {
        iAPManager.HandleInitiatePurchase(posPack);
    }

    public void SetLayoutItemIAP(List<ItemIAP> listItems, List<int> listPosPack)
    {
        Debug.Log("SetLayoutItemIAP");
        for (int i = 0; i < listItems.Count; i++)
        {
            ShopItemUI shopItemUI = Instantiate(shopItemPf, transformSpawn).GetComponent<ShopItemUI>();
            shopItemUI.UpdateUI(items[listPosPack[i]-1].numRuby, items[listPosPack[i]-1].sprite, listItems[i].price, listPosPack[i]);
        }
    }
}
