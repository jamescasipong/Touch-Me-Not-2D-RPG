using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Character : MonoBehaviour
{
    private CharacterAnimator animator;
    public float moveSpeed;

    public bool IsMoving { get; private set; }
    
    // The offsets for aligning to the center of the tile
    public float xOffset = 0.5f;
    public float yOffset = 0.8f;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        // Calculate the snapped position with offsets
        pos.x = Mathf.Floor(pos.x) + xOffset;
        pos.y = Mathf.Floor(pos.y) + yOffset;

        transform.position = pos;
    }

    /*public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        // Calculate the snapped target position with offsets
        targetPos.x = Mathf.Floor(targetPos.x) + xOffset;
        targetPos.y = Mathf.Floor(targetPos.y) + yOffset;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;

        OnMoveOver?.Invoke();
    }*/

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        if (Mathf.Abs(moveVec.x) > Mathf.Abs(moveVec.y))
        {
            moveVec.y = 0;
        }
        else
        {
            moveVec.x = 0;
        }

        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position + new Vector3(moveVec.x, moveVec.y, 0);

        // Calculate the snapped target position with offsets for x and y
        targetPos.x = Mathf.Floor(targetPos.x) + xOffset;
        targetPos.y = Mathf.Floor(targetPos.y) + yOffset;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;

        OnMoveOver?.Invoke();
    }


    public void HandleUpdate()
    {
        if (animator != null) {
        animator.IsMoving = IsMoving;
        }
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;

        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
            return false;

        return true;    
    }
    private bool IsWalkable(Vector3 targetPos3)
    {
        if (Physics2D.OverlapCircle(targetPos3, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }
        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        // Check if the animator is not null before using it
        if (animator != null)
        {
            var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
            var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

            if (xdiff == 0 || ydiff == 0)
            {
                animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
                animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
            }
            else
            {
                Debug.LogError("xd");
            }
        }
    }


    public CharacterAnimator Animator
    {
        get => animator;
    }
}
