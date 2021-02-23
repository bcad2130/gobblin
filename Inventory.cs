using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    // public int ItemCount { get; set; }

    // private List<Item> itemList = new List<Item>();

    // retrying with dict format
    private Dictionary<string,int> itemList = new Dictionary<string,int>();


    // private void Awake()
    // {
    //     // itemList = new List<Item>();
    // }

    // public void AddItem(Item item) {
    //     Debug.Log ("Received one" + item);

    //     // Item testItem = new Item();
    //     // testItem = item;
    //     // Debug.Log (itemList);
    //     itemList.Add (item);
    // }

    // public void RemoveItem(Item item) {
    //     itemList.Remove (item);
    // }

    public void AddItem(string itemName) {
        Debug.Log ("Received one " + itemName);

        int stacks = 1;
        UpdateItem(itemName, stacks);
    }

    public void RemoveItem(string itemName) {
        Debug.Log ("Lost one " + itemName);

        int stacks = -1;
        UpdateItem(itemName, stacks);
    }

    public void UpdateItem(string itemName, int stacks) {
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

    public bool CheckItem(string itemName) {

        if (itemList.ContainsKey(itemName)) {
            if (itemList[itemName] > 0) {
                return true;
            }
        }

        return false;

        // PrintItems();
        // Debug.Log(itemList.Contains (item));
        // Debug.Log(item);

        // List<Item> test = new List<Item>();
        // AppleItem apple = new AppleItem();

        // test.Add(apple);

        // // Debug.Log(test.Contains(apple));
        // Debug.Log(item);
        // Debug.Log(apple);


        // Debug.Log(apple == item);

        // return itemList.Contains (item);
    }
}
