using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameCanvas _gameCanvas;
    [SerializeField] private AudioClip _finish; 

    [SerializeField] private Transform[] _doors;
    [SerializeField] private Vector3 _openAngle;
    private Quaternion _targetRotation;
    [SerializeField] private float _rotationSpeed = 50f;
    private bool _openDoors;

    private void Start()
    {
        _targetRotation = Quaternion.Euler(_openAngle);
    }
    void Update()
    {
        if (_openDoors)
        {
            foreach (Transform t in _doors)
            {
                t.rotation = Quaternion.Slerp(t.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            _openDoors = true;
            AudioManager.Instance.PlaySFX(_finish);
            player.CompleteLevel();
            _gameCanvas.WinOrLose(true);
        }
    }
}
