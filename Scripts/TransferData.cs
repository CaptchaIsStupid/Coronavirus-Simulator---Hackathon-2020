﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferData : MonoBehaviour
{
    public DataStorage Object,prefab;
    public GameObject me;

    public void ToPrefab()
    {
        prefab.population = Object.population;
        prefab.healthCareCapacity = Object.healthCareCapacity;
        prefab.cooperation = Object.cooperation;
        prefab.infected = Object.infected;
    }

    public void ToObject()
    {
        Object.population = prefab.population;
        Object.healthCareCapacity = prefab.healthCareCapacity;
        Object.cooperation = prefab.cooperation;
        Object.infected = prefab.infected;
    }

    private void Update()
    {
        DontDestroyOnLoad(me);
    }
}
