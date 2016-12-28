using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InventoryView : MonoBehaviourTrans
{
    public int m_iSlotNumber = 0;
    Image m_imageThis;

    private string counter;

    private void Start()
    {
        m_imageThis = GetComponent<Image>();
        Inventory.s_instance.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => ShowInventorySprite());

    }

    private void ShowInventorySprite()
    {
        Debug.Log("ShowInventorySprite");
        if (Inventory.s_instance.m_weaponSlots[m_iSlotNumber] != null)
            m_imageThis.sprite = Inventory.s_instance.m_weaponSlots[m_iSlotNumber].m_spriteUI;
    }

}
