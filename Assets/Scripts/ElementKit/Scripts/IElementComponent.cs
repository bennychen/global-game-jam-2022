namespace Codeplay
{
	public interface IElementComponent
	{
		bool IsInited { get; }
		void Init();
		void SetContext(SceneElementBase context, IElementComponents container);
		bool IsActive { get; }
		void Activate();
		void Deactivate();
	}
}
