using System;

namespace Codeplay
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class InjectElementAttribute : Attribute
	{
		public InjectElementAttribute() { }

		public InjectElementAttribute(string name)
		{
			Name = name;
			IsChild = false;
		}

		public InjectElementAttribute(bool isChild)
		{
			IsChild = isChild;
		}

		public InjectElementAttribute(string name, bool isChild)
		{
			Name = name;
			IsChild = isChild;
		}

		public string Name { get; private set; }
		public bool IsChild { get; private set; }
	}
}
