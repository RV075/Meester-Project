using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    private float timers = 0;
    public TextMeshProUGUI UI_timer;
    // Update is called once per frame
    void Update()
    {
        timers += Time.deltaTime;
        UI_timer.text = Mathf.RoundToInt(timers).ToString();
    }
}
