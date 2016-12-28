using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourTrans : MonoBehaviour
{
    Transform m_transThis;
    public new Transform transform
    {
        get
        {
            if (m_transThis == null)
            {
                m_transThis = base.transform;
            }
            return m_transThis;
        }
    }
}
