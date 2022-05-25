using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class SliderDataBase : MonoBehaviour
{
    public int SliderCount;
    public Slider ThisGameObjectSlider;
    protected BuildingUI UIScript;

    [Tooltip("第一个进度条的最大值")]
    public float SliderMaxValue;
    protected float SliderStartValue = 0;
    protected TextMeshProUGUI ThisGameObjectTextMeshPro;
    private void Start()
    {
        FindSliderValueGamObject();
    }
    private void OnEnable()
    {
        SliderStartValue = 0;
    }
    public virtual void FindSliderValueGamObject() {
        ThisGameObjectSlider = this.GetComponentInChildren<Slider>();
        UIScript = this.GetComponentInParent<BuildingUI>();
        ThisGameObjectTextMeshPro = this.GetComponentInChildren<TextMeshProUGUI>();
        //SliderMaxValue = UIScript.SliderCountData[SliderCount];
    }
    public virtual void SliderValueChage()
    {
        if (SliderStartValue <= SliderMaxValue)
        {
            SliderStartValue += Time.deltaTime / 4;

        }
        //if(SliderStartValue == SliderMaxValue)
        //SliderMaxValue += Random.Range(-0.2f, 0.2f);
        //if (SliderStartValue >= SliderMaxValue) {
        //    SliderStartValue -= Time.deltaTime/10;
        //}
        ThisGameObjectSlider.value = SliderStartValue;
        ThisGameObjectTextMeshPro.text = (int)(SliderStartValue * 100) + "%";
    }
    private void Update()
    {
        SliderValueChage();

    }
    public interface SetSliderValue {
        void SliderValueChage();


    }
}
