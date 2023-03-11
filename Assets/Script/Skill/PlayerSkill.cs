using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : Skill
{
    protected PlayerController playerController;
    protected PlayableCharacter character;
    protected Vector3 centerScreen;

    public virtual void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        character = gameObject.GetComponent<PlayableCharacter>();
        centerScreen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    public abstract void PlayerInput();
    public abstract void AIActivate();
    public abstract int AICalculate();
}
