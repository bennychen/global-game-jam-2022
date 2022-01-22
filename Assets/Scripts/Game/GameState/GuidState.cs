
using Codeplay;
using UnityEngine.Video;

namespace Game
{
    public class GuidState : State<GameController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var contextGameGuideScene = _context.GameGuideScene;
            contextGameGuideScene.SetActive(true);
            var videoPlayer = contextGameGuideScene.GetComponentInChildren<VideoPlayer>();
            videoPlayer.Play();
            // videoPlayer.
            
        }

        public override void OnExit()
        {
            base.OnExit();
            _context.GameGuideScene.SetActive(false);
        }
    }
}