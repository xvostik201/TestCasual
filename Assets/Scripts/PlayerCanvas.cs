using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Image _fillImage;

    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private string[] _statusString;

    [SerializeField] private float _autoSizeMin;
    [SerializeField] private float _autoSizeMax;
    [SerializeField] private float _turnDuration;

    [SerializeField] private TMP_Text _coinText;

    [SerializeField] private Color[] _statusColors;
    private Player _player;

    private int previewIndex;
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        SetUI();
        if (_player != null)
        {
            _player.onCollectItem += UpdateProgress;
        }
    }

    private void UpdateProgress(int currentCount, int currentIndex)
    {
        _fillImage.color = _statusColors[currentIndex];
        _statusText.color = _statusColors[currentIndex];
        _statusText.text = _statusString[currentIndex].ToString();
        _progressSlider.value = currentCount;
        _coinText.text = currentCount.ToString();
        if (currentIndex != previewIndex)
        {
            StartCoroutine(TextAnim());
            previewIndex = currentIndex;
        }
    }

    private void SetUI()
    {
        _progressSlider.value = 0;
        _fillImage.color = _statusColors[0];
        _statusText.color = _statusColors[0];
        _statusText.text = _statusString[0].ToString();
        _coinText.text = "0";
    }

    private IEnumerator TextAnim()
    {
        float elapsedTime = 0;
        float initialSize = _autoSizeMax;

        while (elapsedTime < _turnDuration)
        {
            _statusText.fontSizeMax = Mathf.Lerp(initialSize, _autoSizeMin, elapsedTime / _turnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _statusText.fontSizeMax = _autoSizeMin;
    }

}
