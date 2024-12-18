using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorGameObject;
    [SerializeField] private Player _player;


    private bool _isTutorCompelete;

    private void Awake()
    {
        _tutorGameObject.SetActive(!_isTutorCompelete);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_isTutorCompelete)
        {
            _isTutorCompelete = true;
            _tutorGameObject.SetActive(!_isTutorCompelete);
            _player.StartGame();
        }
    }

}
