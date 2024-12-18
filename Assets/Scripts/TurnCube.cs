using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCube : MonoBehaviour
{
    [SerializeField] private float _turnAngle;
    [SerializeField] private Player.MovementDirection _newDirection;

    private bool _isPlayerEnter = false;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !_isPlayerEnter)
        {
            _isPlayerEnter = true;
            StartCoroutine(player.TurnCharacter(_turnAngle));
            if(_newDirection == Player.MovementDirection.GlobalZ)
            {
                player.SetMovementDirection(_newDirection, transform.position.x);
            }
            else
            {
                player.SetMovementDirection(_newDirection, transform.position.z);
            }
        }
    }
}
