package com.testapp.arltestapp;

import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import java.io.InputStream;

public class NewItemActivity extends Activity {

    TextView latitude;
    TextView longitude;
    TextView formIndex;
    Button cadastrar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.new_item_activity);

        if (ContextCompat.checkSelfPermission(this, "android.permission.ACCESS_COARSE_LOCATION") != 0) {
            ActivityCompat.requestPermissions(this, new String[]{"android.permission.ACCESS_COARSE_LOCATION"}, 11);
        }

        latitude = (TextView) findViewById(R.id.latitutde_edit);
        longitude = (TextView)  findViewById(R.id.longitude_edit);
        formIndex = (TextView)  findViewById(R.id.forma_edit);
        cadastrar = (Button)  findViewById(R.id.btCadastrar);
        cadastrar.setOnClickListener(v -> {
            float latitude_value = Float.parseFloat(latitude.getText().toString().replace(",","."));
            float longitude_value = Float.parseFloat(longitude.getText().toString().replace(",","."));
            int forma_value = Integer.parseInt(formIndex.getText().toString());

            switch (forma_value)
            {
                case 1:
                    forma_value = R.raw.circle;
                    break;
                case 2:
                    forma_value = R.raw.sofa;
                    break;
                case 3:
                    forma_value = R.raw.tree;
                    break;
            }

            String file = "";
            try {
                InputStream in_s = getResources().openRawResource(forma_value);
                byte[] b = new byte[in_s.available()];
                in_s.read(b);
                file = new String(b);
            } catch (Exception e) {
                file = "";
            }

            Toast.makeText(getBaseContext(), "Item cadastrados com sucesso!", Toast.LENGTH_LONG).show();
            ItemsRepository.GetInstance().getItems().add(new Item(latitude_value, longitude_value, file));
            ResetaValores();
        });

        ResetaValores();
    }

    public void ResetaValores()   {
        if ( Build.VERSION.SDK_INT >= 23 &&
                ContextCompat.checkSelfPermission( this, android.Manifest.permission.ACCESS_FINE_LOCATION ) != PackageManager.PERMISSION_GRANTED &&
                ContextCompat.checkSelfPermission( this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            return  ;
        }

        LocationManager lm = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
        Location location = lm.getLastKnownLocation(LocationManager.GPS_PROVIDER);

        latitude.setText(location.getLatitude() + "");
        longitude.setText(location.getLongitude() + "");
        formIndex.setText("1");
    }
}
