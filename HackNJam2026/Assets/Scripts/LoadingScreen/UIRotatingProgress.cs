using UnityEngine;
using UnityEngine.UI;

public class UIRotatingProgress : UILoadingProgressCore
{
    public Image ProgressImage;
    public float SpinSpeed = 1f;
    
 
    // Update is called once per frame
    void Update()
    {
        ProgressImage.rectTransform.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
    }

    public override void Reset()
    {
    }

    public override void SetProgress(float progress)
    {
    }
}
