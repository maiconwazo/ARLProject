package com.testapp.arltestapp;

import com.FURB.ARLibrary.lib.*;
import java.util.ArrayList;
import java.util.List;

public class ItemsRepository implements IRepository {
    private static ItemsRepository instance;
    private List<IItem> itemList;

    public ItemsRepository(){
        instance = this;
        this.itemList = new ArrayList();
    }

    public List<IItem> getItems()    {
        return itemList;
    }

    public List<IItem> getItems(float latitude, float longitude)    {
        return itemList;
    }

    public static ItemsRepository GetInstance()    {
        return instance;
    }

}
