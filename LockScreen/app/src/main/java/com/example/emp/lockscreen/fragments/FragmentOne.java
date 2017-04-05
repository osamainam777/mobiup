package com.example.emp.lockscreen.fragments;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.emp.lockscreen.R;

/**
 * Created by emp on 2/9/2017.
 */

public class FragmentOne extends Fragment {

    public FragmentOne(){

    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        ViewGroup rootView = (ViewGroup) inflater.inflate(
                R.layout.fragment_one, container, false);

        return rootView;
    }
}
