using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
    /// <summary>
    /// 是否向左跳动
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// 是否正在跳跃
    /// </summary>
    private bool isJumping = false;

    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVas vars;
    private void Awake()
    {
        vars = ManagerVas.GetManagerVas();
    }
    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsGameOver)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isJumping)
        {
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mouseClickPos = Input.mousePosition;
            //点击屏幕左边
            if (mouseClickPos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }
            else
            {
                isMoveLeft = false;
            }
            Jump();
        }
    }

    private void Jump()
    {
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
        }
        else
        {
            transform.localScale = Vector3.one;
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 curPlatformPos = collision.transform.position;
            nextPlatformLeft = new Vector3(curPlatformPos.x - vars.nextXPos, curPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(curPlatformPos.x + vars.nextXPos, curPlatformPos.y + vars.nextYPos, 0);
        }
    }
}
