using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class CheckButtonAndAxis : MonoBehaviour
{

    private string currentButton; //当前按下的按键

    private string currentAxis; //当前移动的轴向

    private float axisInput; //当前轴向的值

    private void Update()
    {
        //getButtons();
        //getAxis();

        //// 实时打印当前按下的按键
        //if (!currentButton.Equals(string.Empty))
        //{
        //    Debug.Log("Current Button : " + currentButton);
        //}

        ////实时打印当前按下的轴和轴的量
        //if (!currentAxis.Equals(string.Empty))
        //{
        //    Debug.Log("Current Axis : " + currentAxis);
        //    Debug.Log("Axis Value : " + axisInput);
        //}
        if (Input.GetKeyDown(KeyCode.None)) Debug.Log("KeyCode.None");
    }

    private void getButtons()
    {
        currentButton = string.Empty;

        var values = Enum.GetValues(typeof(KeyCode));//存储所有的按键

        for (int x = 0; x < values.Length; x++)
        {

            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                currentButton = values.GetValue(x).ToString();//遍历并获取当前按下的按键
            }
        }
    }

    private void getAxis()
    {
        currentAxis = string.Empty;

        if (Mathf.Abs(Input.GetAxisRaw("LeftstickHorizontal")) > 0.3)
        {
            currentAxis = "LeftstickHorizontal";
            axisInput = Input.GetAxisRaw("LeftstickHorizontal");
        }
        if (Mathf.Abs(Input.GetAxisRaw("LeftstickVertical")) > 0.3)
        {
            currentAxis = "LeftstickVertical";
            axisInput = Input.GetAxisRaw("LeftstickVertical");
        }
        if (Mathf.Abs(Input.GetAxisRaw("DpadHorizontal")) > 0.3)
        {
            currentAxis = "DpadHorizontal";
            axisInput = Input.GetAxisRaw("DpadHorizontal");
        }
        if (Mathf.Abs(Input.GetAxisRaw("DpadVertical")) > 0.3)
        {
            currentAxis = "DpadVertical";
            axisInput = Input.GetAxisRaw("DpadVertical");
        }
        if (Mathf.Abs(Input.GetAxisRaw("RightstickHorizontal")) > 0.3)
        {
            currentAxis = "RightstickHorizontal";
            axisInput = Input.GetAxisRaw("RightstickHorizontal");
        }
        if (Mathf.Abs(Input.GetAxisRaw("RightstickVertical")) > 0.3)
        {
            currentAxis = "RightstickVertical";
            axisInput = Input.GetAxisRaw("RightstickVertical");
        }
        if (Mathf.Abs(Input.GetAxisRaw("Trigger")) > 0.3)
        {
            currentAxis = "Trigger";
            axisInput = Input.GetAxisRaw("Trigger");
        }
        //if (Mathf.Abs(Input.GetAxisRaw("Eight")) > 0.3)
        //{
        //    currentAxis = "Eight";
        //    axisInput = Input.GetAxisRaw("Eight");
        //}
        //if (Mathf.Abs(Input.GetAxisRaw("Nine")) > 0.3)
        //{
        //    currentAxis = "Nine";
        //    axisInput = Input.GetAxisRaw("Nine");
        //}
    }
}

