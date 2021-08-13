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
    // public void AddItem(Item item) {

        // Debug.Log ("Received one " + itemName);

        int stacks = 1;
        UpdateItem(itemName, stacks);
    }

    public void RemoveItem(string itemName) {
        // Debug.Log ("Lost one " + item.Name);

        int stacks = -1;
        UpdateItem(itemName, stacks);
    }

    // public void RemoveItem(Item item) {
    //     // Debug.Log ("Lost one " + item.Name);

    //     int stacks = -1;
    //     UpdateItem(item.Name, stacks);
    // }

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

    // public bool CheckItem(string itemName) {
    public bool CheckItem(Item item) {
        // Debug.Log ("CheckItem");
        if (itemList.ContainsKey(item.Name)) {
            // Debug.Log( "you have the item");
            if (itemList[item.Name] > 0) {
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

    // public bool CheckItem(string itemName) {
    public bool CheckItem(string itemName) {
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

        // for testing v
        // int counter = 0;
        // for(int q = 0; q < 100; q++) {
        //     int test = Random.Range(0, 2);

        //     if (test == 1) {
        //         counter++;
        //     }
        // }
        // Debug.Log(counter);

        int randomIndex = Random.Range(0, itemCount);
        // Debug.Log(randomIndex.ToString());

        int i = 0;

        foreach (KeyValuePair<string,int> item in itemList) {
            i += item.Value;

            // todo test for off by one
            if (i > randomIndex) {
                return item.Key;
            }
        }

        // Debug.Log("Random Item not found");
        return "Error";

        // while (t = 0) {
        //     for (j = 0; )
        // }
    }

    public Dictionary<string,int> GetInventoryAsDictionary()
    {
        return itemList;
    }
}
