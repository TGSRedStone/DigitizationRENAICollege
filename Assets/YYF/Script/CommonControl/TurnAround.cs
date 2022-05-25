using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnAround : MonoBehaviour
{
    // Start is called before the first frame update
    public int turnSpeed;
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
