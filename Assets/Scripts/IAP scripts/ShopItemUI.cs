using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour {
    public int posPack;
    public Image img;
    public TextMeshProUGUI textprice;
    public TextMeshProUGUI textnumRuby;
    public Button buyButton;

    private void Start()
    {
        buyButton.onClick.AddListener(()=>
            OnClickBuy()
        );
    }

    public void UpdateUI( int numRuby, Sprite sprite, string price, int pos)
    {
        textnumRuby.text = "X" + numRuby.ToString();
        textprice.text = price.ToString();
        posPack = pos;
        img.sprite = sprite;
   
    }

    public void OnClickBuy()
    {      
        int posPack = this.posPack;
        ShopManager.Instance.EventHandlerBuy(posPack);
        Debug.Log("postPack" + posPack);
    }

}
