package com.testapp.arltestapp;


import com.FURB.ARLibrary.lib.IItem;

import java.util.UUID;

public class Item implements IItem {
    private UUID id;
    private float latitude;
    private float longitude;
    private String wavefrontFile;

    public Item(float latitude, float longitude, String wavefrontFile)
    {
        this.id = UUID.randomUUID();
        this.latitude = latitude;
        this.longitude = longitude;
        this.wavefrontFile = wavefrontFile;
    }

    @Override
    public UUID getId() {
        return id;
    }

    @Override
    public float getLatitude() {
        return latitude;
    }

    @Override
    public float getLongigute() {
        return longitude;
    }

    @Override
    public String getWavefrontFile() { return wavefrontFile; }
}
