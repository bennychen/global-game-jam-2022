using UnityEngine;

namespace Codeplay
{
	public abstract class SceneElementBase : MonoBehaviour
	{
		public string ID
		{
			get
			{
				if (string.IsNullOrEmpty(_id))
				{
					_id = name;
				}
				return _id;
			}
		}

		public bool IsRootElement { get { return Parent == null; } }
		public SceneElementBase Parent { get { return _parent; } }

		public abstract void Init(object data);
		public virtual void Free() { }

		[SerializeField]
		[HideInInspector]
		protected SceneElementBase _parent;

		private string _id;
	}
}