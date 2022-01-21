using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.UI;
using UnityEngine;

public class StickFsm : MonoBehaviour
{
    private PlayMakerFSM _fsm;
    private Vector3 _offset;
    private Transform _transformRef;

    private void Awake()
    {
        _fsm = GetComponent<PlayMakerFSM>();
        if (_fsm==null)
        {
            throw new Exception("play maker fsm does not exist on this game object...");
            return;
        }

        _fsm.Fsm.StateChanged += OnStateChanged;
        
    }
    
    void OnStateChanged(FsmState fsmState)
    {
        switch (fsmState.Name)
        {
            case "DroppedOut":
                DroppedOut();
                break;
            case "Idle":
                Idle();
                break;
            case "DroppedBack":
                DroppedBack();
                break;
            case "BeingDragged":
                BeingDragged();
                break;
            case "Dispose":
                Dispose();
                break;
            default:
                break;
        }
    }

    private void Dispose()
    {
        Destroy(gameObject);//todo use a pool instead.
    }

    private void BeingDragged()
    {
        _transformRef = transform;
        _offset = Input.mousePosition - _transformRef.position;
        _transformRef.localScale = new Vector3(0.7f, 0.7f, 1f);
    }

    private void DroppedBack()
    {
        throw new NotImplementedException();
    }

    private void Idle()
    {
        
    }

    private void DroppedOut()
    {
        throw new NotImplementedException();
    }

    public void OnDrag()
    {
        transform.position = Input.mousePosition - _offset;
        Debug.Log(
            $"mouse position is {Input.mousePosition}, offset is {_offset}, sprite position is {_transformRef.position}");
        
    }
}
