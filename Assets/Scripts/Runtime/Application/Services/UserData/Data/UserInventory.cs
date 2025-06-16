using System;
using System.Collections.Generic;

[Serializable]
public class UserInventory
{
    public int Balance = 0;
    public int UsedGameItemID = 0;
    public List<int> PurchasedGameItemsIDs = new List<int>() { 0 };
}