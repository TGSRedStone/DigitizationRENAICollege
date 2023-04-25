using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChartSlider : SliderDataBase
{
    public float chartSliderValue;

    private void Start()
    {
        SliderMaxValue = chartSliderValue;
        FindSliderValueGamObject();
    }
    // Update is called once per frame
    void Update()
    {
        SliderValueChage();
    }
    public override void SliderValueChage()
    {
        if (SliderStartValue <= SliderMaxValue)
        {
            SliderStartValue += Time.deltaTime / 3;

        }
        //if(SliderStartValue == SliderMaxValue)
        //SliderMaxValue += Random.Range(-0.2f, 0.2f);
        //if (SliderStartValue >= SliderMaxValue) {
        //    SliderStartValue -= Time.deltaTime/10;
        //}
        ThisGameObjectSlider.value = SliderStartValue;
        ThisGameObjectTextMeshPro.text = (int)(SliderStartValue * 100) + "%";
    }
    public override void FindSliderValueGamObject()
    {
        ThisGameObjectSlider = this.GetComponentInChildren<Slider>();
        UIScript = this.GetComponentInParent<BuildingUI>();
        ThisGameObjectTextMeshPro = this.GetComponentInChildren<TextMeshProUGUI>();
        SliderMaxValue = chartSliderValue;
    }
}
