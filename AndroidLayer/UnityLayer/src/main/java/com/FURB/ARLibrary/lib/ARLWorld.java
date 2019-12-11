package com.FURB.ARLibrary.lib;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.view.View;
import android.widget.Button;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import com.FURB.ARLibrary.UnityPlayerActivity;

import java.util.List;

public class ARLWorld {
    private static ARLWorld instance;
    private IRepository repository;

    public ARLWorld(Activity activity, Button btnInit, IRepository repository) {
        instance = this;
        this.repository = repository;
        btnInit.setOnClickListener((View v) -> {
            Intent unityIntent = new Intent(activity, UnityPlayerActivity.class);
            activity.startActivity(unityIntent);
        });

        if (ContextCompat.checkSelfPermission( activity, Manifest.permission.ACCESS_FINE_LOCATION ) != PackageManager.PERMISSION_GRANTED ) {

            ActivityCompat.requestPermissions( activity, new String[] {  Manifest.permission.ACCESS_FINE_LOCATION  },
                    102 );
        }

        if (ContextCompat.checkSelfPermission( activity, android.Manifest.permission.ACCESS_COARSE_LOCATION ) != PackageManager.PERMISSION_GRANTED ) {

            ActivityCompat.requestPermissions( activity, new String[] {  Manifest.permission.ACCESS_COARSE_LOCATION  },
                    101 );
        }

        if (ContextCompat.checkSelfPermission( activity, Manifest.permission.CAMERA ) != PackageManager.PERMISSION_GRANTED ) {

            ActivityCompat.requestPermissions( activity, new String[] {  Manifest.permission.CAMERA  },
                    100 );
        }
    }

    public static ARLWorld getARLWorldIntance()  {
        return instance;
    }

    public String getNearItems(float latitude, float longitude){
        String retorno = "";
        List<IItem> itemList = repository.getItems(latitude, longitude);

        for (IItem item : itemList) {
            if (!retorno.isEmpty())
                retorno += ",";

            retorno += String.format("{ \"Id\": \"%s\", \"Latitude\": %s, \"Longitude\": %s, \"Wavefront\": \"%s\", \"Placed\": false }",
                    item.getId().toString(), item.getLatitude(), item.getLongigute(),
                    item.getWavefrontFile().replace("\r", "").replace("\n", "\\n"));
        }

        return String.format("{ \"Items\": [%s] }", retorno);
    }
}
