using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    // Start is called before the first frame update
    float time;
    private TextMeshProUGUI text;
    void Awake()
    {
        time = 0;
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        time += Time.deltaTime;
    }

    void Update()
    {
        text.text = "Time: " + Mathf.RoundToInt(time);
    }
}
