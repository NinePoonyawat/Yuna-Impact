using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool Targetable(Enemy enemy)
    {
        return true;
    }
}
