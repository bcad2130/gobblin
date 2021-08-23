using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    private Dictionary<string,int> itemList = new Dictionary<string,int>();


    public void AddItem(string itemName)
    {

        // Debug.Log ("Received one " + itemName);

        int stacks = 1;
        UpdateItem(itemName, stacks);
    }

    public void RemoveItem(string itemName)
    {
        // Debug.Log ("Lost one " + item.Name);

        int stacks = -1;
        UpdateItem(itemName, stacks);
    }

    public void UpdateItem(string itemName, int stacks)
    {
        if (itemList.ContainsKey(itemName)) {
            itemList[itemName] += stacks;
        } else {
            itemList.Add(itemName, stacks);
        }
    }

    // public void PrintItems() {
    //     foreach (Item item in itemList) {
    //         Debug.Log (item);
    //         // print (item.Name);
    //     }
    // }

    // public bool CheckItem(string itemName) {
    // public bool CheckItem(Item item) {
    //     // Debug.Log ("CheckItem");
    //     if (itemList.ContainsKey(item.Name)) {
    //         // Debug.Log( "you have the item");
    //         if (itemList[item.Name] > 0) {
    //             return true;
    //         }
    //     }

    //     return false;

    //     // PrintItems();
    //     // Debug.Log(itemList.Contains (item));
    //     // Debug.Log(item);

    //     // List<Item> test = new List<Item>();
    //     // AppleItem apple = new AppleItem();

    //     // test.Add(apple);

    //     // // Debug.Log(test.Contains(apple));
    //     // Debug.Log(item);
    //     // Debug.Log(apple);


    //     // Debug.Log(apple == item);

    //     // return itemList.Contains (item);
    // }

    public bool CheckItem(string itemName)
    {
        // Debug.Log ("CheckItem");
        if (itemList.ContainsKey(itemName)) {
            if (itemList[itemName] > 0) {
                return true;
            }
        }

        return false;
    }

    public int CountItems()
    {
        int itemCount = 0;

        foreach (KeyValuePair<string,int> item in itemList) {
            itemCount += item.Value;
        }

        return itemCount;
    }

    public string GetRandomItem()
    {
        int itemCount = CountItems();
        int randomIndex = Random.Range(0, itemCount);
        int i = 0;

        foreach (KeyValuePair<string,int> item in itemList) {
            i += item.Value;

            if (i > randomIndex) {
                return item.Key;
            }
        }

        Debug.Log("Random Item not found");
        return "Error";
    }

    public Dictionary<string,int> GetInventoryAsDictionary()
    {
        return itemList;
    }
}
