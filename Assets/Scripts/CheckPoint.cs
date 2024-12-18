using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform[] _flags;
    [SerializeField] private Vector3 _openAngle;
    private Quaternion _targetRotation;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private AudioClip _flagSound;
    private bool _openFlags;

    private void Start()
    {
        _targetRotation = Quaternion.Euler(_openAngle);
    }
    void Update()
    {
        if (_openFlags)
        {
            foreach (Transform t in _flags)
            {
                t.rotation = Quaternion.Slerp(t.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            _openFlags = true;
            if (_flagSound != null)
            {
                AudioManager.Instance.PlaySFX(_flagSound);
            }
        }
    }
}
