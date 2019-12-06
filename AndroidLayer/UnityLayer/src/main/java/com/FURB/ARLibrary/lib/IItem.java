package com.FURB.ARLibrary.lib;

import java.util.UUID;

public interface IItem {
    UUID getId();
    float getLatitude();
    float getLongigute();
    String getWavefrontFile();
}
