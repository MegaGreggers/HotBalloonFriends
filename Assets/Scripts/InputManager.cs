using UnityEngine;
using System.Collections;

public class InputButton
{
    public InputButton(int playerIndex, string buttonName)
    {
        mButtonName = buttonName + "_" + playerIndex;
        //for example: mButtonName may equal "A_1" which is A Button for Player 1, as they are defined by this string format in Unity's InputManager.
    }

    public void Update()
    {
        // Value = Input.GetButton(mButtonName) ? 1f : 0f;
        WentDown = Input.GetButtonDown(mButtonName);
        IsDown = Input.GetButton(mButtonName);
        WentUp = Input.GetButtonUp(mButtonName);
    }

    public float Value;
    public bool WentDown;
    public bool IsDown;
    public bool WentUp;

    private string mButtonName;
}
/* PS4 Controller - Unity Input values
 
 Buttons: 
    Square  = joystick button 0
    X       = joystick button 1
    Circle  = joystick button 2
    Triangle= joystick button 3
    L1      = joystick button 4
    R1      = joystick button 5
    L2      = joystick button 6
    R2      = joystick button 7
    Share	= joystick button 8
    Options = joystick button 9
    L3      = joystick button 10
    R3      = joystick button 11
    PS      = joystick button 12
    PadPress= joystick button 13

Axes:
    LeftStickX      = X-Axis
    LeftStickY      = Y-Axis (Inverted?)
    RightStickX     = 3rd Axis
    RightStickY     = 4th Axis (Inverted?)
    L2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    R2              = 6th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    DPadX           = 7th Axis
    DPadY           = 8th Axis (Inverted?)
 */

public class PlayersInputs
{
    private float UI_AnanlogSensitivity = 0.5f;
    float LeftStick_X_lastFrame;
    float LeftStick_Y_lastFrame;
    float DPad_X_lastFrame;
    float DPad_Y_lastFrame;

    public PlayersInputs(int playerIndex)
    {
        mPlayerIndex = playerIndex;
        AButton = new InputButton(playerIndex, "A");
        BButton = new InputButton(playerIndex, "B");
        XButton = new InputButton(playerIndex, "X");
        YButton = new InputButton(playerIndex, "Y");
        StartButton = new InputButton(playerIndex, "Start");
        BackButton = new InputButton(playerIndex, "Back");
        LBumperButton = new InputButton(playerIndex, "LB");
        RBumperButton = new InputButton(playerIndex, "RB");
        LTriggerButton = new InputButton(playerIndex, "LT");
        RTriggerButton = new InputButton(playerIndex, "RT");
        UI_Movement_Up = new InputButton(playerIndex, "UI_MoveUp");
        UI_Movement_Down = new InputButton(playerIndex, "UI_MoveDown");
        UI_Movement_Left = new InputButton(playerIndex, "UI_MoveLeft");
        UI_Movement_Right = new InputButton(playerIndex, "UI_MoveRight");
        Dpad_Movement_Up = new InputButton(playerIndex, "DPad_MoveUp");
        Dpad_Movement_Down = new InputButton(playerIndex, "DPad_MoveDown");
        Dpad_Movement_Left = new InputButton(playerIndex, "DPad_MoveLeft");
        Dpad_Movement_Right = new InputButton(playerIndex, "DPad_MoveRight");
    }

    public void Update()
    {
        LeftStick_X = Input.GetAxis("L_XAxis_" + mPlayerIndex);
        LeftStick_Y = Input.GetAxis("L_YAxis_" + mPlayerIndex);
        LeftStick = new Vector2(LeftStick_X, LeftStick_Y);
        LTriggerButton.Value = Input.GetAxis("LT_" + mPlayerIndex);

        RightStick_X = Input.GetAxis("R_XAxis_" + mPlayerIndex);
        RightStick_Y = Input.GetAxis("R_YAxis_" + mPlayerIndex);
        RightStick = new Vector2(RightStick_X, RightStick_Y);
        RTriggerButton.Value = Input.GetAxis("RT_" + mPlayerIndex);

        DPad_X = Input.GetAxis("DPad_XAxis_" + mPlayerIndex);
        DPad_Y = Input.GetAxis("DPad_YAxis_" + mPlayerIndex);

        AButton.Update();
        BButton.Update();
        XButton.Update();
        YButton.Update();
        StartButton.Update();
        BackButton.Update();
        LBumperButton.Update();
        RBumperButton.Update();

        UI_Movement_Up.IsDown = (LeftStick_Y > UI_AnanlogSensitivity);
        UI_Movement_Down.IsDown = (LeftStick_Y < -UI_AnanlogSensitivity);
        UI_Movement_Left.IsDown = (LeftStick_X < -UI_AnanlogSensitivity);
        UI_Movement_Right.IsDown = (LeftStick_X > UI_AnanlogSensitivity);

        UI_Movement_Up.WentDown = (LeftStick_Y_lastFrame < UI_AnanlogSensitivity && LeftStick_Y >= UI_AnanlogSensitivity) ? true : false;
        UI_Movement_Down.WentDown = (LeftStick_Y_lastFrame > -UI_AnanlogSensitivity && LeftStick_Y <= -UI_AnanlogSensitivity) ? true : false;
        UI_Movement_Left.WentDown = (LeftStick_X_lastFrame > -UI_AnanlogSensitivity && LeftStick_X <= -UI_AnanlogSensitivity) ? true : false;
        UI_Movement_Right.WentDown = (LeftStick_X_lastFrame < UI_AnanlogSensitivity && LeftStick_X >= UI_AnanlogSensitivity) ? true : false;

        LeftStick_X_lastFrame = LeftStick_X;
        LeftStick_Y_lastFrame = LeftStick_Y;

        Dpad_Movement_Up.IsDown = (DPad_Y > UI_AnanlogSensitivity);
        Dpad_Movement_Down.IsDown = (DPad_Y < -UI_AnanlogSensitivity);
        Dpad_Movement_Left.IsDown = (DPad_X < -UI_AnanlogSensitivity);
        Dpad_Movement_Right.IsDown = (DPad_X > UI_AnanlogSensitivity);

        Dpad_Movement_Up.WentDown = (DPad_Y_lastFrame < UI_AnanlogSensitivity && DPad_Y >= UI_AnanlogSensitivity) ? true : false;
        Dpad_Movement_Down.WentDown = (DPad_Y_lastFrame > -UI_AnanlogSensitivity && DPad_Y <= -UI_AnanlogSensitivity) ? true : false;
        Dpad_Movement_Left.WentDown = (DPad_X_lastFrame > -UI_AnanlogSensitivity && DPad_X <= -UI_AnanlogSensitivity) ? true : false;
        Dpad_Movement_Right.WentDown = (DPad_X_lastFrame < UI_AnanlogSensitivity && DPad_X >= UI_AnanlogSensitivity) ? true : false;

        DPad_X_lastFrame = DPad_X;
        DPad_Y_lastFrame = DPad_Y;
    }

    private int mPlayerIndex;

    public float LeftStick_X;
    public float LeftStick_Y;
    public Vector2 LeftStick;
    
    public float RightStick_X;
    public float RightStick_Y;
    public Vector2 RightStick;

    public float DPad_X;
    public float DPad_Y;
    public Vector2 DPad;

    public InputButton AButton;
    public InputButton BButton;
    public InputButton XButton;
    public InputButton YButton;
    public InputButton StartButton;
    public InputButton BackButton;
    public InputButton LBumperButton;
    public InputButton RBumperButton;
    public InputButton LTriggerButton;
    public InputButton RTriggerButton;
    public InputButton UI_Movement_Up;
    public InputButton UI_Movement_Down;
    public InputButton UI_Movement_Left;
    public InputButton UI_Movement_Right;
    public InputButton Dpad_Movement_Up;
    public InputButton Dpad_Movement_Down;
    public InputButton Dpad_Movement_Left;
    public InputButton Dpad_Movement_Right;
}


