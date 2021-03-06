﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    /// <summary>
    /// 射线检测的起点
    /// </summary>
    public Transform rayDown, rayLeft, rayRight;
    /// <summary>
    /// 射线检测的层级
    /// </summary>
    public LayerMask platformLayer, obstacleLayer;


    /// <summary>
    /// 是否向左跳动
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// 是否正在跳跃
    /// </summary>
    private bool isJumping = false;
    /// <summary>
    /// 是否开始移动
    /// </summary>
    private bool isStartMove = false;
    private Rigidbody2D myBody;
    private SpriteRenderer SpriteRenderer;
    

    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVas vars;
    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVas.GetManagerVas();
    }
    private void Update()
    {
        Debug.DrawRay(rayDown.position, Vector2.down * 1f, Color.red);
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);
        //是否点击的是ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //游戏未开始或已结束或暂停
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsGameOver || GameManager.Instance.IsGamePause)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isJumping)
        {
            if (!isStartMove)
            {
                isStartMove = true;
                EventCenter.Broadcast(EventDefine.PlayerStartMove);
            }
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
        //游戏结束
        if (myBody.velocity.y < 0 && !IsRayPlatform() && !GameManager.Instance.IsGameOver)//y方向速度小于0，代表往下落,并且没有检测到平台，游戏没有结束
        {
            SpriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
        }
        if (isJumping && IsRayObstacle() && !GameManager.Instance.IsGameOver)//正在跳跃，检测到障碍物，游戏没有结束
        {
            GameManager.Instance.IsGameOver = true;
            Destroy(gameObject);
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.transform.position = transform.position;
            go.SetActive(true);
        }
        if (transform.position.y - Camera.main.transform.position.y < -6 && !GameManager.Instance.IsGameOver)
        {
            gameObject.SetActive(false);
            GameManager.Instance.IsGameOver = true;
        }
    }
    /// <summary>
    /// 上一次碰到的平台
    /// </summary>
    private GameObject lastHitGO = null;
    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Platform")
            {
                if(hit.collider.gameObject != lastHitGO)
                {
                    if (lastHitGO == null)
                    {
                        lastHitGO = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGO = hit.collider.gameObject;
                }
                
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);
        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        return false;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //吃到钻石
        if (collision.collider.tag == "PickUp")
        {
            EventCenter.Broadcast(EventDefine.AddDiamond);
            collision.gameObject.SetActive(false);
        }
    }
}
