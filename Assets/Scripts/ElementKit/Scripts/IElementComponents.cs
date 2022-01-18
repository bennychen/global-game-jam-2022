namespace Codeplay
{
	public interface IElementComponents
	{
		bool IsInited { get; }
		void Add(IElementComponent component);
		void Remove(IElementComponent component);
		T Get<T>() where T : class, IElementComponent;
	}
}
