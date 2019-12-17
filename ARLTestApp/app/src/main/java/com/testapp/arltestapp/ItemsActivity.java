package com.testapp.arltestapp;

import android.app.Activity;
import android.os.Bundle;
import android.widget.GridView;

public class ItemsActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.items_activity);

        GridView grid = (GridView) findViewById(R.id.grid);
        grid.setAdapter(new GridAdapter(this));
    }
}
