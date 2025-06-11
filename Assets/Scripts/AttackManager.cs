using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Attack currentAttack;
    public Attack nexttAttack;
    public NewPlayerController playerController;

    public AttackManager(NewPlayerController _playerController)
    {
        playerController = _playerController;
    }


}
