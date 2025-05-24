using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarEnemie : MonoBehaviour
{
    public Image barLife;
    private IDamageable target;


    void Start()
    {
        target = GetComponentInParent<IDamageable>();
    }

    void Update()
    {
        if(target != null && target.MaxLife > 0)
        {
            barLife.fillAmount = target.CurrentLife / target.MaxLife;
        }
    }

    void LateUpdate()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        transform.rotation = Quaternion.identity;
    }


}
