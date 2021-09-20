using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearPack : MonoBehaviour
{
    public int id;

    public bool bought;

    public ShopPack opt;

    public Image box;
    public Text header;
    public Text months;

    public void Update(){
        if(PlayerPrefs.GetInt("SP" + id) > 0 ||(opt != null && opt.bought)){
            box.color = Color.gray;
            header.color = Color.gray;
            months.color = Color.grey;
            bought = true;
            PlayerPrefs.SetInt("SP" + id, 1);
        }
    }
}
