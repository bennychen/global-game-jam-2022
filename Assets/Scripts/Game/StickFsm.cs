using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using HutongGames.PlayMaker;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.UI;
using UnityEngine;

namespace Game
{
    public enum Judgement
    {
        Hell,
        Heaven,
    }

    public class StickFsm : MonoBehaviour
    {
        private PlayMakerFSM _fsm;
        private Vector3 _offset;
        private Transform _transformRef;
        private float _seaLevelY;
        private Vector3 _oriPos;
        private Vector3 _oriScale;

        public Judgement Judgement;
        private void Awake()
        {
            _fsm = GetComponent<PlayMakerFSM>();
            var transformRef = transform;
            _oriPos = transformRef.position;
            _oriScale = transformRef.localScale;
            if (_fsm==null)
            {
                throw new Exception("play maker fsm does not exist on this game object...");
                return;
            }

            _fsm.Fsm.StateChanged += OnStateChanged;
            _seaLevelY = SeaLevel.Instance.transform.position.y;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _fsm.SendEvent("DrawEnable");
                _fsm.SendEvent("DrawDisable");
                _fsm.SendEvent("Reset");
            }
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
                case "Dropping":
                    Dropping();
                    break;
                case "Available":
                    Available();
                    break;
                default:
                    break;
            }
        }

        private void Available()
        {
            GetComponent<SpriteRenderer>().DOColor(Color.white, 0.2f);
        }

        private void Dropping()
        {
            if (transform.position.y >= _seaLevelY)
            {
                _fsm.SendEvent("ReleaseOut");
            }
            else
            {
                _fsm.SendEvent("ReleaseBack");
            }
        }

        private void Dispose()
        {
            gameObject.transform.position = _oriPos; //todo use a pool instead.
        }

        private void BeingDragged()
        {
            _transformRef = transform;
            var position = _transformRef.position;
            System.Diagnostics.Debug.Assert(Camera.main != null, "Camera.main != null");
            _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - position;
            _transformRef.DOScale(_oriScale*.7f, 0.3f);
        }

        private void DroppedBack()
        {
            _transformRef.DOScale(_oriScale, 0.3f);
            _transformRef.DOMove(_oriPos, 0.7f).OnComplete(() =>
            {
                _fsm.SendEvent("TweenFinish");
            });
        }

        private void Idle()
        {
            _transformRef.DOScale(_oriScale, 0.2f);
            GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.2f);
        }

        private void DroppedOut()
        {
            _transformRef.DOScale(Vector3.zero, 0.7f).OnComplete(() =>
            {
                _fsm.SendEvent("TweenFinish");
            });
        }

        public void OnDrag()
        {
            System.Diagnostics.Debug.Assert(Camera.main != null, "Camera.main != null");
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset;
            // Debug.Log(
            // $"mouse position is {Input.mousePosition}, offset is {_offset}, sprite position is {_transformRef.position}");
        }
    }
}