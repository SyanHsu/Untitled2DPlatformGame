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

    private static readonly Dictionary<XboxControllerButton, KeyCode> ControllerButtonToKeyCode =
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

    private static readonly Dictionary<KeyCode, XboxControllerButton> KeyCodeToControllerButton =
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

    private static readonly Dictionary<XboxControllerButton, string> ControllerButtonToAxisName =
        new Dictionary<XboxControllerButton, string>
    {
            {XboxControllerButton.LeftTrigger, "Trigger"},
            {XboxControllerButton.RightTrigger, "Trigger"},
            {XboxControllerButton.Left, "DpadHorizontal"},
            {XboxControllerButton.Right, "DpadHorizontal"},
            {XboxControllerButton.Up, "DpadVertical"},
            {XboxControllerButton.Down, "DpadVertical"},
    };

    private static readonly Dictionary<XboxControllerButton, bool> ControllerButtonToAxisDir =
        new Dictionary<XboxControllerButton, bool>
    {
            {XboxControllerButton.LeftTrigger, false},
            {XboxControllerButton.RightTrigger, true},
            {XboxControllerButton.Left, false},
            {XboxControllerButton.Right, true},
            {XboxControllerButton.Up, true},
            {XboxControllerButton.Down, false},
    };

    private static readonly Dictionary<XboxControllerAxis, string> ControllerAxisToAxisName =
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
        [SerializeField]
        private XboxControllerButton m_ControllerButton;
        public XboxControllerButton ControllerButton { get { return m_ControllerButton; } }
        public bool IsControllerKey { get; private set; }
        public KeyCode ControllerKeyCode { get; private set; }
        public string ControllerAxisName { get; private set; }
        public bool IsAxisPositive { get; private set; }

        [SerializeField]
        private bool m_IsKey;
        public bool IsKey { get { return m_IsKey; } }
        [SerializeField]
        private KeyCode m_KeyButton;
        public KeyCode KeyButton { get { return m_KeyButton; } }
        [SerializeField]
        private int m_MouseButton;
        public int MouseButton { get { return m_MouseButton; } }

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
            m_ControllerButton = copy.ControllerButton;
            IsControllerKey = copy.IsControllerKey;
            ControllerKeyCode = copy.ControllerKeyCode;
            ControllerAxisName = copy.ControllerAxisName;
            IsAxisPositive = copy.IsAxisPositive;
            m_IsKey = copy.IsKey;
            m_KeyButton = copy.KeyButton;
            m_MouseButton = copy.MouseButton;
        }

        public void ChangeControllerButton()
        {
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

        public void ChangeControllerButton(XboxControllerButton controllerButton)
        {
            m_ControllerButton = controllerButton;
            ChangeControllerButton();
        }

        public void ChangeControllerButton(KeyCode controllerKeyCode)
        {
            ControllerKeyCode = controllerKeyCode;
            m_ControllerButton = KeyCodeToControllerButton[ControllerKeyCode];
            IsControllerKey = true;
        }

        public void ChangeControllerButton(string controllerAxisName, bool isAxisPositive)
        {
            ControllerAxisName = controllerAxisName;
            IsAxisPositive = isAxisPositive;
            switch (ControllerAxisName)
            {
                case "Trigger":
                    if (IsAxisPositive) m_ControllerButton = XboxControllerButton.RightTrigger;
                    else m_ControllerButton = XboxControllerButton.LeftTrigger;
                    break;
                case "DpadHorizontal":
                    if (IsAxisPositive) m_ControllerButton = XboxControllerButton.Right;
                    else m_ControllerButton = XboxControllerButton.Left;
                    break;
                case "DpadVertical":
                    if (IsAxisPositive) m_ControllerButton = XboxControllerButton.Up;
                    else m_ControllerButton = XboxControllerButton.Down;
                    break;
                default:
                    m_ControllerButton = XboxControllerButton.None;
                    break;
            }
            IsControllerKey = false;
        }

        public void ChangeKeyButton(KeyCode keyButton)
        {
            m_IsKey = true;
            m_KeyButton = keyButton;
        }

        public void ChangeMouseButton(int mouseButton)
        {
            m_IsKey = false;
            m_MouseButton = mouseButton;
        }
    }

    [Serializable]
    public class InputButton
    {
        public InputButtonInfo buttonInfo;
        public InputButtonInfo buttonInfo_Default;

        [SerializeField]
        private bool m_Enabled = true;
        private bool m_GettingInput = true;

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
            currentButton = buttonInfo.ControllerButton;
        }

        public InputButton(XboxControllerButton controllerButton, int mouseButton)
        {
            buttonInfo_Default = new InputButtonInfo(controllerButton, mouseButton);
            buttonInfo = new InputButtonInfo(buttonInfo_Default);
            currentButton = buttonInfo.ControllerButton;
        }

        public void Enable()
        {
            m_Enabled = true;
        }

        public void Disable()
        {
            m_Enabled = false;
        }

        public void GainControl()
        {
            m_GettingInput = true;
        }

        public IEnumerator ReleaseControl(bool resetValues)
        {
            m_GettingInput = false;

            if (!resetValues)
                yield break;

            if (Down)
                Up = true;
            Down = false;
            Held = false;

            m_AfterFixedUpdateDown = false;
            m_AfterFixedUpdateHeld = false;
            m_AfterFixedUpdateUp = false;

            yield return null;

            Up = false;
        }

        private bool m_lastHeld = false;
        private XboxControllerButton currentButton;

        public void Get(bool fixedUpdateHappened, InputType inputType)
        {
            if (!m_Enabled)
            {
                Held = false;
                Down = false;
                Up = false;
                return;
            }

            if (!m_GettingInput)
                return;

            if (inputType == InputType.Controller)
            {
                if (currentButton != buttonInfo.ControllerButton)
                {
                    buttonInfo.ChangeControllerButton();
                    currentButton = buttonInfo.ControllerButton;
                }
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
                        Held = Input.GetAxisRaw(buttonInfo.ControllerAxisName) > float.Epsilon;
                    }
                    else
                    {
                        Held = Input.GetAxisRaw(buttonInfo.ControllerAxisName) < -float.Epsilon;
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
                if (Mathf.Abs(Input.GetAxisRaw(axis)) > float.Epsilon)
                {
                    buttonInfo.ChangeControllerButton(axis, Input.GetAxisRaw(axis) > float.Epsilon);
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
        [HideInInspector]
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

        public void ChangeControllerAxis()
        {
            ControllerAxisName = ControllerAxisToAxisName[ControllerAxis];
        }
        public void ChangeControllerAxis(XboxControllerAxis controllerAxis)
        {
            ControllerAxis = controllerAxis;
            ChangeControllerAxis();
        }

        public void ChangeKeyAxis(KeyCode key, bool isPositive)
        {
            if (isPositive) Positive = key;
            else Negative = key;
        }
    }

    [Serializable]
    public class InputAxis
    {
        public InputAxisInfo axisInfo;
        public InputAxisInfo axisInfo_Alternate;

        [SerializeField]
        private bool m_HasAlternate = false;
        [SerializeField]
        private bool m_Enabled = true;
        private bool m_GettingInput = true;

        public float Value { get; private set; }
        public bool HasAlternate
        {
            get { return m_HasAlternate; }
        }
        public bool Enabled
        {
            get { return m_Enabled; }
        }
        public bool ReceivingInput { get; private set; }

        public InputAxis(XboxControllerAxis controllerAxis = XboxControllerAxis.None, 
            KeyCode positive = KeyCode.None, KeyCode negative = KeyCode.None, 
            XboxControllerAxis controllerAxis_Alternate = XboxControllerAxis.None)
        {
            axisInfo = new InputAxisInfo(controllerAxis, positive, negative);
            axisInfo_Alternate = new InputAxisInfo(controllerAxis_Alternate);
            m_HasAlternate = controllerAxis_Alternate != XboxControllerAxis.None;
            currentAxis = axisInfo.ControllerAxis;
            currentAxis_Alternate = axisInfo_Alternate.ControllerAxis;
        }

        public void Enable()
        {
            m_Enabled = true;
        }

        public void Disable()
        {
            m_Enabled = false;
        }

        public void GainControl()
        {
            m_GettingInput = true;
        }

        public void ReleaseControl(bool resetValues)
        {
            m_GettingInput = false;
            if (resetValues)
            {
                Value = 0f;
                ReceivingInput = false;
            }
        }

        private XboxControllerAxis currentAxis;
        private XboxControllerAxis currentAxis_Alternate;
        public void Get(InputType inputType)
        {
            if (!m_Enabled)
            {
                Value = 0f;
                return;
            }

            if (!m_GettingInput)
                return;

            bool positiveHeld = false;
            bool negativeHeld = false;

            if (inputType == InputType.Controller)
            {
                if (currentAxis != axisInfo.ControllerAxis)
                {
                    axisInfo.ChangeControllerAxis();
                    currentAxis = axisInfo.ControllerAxis;
                }
                float value = Input.GetAxisRaw(axisInfo.ControllerAxisName);
                
                positiveHeld = value > float.Epsilon;
                negativeHeld = value < -float.Epsilon;
                if (m_HasAlternate)
                {
                    if (currentAxis_Alternate != axisInfo_Alternate.ControllerAxis)
                    {
                        axisInfo_Alternate.ChangeControllerAxis();
                        currentAxis_Alternate = axisInfo_Alternate.ControllerAxis;
                    }
                    float value_Alternate = Input.GetAxisRaw(axisInfo_Alternate.ControllerAxisName);
                    positiveHeld |= value_Alternate > float.Epsilon;
                    negativeHeld |= value_Alternate < -float.Epsilon;
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

    protected void GainControl(InputButton inputButton)
    {
        inputButton.GainControl();
    }

    protected void GainControl(InputAxis inputAxis)
    {
        inputAxis.GainControl();
    }

    protected void ReleaseControl(InputButton inputButton, bool resetValues)
    {
        StartCoroutine(inputButton.ReleaseControl(resetValues));
    }

    protected void ReleaseControl(InputAxis inputAxis, bool resetValues)
    {
        inputAxis.ReleaseControl(resetValues);
    }
}