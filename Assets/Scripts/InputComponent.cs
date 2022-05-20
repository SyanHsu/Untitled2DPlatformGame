using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class InputComponent : MonoBehaviour
{
    public enum InputType
    {
        Controller,
        MouseAndKeyboard,
    }

    public enum XboxControllerButton
    {
        None,
        A,
        B,
        X,
        Y,
        LeftBumper,
        RightBumper,
        Back,
        Start,
        Leftstick,
        Rightstick,
        LeftTrigger,
        RightTrigger,
        Left,
        Right,
        Up,
        Down,
    }
    
    public enum XboxControllerAxis
    {
        None,
        LeftstickHorizontal,
        LeftstickVertical,
        DpadHorizontal,
        DpadVertical,
        RightstickHorizontal,
        RightstickVertical,
        Trigger,
    }

    public static readonly Dictionary<XboxControllerButton, KeyCode> ControllerButtonToKeyCode =
        new Dictionary<XboxControllerButton, KeyCode>
        {
            {XboxControllerButton.None, KeyCode.None},
            {XboxControllerButton.A, KeyCode.JoystickButton0},
            {XboxControllerButton.B, KeyCode.JoystickButton1},
            {XboxControllerButton.X, KeyCode.JoystickButton2},
            {XboxControllerButton.Y, KeyCode.JoystickButton3},
            {XboxControllerButton.LeftBumper, KeyCode.JoystickButton4},
            {XboxControllerButton.RightBumper, KeyCode.JoystickButton5},
            {XboxControllerButton.Back, KeyCode.JoystickButton6},
            {XboxControllerButton.Start, KeyCode.JoystickButton7},
            {XboxControllerButton.Leftstick, KeyCode.JoystickButton8},
            {XboxControllerButton.Rightstick, KeyCode.JoystickButton9},
            {XboxControllerButton.LeftTrigger, KeyCode.None},
            {XboxControllerButton.RightTrigger, KeyCode.None},
            {XboxControllerButton.Left, KeyCode.None},
            {XboxControllerButton.Right, KeyCode.None},
            {XboxControllerButton.Up, KeyCode.None},
            {XboxControllerButton.Down, KeyCode.None},
        };

    public static readonly Dictionary<KeyCode, XboxControllerButton> KeyCodeToControllerButton =
        new Dictionary<KeyCode, XboxControllerButton>
        {
            {KeyCode.None, XboxControllerButton.None},
            {KeyCode.JoystickButton0, XboxControllerButton.A},
            {KeyCode.JoystickButton1, XboxControllerButton.B},
            {KeyCode.JoystickButton2, XboxControllerButton.X},
            {KeyCode.JoystickButton3, XboxControllerButton.Y},
            {KeyCode.JoystickButton4, XboxControllerButton.LeftBumper},
            {KeyCode.JoystickButton5, XboxControllerButton.RightBumper},
            {KeyCode.JoystickButton6, XboxControllerButton.Back},
            {KeyCode.JoystickButton7, XboxControllerButton.Start},
            {KeyCode.JoystickButton8, XboxControllerButton.Leftstick},
            {KeyCode.JoystickButton9, XboxControllerButton.Rightstick},
        };

    public static readonly Dictionary<XboxControllerButton, string> ControllerButtonToAxisName =
        new Dictionary<XboxControllerButton, string>
    {
            {XboxControllerButton.LeftTrigger, "Trigger"},
            {XboxControllerButton.RightTrigger, "Trigger"},
            {XboxControllerButton.Left, "DpadHorizontal"},
            {XboxControllerButton.Right, "DpadHorizontal"},
            {XboxControllerButton.Up, "DpadVertical"},
            {XboxControllerButton.Down, "DpadVertical"},
    };

    public static readonly Dictionary<XboxControllerButton, bool> ControllerButtonToAxisDir =
        new Dictionary<XboxControllerButton, bool>
    {
            {XboxControllerButton.LeftTrigger, false},
            {XboxControllerButton.RightTrigger, true},
            {XboxControllerButton.Left, false},
            {XboxControllerButton.Right, true},
            {XboxControllerButton.Up, true},
            {XboxControllerButton.Down, false},
    };

    public static readonly Dictionary<XboxControllerAxis, string> ControllerAxisToAxisName =
        new Dictionary<XboxControllerAxis, string>
    {
            {XboxControllerAxis.None, String.Empty},
            {XboxControllerAxis.LeftstickHorizontal, "LeftstickHorizontal"},
            {XboxControllerAxis.LeftstickVertical, "LeftstickVertical"},
            {XboxControllerAxis.DpadHorizontal, "DpadHorizontal"},
            {XboxControllerAxis.DpadVertical, "DpadVertical"},
            {XboxControllerAxis.RightstickHorizontal, "RightstickHorizontal"},
            {XboxControllerAxis.RightstickVertical, "RightstickVertical"},
            {XboxControllerAxis.Trigger, "Trigger"},
    };

    [Serializable]
    public class InputButtonInfo
    {
        public XboxControllerButton ControllerButton { get; private set; }
        public bool IsControllerKey { get; private set; }
        public KeyCode ControllerKeyCode { get; private set; }
        public string ControllerAxisName { get; private set; }
        public bool IsAxisPositive { get; private set; }
        public bool IsKey { get; private set; }
        public KeyCode KeyButton { get; private set; }
        public int MouseButton { get; private set; }

        public InputButtonInfo(XboxControllerButton controllerButton = XboxControllerButton.None, 
            KeyCode keyButton = KeyCode.None)
        {
            ChangeControllerButton(controllerButton);
            ChangeKeyButton(keyButton);
        }

        public InputButtonInfo(XboxControllerButton controllerButton, int mouseButton)
        {
            ChangeControllerButton(controllerButton);
            ChangeMouseButton(mouseButton);
        }

        public InputButtonInfo(InputButtonInfo copy)
        {
            CopyFrom(copy);
        }

        public void CopyFrom(InputButtonInfo copy)
        {
            ControllerButton = copy.ControllerButton;
            IsControllerKey = copy.IsControllerKey;
            ControllerKeyCode = copy.ControllerKeyCode;
            ControllerAxisName = copy.ControllerAxisName;
            IsAxisPositive = copy.IsAxisPositive;
            IsKey = copy.IsKey;
            KeyButton = copy.KeyButton;
            MouseButton = copy.MouseButton;
        }

        public void ChangeControllerButton(XboxControllerButton controllerButton)
        {
            ControllerButton = controllerButton;
            ControllerKeyCode = ControllerButtonToKeyCode[ControllerButton];

            if (ControllerButton != XboxControllerButton.None && ControllerKeyCode == KeyCode.None)
            {
                IsControllerKey = false;
                ControllerAxisName = ControllerButtonToAxisName[ControllerButton];
                IsAxisPositive = ControllerButtonToAxisDir[ControllerButton];
            }
            else
            {
                IsControllerKey = true;
            }
        }

        public void ChangeControllerButton(KeyCode controllerKeyCode)
        {
            ControllerKeyCode = controllerKeyCode;
            ControllerButton = KeyCodeToControllerButton[ControllerKeyCode];
            IsControllerKey = true;
        }

        public void ChangeControllerButton(string controllerAxisName, bool isAxisPositive)
        {
            ControllerAxisName = controllerAxisName;
            IsAxisPositive = isAxisPositive;
            switch (ControllerAxisName)
            {
                case "Trigger":
                    if (IsAxisPositive) ControllerButton = XboxControllerButton.RightTrigger;
                    else ControllerButton = XboxControllerButton.LeftTrigger;
                    break;
                case "DpadHorizontal":
                    if (IsAxisPositive) ControllerButton = XboxControllerButton.Right;
                    else ControllerButton = XboxControllerButton.Left;
                    break;
                case "DpadVertical":
                    if (IsAxisPositive) ControllerButton = XboxControllerButton.Up;
                    else ControllerButton = XboxControllerButton.Down;
                    break;
                default:
                    ControllerButton = XboxControllerButton.None;
                    break;
            }
            IsControllerKey = false;
        }

        public void ChangeKeyButton(KeyCode keyButton)
        {
            IsKey = true;
            KeyButton = keyButton;
        }

        public void ChangeMouseButton(int mouseButton)
        {
            IsKey = false;
            MouseButton = mouseButton;
        }
    }

    [Serializable]
    public class InputButton
    {
        public InputButtonInfo buttonInfo;
        public InputButtonInfo buttonInfo_Default;

        private bool m_Enabled = true;

        // 仅当前一帧和此帧之间至少发生了FixedUpdate时，这用于更改按钮的状态
        private bool m_AfterFixedUpdateDown = false;
        private bool m_AfterFixedUpdateHeld = false;
        private bool m_AfterFixedUpdateUp = false;

        public bool Held { get; private set; }
        public bool Down { get; private set; }
        public bool Up { get; private set; }
        public bool Enabled
        {
            get { return m_Enabled; }
        }

        public InputButton(XboxControllerButton controllerButton = XboxControllerButton.None, 
            KeyCode keyButton = KeyCode.None)
        {
            buttonInfo_Default = new InputButtonInfo(controllerButton, keyButton);
            buttonInfo = new InputButtonInfo(buttonInfo_Default);
        }

        public InputButton(XboxControllerButton controllerButton, int mouseButton)
        {
            buttonInfo_Default = new InputButtonInfo(controllerButton, mouseButton);
            buttonInfo = new InputButtonInfo(buttonInfo_Default);
        }

        public void Enable()
        {
            m_Enabled = true;
        }

        public void Disable()
        {
            m_Enabled = false;
        }

        private bool m_lastHeld = false;

        public void Get(bool fixedUpdateHappened, InputType inputType)
        {
            if (!m_Enabled)
            {
                Held = false;
                Down = false;
                Up = false;
                return;
            }

            if (inputType == InputType.Controller)
            {
                if (buttonInfo.IsControllerKey)
                {
                    Held = Input.GetKey(buttonInfo.ControllerKeyCode);
                    Down = Input.GetKeyDown(buttonInfo.ControllerKeyCode);
                    Up = Input.GetKeyUp(buttonInfo.ControllerKeyCode);
                }
                else
                {
                    if (buttonInfo.IsAxisPositive)
                    {
                        Held = Input.GetAxisRaw(buttonInfo.ControllerAxisName) > Single.Epsilon;
                    }
                    else
                    {
                        Held = Input.GetAxisRaw(buttonInfo.ControllerAxisName) < -Single.Epsilon;
                    }
                    Down = !m_lastHeld && Held;
                    Up = m_lastHeld && !Held;
                    m_lastHeld = Held;
                }
            }
            else if (inputType == InputType.MouseAndKeyboard)
            {
                if (buttonInfo.IsKey)
                {
                    Held = Input.GetKey(buttonInfo.KeyButton);
                    Down = Input.GetKeyDown(buttonInfo.KeyButton);
                    Up = Input.GetKeyUp(buttonInfo.KeyButton);
                }
                else
                {
                    Held = Input.GetMouseButton(buttonInfo.MouseButton);
                    Down = Input.GetMouseButtonDown(buttonInfo.MouseButton);
                    Up = Input.GetMouseButtonUp(buttonInfo.MouseButton);
                }
            }

            if (fixedUpdateHappened)
            {
                m_AfterFixedUpdateHeld = Held;
                m_AfterFixedUpdateDown = Down;
                m_AfterFixedUpdateUp = Up;
            }
            else
            {
                Held |= m_AfterFixedUpdateHeld;
                Down |= m_AfterFixedUpdateDown;
                Up |= m_AfterFixedUpdateUp;

                m_AfterFixedUpdateHeld |= Held;
                m_AfterFixedUpdateDown |= Down;
                m_AfterFixedUpdateUp |= Up;
            }
        }

        public void ResetToDefault()
        {
            buttonInfo.CopyFrom(buttonInfo_Default);
        }
    }

    public void ChangeControllerButton(InputButton button)
    {
        StartCoroutine(GetControllerButton(button.buttonInfo));

        // 进行改键之后的UI改动
    }

    private IEnumerator GetControllerButton(InputButtonInfo buttonInfo)
    {
        bool getButton = false;
        var keys = KeyCodeToControllerButton.Keys;
        List<string> axisList = new List<string> { "Trigger", "DpadHorizontal", "DpadVertical" };
        while (!getButton)
        {
            foreach (var key in keys)
            {
                if (Input.GetKeyDown(key))
                {
                    buttonInfo.ChangeControllerButton(key);
                    getButton = true;
                    break;
                }
            }
            foreach (var axis in axisList)
            {
                if (Mathf.Abs(Input.GetAxisRaw(axis)) > Single.Epsilon)
                {
                    buttonInfo.ChangeControllerButton(axis, Input.GetAxisRaw(axis) > Single.Epsilon);
                    getButton = true;
                    break;
                }
            }
        }
        yield return null;
    }

    public void ChangeKeyButton(InputButton button)
    {
        StartCoroutine(GetKeyButton(button.buttonInfo));

        // 进行改键之后的UI改动
    }

    private IEnumerator GetKeyButton(InputButtonInfo buttonInfo)
    {
        bool getButton = false;
        while (!getButton)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        buttonInfo.ChangeKeyButton(keyCode);
                        break;
                    }
                }
                break;
            }
            else
            {
                for (int mouseButton = 0; mouseButton < 3; mouseButton++)
                {
                    if (Input.GetMouseButtonDown(mouseButton))
                    {
                        buttonInfo.ChangeMouseButton(mouseButton);
                        getButton = true;
                        break;
                    }
                }
            }
        }
        yield return null;
    }

    [Serializable]
    public class InputAxisInfo
    {
        public XboxControllerAxis ControllerAxis { get; private set; }
        public string ControllerAxisName { get; private set; }
        public KeyCode Positive { get; private set; }
        public KeyCode Negative { get; private set; }

        public InputAxisInfo(XboxControllerAxis controllerAxis = XboxControllerAxis.None, 
            KeyCode positive = KeyCode.None, KeyCode negative = KeyCode.None)
        {
            ChangeControllerAxis(controllerAxis);
            ChangeKeyAxis(positive, true);
            ChangeKeyAxis(negative, false);
        }

        public InputAxisInfo(InputAxisInfo copy)
        {
            CopyFrom(copy);
        }

        public void CopyFrom(InputAxisInfo copy)
        {
            ControllerAxis = copy.ControllerAxis;
            ControllerAxisName = copy.ControllerAxisName;
            Positive = copy.Positive;
            Negative = copy.Negative;
        }

        public void ChangeKeyAxis(KeyCode key, bool isPositive)
        {
            if (isPositive) Positive = key;
            else Negative = key;
        }

        public void ChangeControllerAxis(XboxControllerAxis controllerAxis)
        {
            ControllerAxis = controllerAxis;
            ControllerAxisName = ControllerAxisToAxisName[ControllerAxis];
        }
    }

    [Serializable]
    public class InputAxis
    {
        public InputAxisInfo axisInfo;
        public InputAxisInfo axisInfo_Alternate;

        private bool m_Enabled = true;
        private bool m_HasAlternate = false;

        public float Value { get; private set; }
        public bool ReceivingInput { get; private set; }
        public bool Enabled
        {
            get { return m_Enabled; }
        }

        public InputAxis(XboxControllerAxis controllerAxis = XboxControllerAxis.None, 
            KeyCode positive = KeyCode.None, KeyCode negative = KeyCode.None, 
            XboxControllerAxis controllerAxis_Alternate = XboxControllerAxis.None)
        {
            axisInfo = new InputAxisInfo(controllerAxis, positive, negative);
            axisInfo_Alternate = new InputAxisInfo(controllerAxis_Alternate);
            m_HasAlternate = controllerAxis_Alternate != XboxControllerAxis.None;
        }

        public void Enable()
        {
            m_Enabled = true;
        }

        public void Disable()
        {
            m_Enabled = false;
        }

        public void Get(InputType inputType)
        {
            if (!m_Enabled)
            {
                Value = 0f;
                return;
            }

            bool positiveHeld = false;
            bool negativeHeld = false;

            if (inputType == InputType.Controller)
            {
                float value = Input.GetAxisRaw(axisInfo.ControllerAxisName);
                
                positiveHeld = value > Single.Epsilon;
                negativeHeld = value < -Single.Epsilon;
                if (m_HasAlternate)
                {
                    float value_Alternate = Input.GetAxisRaw(axisInfo_Alternate.ControllerAxisName);
                    positiveHeld |= value_Alternate > Single.Epsilon;
                    negativeHeld |= value_Alternate < -Single.Epsilon;
                }
            }
            else if (inputType == InputType.MouseAndKeyboard)
            {
                positiveHeld = Input.GetKey(axisInfo.Positive);
                negativeHeld = Input.GetKey(axisInfo.Negative);
            }

            if (positiveHeld == negativeHeld)
                Value = 0f;
            else if (positiveHeld)
                Value = 1f;
            else
                Value = -1f;

            ReceivingInput = positiveHeld || negativeHeld;
        }
    }

    public void ChangeKeyAxis(InputAxis axis, bool isPositive)
    {
        StartCoroutine(GetKeyAxis(axis.axisInfo, isPositive));

        // 进行改键之后的UI改动
    }

    private IEnumerator GetKeyAxis(InputAxisInfo axisInfo, bool isPositive)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        axisInfo.ChangeKeyAxis(keyCode, isPositive);
                        break;
                    }
                }
                break;
            }
        }
        yield return null;
    }

    public InputType inputType = InputType.MouseAndKeyboard;

    private bool m_FixedUpdateHappened;

    private void Update()
    {
        GetInputs(m_FixedUpdateHappened || Mathf.Approximately(Time.timeScale, 0));

        m_FixedUpdateHappened = false;
    }

    private void FixedUpdate()
    {
        m_FixedUpdateHappened = true;
    }

    protected abstract void GetInputs(bool fixedUpdateHappened);
}