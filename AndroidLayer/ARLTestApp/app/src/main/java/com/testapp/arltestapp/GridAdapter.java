package com.testapp.arltestapp;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.FURB.ARLibrary.lib.IItem;

public class GridAdapter extends BaseAdapter {
    private Context context;

    public GridAdapter(Context context) {
        this.context = context;
    }

    @Override
    public int getCount() {
        return ItemsRepository.GetInstance().getItems().size();
    }

    @Override
    public Object getItem(int position) {
        return ItemsRepository.GetInstance().getItems().get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        LayoutInflater inflater = (LayoutInflater) context
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE);

        View gridView;
        if (convertView == null) {
            gridView = inflater.inflate(R.layout.item, null);
            TextView id_item = (TextView) gridView.findViewById(R.id.id_item);
            TextView lat_item = (TextView) gridView.findViewById(R.id.lat_item);
            TextView long_item = (TextView) gridView.findViewById(R.id.long_item);

            IItem item = ItemsRepository.GetInstance().getItems().get(position);
            id_item.setText(item.getId().toString());
            lat_item.setText(item.getLatitude() + "");
            long_item.setText(item.getLongigute() + "");
        } else {
            gridView = (View) convertView;
        }

        return gridView;
    }
}
