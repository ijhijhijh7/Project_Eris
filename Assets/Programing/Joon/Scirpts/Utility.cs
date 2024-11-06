using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public static class Utility
{
    public static async Task DelayAction(float delaySeconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
    }
}
