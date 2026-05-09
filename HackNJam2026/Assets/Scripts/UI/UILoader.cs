using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILoader : MonoBehaviour
{
    
    public TextMeshProUGUI LoadText;
    
    private List<string> Texts = new List<string>()
    {
        "Loading.",
        "Loading..",
        "Loading...",
    };
    private EasyTimer Timer = new EasyTimer(0.33f);
    private int Current = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (Timer.Update(Time.deltaTime))
        {
            Current++;
            LoadText.text = Texts[Current % 3];
        }
    }
}
