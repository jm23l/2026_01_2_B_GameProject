using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDataBaseSo", menuName = "Scriptable Objects/ItemDataBaseSo")]
public class ItemDataBaseSo : ScriptableObject
{
    public List<ItemSo> items = new List<ItemSo>();                 //ItemSo를 리스트로 관리 한다.

    private Dictionary<int, ItemSo> itemByld;                       //ID로 아이템 찾기 위한 캐싱
    private Dictionary<string, ItemSo> itemByName;                  //이름으로 아이템 찾기


    public void Initialize()
    {
        itemByld = new Dictionary<int, ItemSo>();                   //위에 선언만 했기 때문에 Dictionary 할당
        itemByName = new Dictionary<string, ItemSo>();

        foreach(var item in items)
        {
            itemByld[item.id] = item;
            itemByName[item.itemName] = item;
        }
    }

    public ItemSo GetItemByld(int id)
    {
        if(itemByld == null)                                        //캐싱이 되어있는지 확인하고 아니면 초기화 한다.
        {
            Initialize();
        }

        if (itemByld.TryGetValue(id, out ItemSo item))              //id 값을 찾아서 ItemSo를 리턴한다.
            return item;

        return null;                                                //없을 경우 NULL
    }
    // 이름으로 아이템 찾기
    public ItemSo GetItemByName(string name)
    {
        if(itemByName == null)                                      //캐싱이 되어있는지 확인하고 아니면 초기화 한다
        {
            Initialize();  
        }

        if (itemByName.TryGetValue(name, out ItemSo item))          //Name 값을 찾아서 ItemsSo를 리턴한다.
            return item;

        return null;
    }

    // 타입으로 아이템 필터링
    public List<ItemSo> GetItemByType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }
}

