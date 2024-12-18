using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Image[] _winLosePanelImage;
    [SerializeField] private TMP_Text _winLosePaneltext;

    [SerializeField] private Button _panelButton;
    void Start()
    {
        _levelText.text = "Уровень " + SceneManager.GetActiveScene().buildIndex.ToString();

    }

    private void LevelChanger(int index)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
    }

    public void WinOrLose(bool win)
    {
        _panel.SetActive(true);
        if (win)
        {
            _winLosePaneltext.text = "Вы победили!";
            _panelButton.GetComponentInChildren<TMP_Text>().text = "Следующий уровень";
            _panelButton.onClick.AddListener(() => LevelChanger(0));
            foreach (Image image in _winLosePanelImage)
            {
                image.color = new Color(0, 1, 0, image.color.a);
            }
        }
        else
        {
            _winLosePaneltext.text = "Вы проиграли!";
            _panelButton.GetComponentInChildren<TMP_Text>().text = "Начать снова";
            _panelButton.onClick.AddListener(() => LevelChanger(0));
            foreach (Image image in _winLosePanelImage)
            {
                image.color = new Color(1, 0, 0, image.color.a);
            }
        }
    }

}
