package com.example.emp.lockscreen;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

/**
 * Created by osama on 3/23/2017.
 */

public class AmountAdpater extends RecyclerView.Adapter<AmountAdpater.AmountViewHolder> {
    @Override
    public AmountAdpater.AmountViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_layout, parent, false);
        return new AmountViewHolder(view);
    }

    @Override
    public void onBindViewHolder(AmountAdpater.AmountViewHolder holder, int position) {

        holder.date.setText("Date: "+getYesterdayDateString(position+1));

    }
    private Date yesterday(int i) {
        final Calendar cal = Calendar.getInstance();
        cal.add(Calendar.DATE, -i);
        return cal.getTime();
    }
    private String getYesterdayDateString(int i) {
        DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd");
        return dateFormat.format(yesterday(i));
    }

    @Override
    public int getItemCount() {
        return 10;
    }

    public class AmountViewHolder extends RecyclerView.ViewHolder {
        TextView date;
        public AmountViewHolder(View itemView) {
            super(itemView);
            date = (TextView)itemView.findViewById(R.id.genre);
        }
    }
}
