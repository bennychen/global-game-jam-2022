using Codeplay;
using UnityEngine;
using UnityEngine.Video;

namespace Game
{
    public class GuidState : State<GameController>
    {
        private VideoPlayer _videoPlayer;
        private float _accuTime =0f;

        public override void OnEnter()
        {
            base.OnEnter();
            var contextGameGuideScene = _context.GameGuideScene;
            contextGameGuideScene.SetActive(true);
            _videoPlayer = contextGameGuideScene.GetComponentInChildren<VideoPlayer>();
            _videoPlayer.Play();
            // VideoPlayer.TimeEventHandler += JudgeTimeFinish;

        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _accuTime += deltaTime;
            // Debug.Log(_accuTime);
            if ((_accuTime>47f))
            // if ((_accuTime>55f) && (!_videoPlayer.isPlaying))
            {
                _stateMachine.ChangeState<GameLoopState>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameGuideScene.SetActive(false);
        }
    }
}