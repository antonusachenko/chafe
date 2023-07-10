using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Start()
    {
        GameManager.Instance.OnPlayerHPUpdated += GM_OnPlayerHPUpdated;
    }

    private void GM_OnPlayerHPUpdated(object sender, GameManager.OnPlayerHPUpdatedEventArgs e)
    {
        _image.fillAmount = e.hp / 100;
    }
}
