using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;

    [SerializeField] CanvasGroup pickupGunCanvasGroup;
    [SerializeField] CanvasGroup gunInfoCanvas;

    [Header("Gun UI")]
    Gun currentUIGun;
    [SerializeField] TextMeshProUGUI gunName;
    [SerializeField] TextMeshProUGUI gunAmmo;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(currentUIGun != null)
        {
            gunAmmo.text = $"{currentUIGun.currentAmmo} / {currentUIGun.maxAmmo}";
        }
    }

    public void SetPickupGun(bool b)
    {
        pickupGunCanvasGroup.DOKill();
        pickupGunCanvasGroup.DOFade(b ? 1f : 0f, 0.3f);
    }

    public void ShowGunInfoCanvas(Gun gun)
    {
        currentUIGun = gun;
        gunInfoCanvas.DOKill();
        gunInfoCanvas.DOFade(1f, 0.3f);
        gunName.text = currentUIGun.gunName;
    }

    public void HideGunInfoCanvas()
    {
        currentUIGun = null;
        gunInfoCanvas.DOKill();
        gunInfoCanvas.DOFade(0f, 0.3f);
    }
}