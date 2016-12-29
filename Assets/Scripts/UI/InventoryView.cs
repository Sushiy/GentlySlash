using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class InventoryView : MonoBehaviourTrans
{
    public Inventory m_inventoryThis;   //Inventory to look at

    public Image m_imageFirst;  //UIElement for the first Weapon
    public Image m_imageSecond; //UIElement for the second Weapon

    private void Start()
    {
        //This Class Subscribes to an Inventories ActiveWeaponIndex and calls ShowInventorySprite() on being notified
        m_inventoryThis.m_iActiveWeapon
        .Subscribe(m_iActiveWeapon => ShowInventorySprite());
    }

    //This Updates Both InventorySlots with the respective Item held by the player
    private void ShowInventorySprite()
    {
        if (m_inventoryThis.Weapons[0] != null)
            m_imageFirst.sprite = m_inventoryThis.Weapons[0].m_spriteUI;
        if (m_inventoryThis.Weapons[1] != null)
            m_imageSecond.sprite = m_inventoryThis.Weapons[1].m_spriteUI;
    }

}
