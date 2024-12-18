using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void OnItemCollected(int value)
    {
        if (_player != null)
        {
            _player.CollectItem(value);
        }
    }
    
}
