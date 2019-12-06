package com.testapp.arltestapp;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import com.FURB.ARLibrary.lib.ARLWorld;

public class MainActivity extends Activity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main_activity);

        ItemsRepository repository = new ItemsRepository(); // repositório implementado

        if (ContextCompat.checkSelfPermission(this, Manifest.permission.CAMERA) != 0) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.CAMERA}, 100);
        }

        // binding de botões
        Button btnCamera = (Button) findViewById(R.id.btnCamera);
        Button btnNewItem = (Button) findViewById(R.id.btnNewItem);
        Button btnGetItems = (Button) findViewById(R.id.btnGetItems);

        btnNewItem.setOnClickListener(v -> { // configurando tela de cadastro
            Intent itemsActivity = new Intent(this, NewItemActivity.class);
            startActivity(itemsActivity);
        });

        btnGetItems.setOnClickListener(v -> { // configurando tela de consulta
            Intent itemsActivity = new Intent(this, ItemsActivity.class);
            startActivity(itemsActivity);
        });

        ARLWorld arlWorld = new ARLWorld(this, btnCamera, repository); // instanciando a aplicação
    }
}
