using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public enum collisionType { head, body, arms}
    public collisionType damageType;

    public AiScript controller;

    public void Hit(int value)
    {
        switch (damageType)
        {
            case collisionType.head:
                controller.setHealth(value);
                break;
            case collisionType.body:
                controller.setHealth(value / 2);
                break;
            case collisionType.arms:
                controller.setHealth(value / 4);
                break;
        }
        controller.setHealth(value);
    }
}
