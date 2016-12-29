using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InventoryView : MonoBehaviourTrans
{
    public Image m_imageFirst;
    public Image m_imageSecond;

    private string counter;

    private void Start()
    {
        Inventory.s_instance.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => ShowInventorySprite());
    }

    private void ShowInventorySprite()
    {
        Debug.Log("ShowInventorySprite");
        if (Inventory.s_instance.m_weaponSlots[0] != null)
            m_imageFirst.sprite = Inventory.s_instance.m_weaponSlots[0].m_spriteUI;
        if (Inventory.s_instance.m_weaponSlots[1] != null)
            m_imageSecond.sprite = Inventory.s_instance.m_weaponSlots[1].m_spriteUI;
    }

}
