using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarFade : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image barImage;
    void Start()
    {
        SetHealth(0.8f);
    }
    private void Awake()
    {
        barImage = transform.Find("Bar").GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetHealth(float healthNor)
    {
        barImage.fillAmount = healthNor;
    }
}
