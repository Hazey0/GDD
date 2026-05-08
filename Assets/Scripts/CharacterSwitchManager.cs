using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitchManager : MonoBehaviour
{
    [Header("Characters")]
    public HumanController humanController;
    public FalconController falconController;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("Visual Feedback")]
    public GameObject humanActiveMarker;
    public GameObject falconActiveMarker;

    private bool controllingHuman = true;
    public AudioSource SwitchToHuman;
    public AudioSource SwitchToBird;
    public AudioSource WindSound;


    private void Start()
    {
        SetControlState(true);
    }

    public void OnSwitchCharacter(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;


        if (controllingHuman && falconController.IsPerchedAway())
            return; //cannot switch to falcon when it's perched.

        controllingHuman = !controllingHuman;
        SetControlState(controllingHuman);
    }

    private void SetControlState(bool humanActive)
    {
        humanController.SetActiveCharacter(humanActive);
        falconController.SetActiveCharacter(!humanActive);

        if (cameraFollow != null)
        {
            if (humanActive)
            {
                cameraFollow.SetFalconMode(false);
                cameraFollow.SetTarget(humanController.transform);
                SwitchToHuman.Play();
                WindSound.Stop();
            }
            else
            {
                cameraFollow.SetTarget(falconController.transform);
                cameraFollow.SetFalconMode(true);
                SwitchToBird.Play();
                WindSound.Play();

            }
        }

        if (humanActiveMarker != null)
            humanActiveMarker.SetActive(humanActive);

        if (falconActiveMarker != null)
            falconActiveMarker.SetActive(!humanActive);
    }
}