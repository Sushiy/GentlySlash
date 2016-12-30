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

    Image m_imageSelectionFirst;    //UIElement for the first SelectionRing
    Image m_imageSelectionSecond;   //UIElement for the second SelectionRing

    public Color m_colorSelected = Color.white; //Color of the selected ring
    public Color m_colorNormal = Color.white;   //Color of the other ring

    private void Start()
    {
        //Initialize the selectionrings through getChild (I know thats not very safe, but the prefab shouldn't change)
        m_imageSelectionFirst = m_imageFirst.transform.GetChild(0).GetComponent<Image>();
        m_imageSelectionSecond = m_imageSecond.transform.GetChild(0).GetComponent<Image>();

        //This Class Subscribes to an Inventories ActiveWeaponIndex and calls ShowInventorySprite() on being notified
        m_inventoryThis.m_iActiveWeapon
        .Do(m_iActiveWeapon => ShowSelection(m_iActiveWeapon))
        .Subscribe(m_iActiveWeapon => ShowInventorySprite());
    }

    //This Updates Both InventorySlots with the respective Item held by the player
    private void ShowInventorySprite()
    {
        //set the inventory sprite if the slot isn't empty
        if (m_inventoryThis.Weapons[0] != null)
        {
            m_imageFirst.sprite = m_inventoryThis.Weapons[0].m_spriteUI;
        }
        if (m_inventoryThis.Weapons[1] != null)
        {
            m_imageSecond.sprite = m_inventoryThis.Weapons[1].m_spriteUI;
        }
    }

    //This Updates Both InventorySlots with the respective Item held by the player
    private void ShowSelection(int _index)
    {
        if (_index == 0)
        {
            m_imageFirst.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);  //Scale the selected slot to 1.0f
            m_imageSelectionFirst.color = m_colorSelected;                          //Set the selection rings color to selectioncolor
            m_imageSecond.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f); //Scale the other slot to 0.8f
            m_imageSelectionSecond.color = m_colorNormal;                           //Set the selection rings color to the normal color
        }
        else
        {
            m_imageSecond.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_imageSelectionSecond.color = m_colorSelected;
            m_imageFirst.rectTransform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            m_imageSelectionFirst.color = m_colorNormal;
        }
    }

}
