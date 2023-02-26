using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    bool isTaking = false; // is this character taking by player
    
    public bool Targetable(Enemy enemy)
    {
        return true;
    }

    public void SetTaking(bool newTaking)
    {
        isTaking = newTaking;
    }
}
