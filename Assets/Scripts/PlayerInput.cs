using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerInput : InputComponent
{
    public static PlayerInput Instance;

    private bool m_HaveControl = true;

    public bool HaveControl { get { return m_HaveControl; } }

    public InputButton Pause = new InputButton(XboxControllerButton.Start, KeyCode.Escape);
    public InputButton Menu = new InputButton(XboxControllerButton.Back, KeyCode.P);
    public InputButton Interact = new InputButton(XboxControllerButton.Y, KeyCode.Q);
    public InputButton MeleeAttack = new InputButton(XboxControllerButton.RightBumper, 0);
    public InputButton Block = new InputButton(XboxControllerButton.X, 1);
    public InputButton Heal = new InputButton(XboxControllerButton.B, KeyCode.E);
    public InputButton Jump = new InputButton(XboxControllerButton.A, KeyCode.Space);
    public InputButton Roll = new InputButton(XboxControllerButton.RightTrigger, KeyCode.LeftShift);
    public InputAxis Horizontal = new InputAxis(XboxControllerAxis.LeftstickHorizontal, KeyCode.D, KeyCode.A, XboxControllerAxis.DpadHorizontal);
    public InputAxis Vertical = new InputAxis(XboxControllerAxis.LeftstickVertical, KeyCode.W, KeyCode.S, XboxControllerAxis.DpadVertical);

    void Awake()
    {
        Instance = this;
    }

    protected override void GetInputs(bool fixedUpdateHappened)
    {
        Pause.Get(fixedUpdateHappened, inputType);
        Menu.Get(fixedUpdateHappened, inputType);
        Interact.Get(fixedUpdateHappened, inputType);
        MeleeAttack.Get(fixedUpdateHappened, inputType);
        Block.Get(fixedUpdateHappened, inputType);
        Heal.Get(fixedUpdateHappened, inputType);
        Jump.Get(fixedUpdateHappened, inputType);
        Roll.Get(fixedUpdateHappened, inputType);
        Horizontal.Get(inputType);
        Vertical.Get(inputType);
    }

    public void GainControl()
    {
        m_HaveControl = true;

        GainControl(Pause);
        GainControl(Menu);
        GainControl(Interact);
        GainControl(MeleeAttack);
        GainControl(Block);
        GainControl(Heal);
        GainControl(Jump);
        GainControl(Roll);
        GainControl(Horizontal);
        GainControl(Vertical);
    }

    public void ReleaseControl(bool resetValues = true)
    {
        m_HaveControl = false;

        ReleaseControl(Pause, resetValues);
        ReleaseControl(Menu, resetValues);
        ReleaseControl(Interact, resetValues);
        ReleaseControl(MeleeAttack, resetValues);
        ReleaseControl(Block, resetValues);
        ReleaseControl(Heal, resetValues);
        ReleaseControl(Jump, resetValues);
        ReleaseControl(Roll, resetValues);
        ReleaseControl(Horizontal, resetValues);
        ReleaseControl(Vertical, resetValues);
    }
}