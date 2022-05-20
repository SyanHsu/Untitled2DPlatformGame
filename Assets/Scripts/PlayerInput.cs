using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerInput : InputComponent
{
    public static PlayerInput Instance;

    public InputButton Pause = new InputButton(XboxControllerButton.Start, KeyCode.Escape);
    public InputButton Menu = new InputButton(XboxControllerButton.Back, KeyCode.P);
    public InputButton Interact = new InputButton(XboxControllerButton.Y, KeyCode.Q);
    public InputButton MeleeAttack = new InputButton(XboxControllerButton.RightBumper, 0);
    public InputButton RangedAttack = new InputButton(XboxControllerButton.X, 1);
    public InputButton Heal = new InputButton(XboxControllerButton.B, KeyCode.E);
    public InputButton Jump = new InputButton(XboxControllerButton.A, KeyCode.Space);
    public InputButton Dash = new InputButton(XboxControllerButton.RightTrigger, KeyCode.LeftShift);
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
        RangedAttack.Get(fixedUpdateHappened, inputType);
        Heal.Get(fixedUpdateHappened, inputType);
        Jump.Get(fixedUpdateHappened, inputType);
        Dash.Get(fixedUpdateHappened, inputType);
        Horizontal.Get(inputType);
        Vertical.Get(inputType);
    }
}