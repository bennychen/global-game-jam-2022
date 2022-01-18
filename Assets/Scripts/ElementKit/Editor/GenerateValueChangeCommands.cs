using System.IO;
using UnityEditor;

public class GenerateValueChangeCommands : Editor
{
	[MenuItem("Codeplay/Scene Element/Generate Value Change Commands")]
	public static void Exec()
	{
		using (var sw = new StreamWriter(ClassFilePath))
		{
			sw.NewLine = "\r\n";
			sw.WriteLine("// This class is auto-generated do not modify");
			sw.WriteLine("using System;");
			sw.WriteLine("using UnityEngine;");
			sw.WriteLine();
			sw.WriteLine("namespace " + Namespace);
			sw.WriteLine("{");
			sw.WriteLine(string.Format(ClassContent, "Value", "<TParam>", "TParam", CheckSerializable, CompareGeneric));
			sw.WriteLine(string.Format(ClassContent, "Int", "", "int", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "Float", "", "float", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "Bool", "", "bool", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "String", "", "string", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "Vector2", "", "Vector2", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "Vector3", "", "Vector3", "", CompareNormal));
			sw.WriteLine(string.Format(ClassContent, "Quaternion", "", "Quaternion", "", CompareNormal));
			sw.WriteLine("}");
		}
		AssetDatabase.ImportAsset(ClassFilePath, ImportAssetOptions.ForceUpdate);
	}

	private const string Namespace = "Codeplay";
	private const string Indent = "    ";
	private const string ClassFilePath = "Assets/ElementKit/Scripts/CommandValueChange.cs";

	// {0} Prefix
	// {1} generic
	// {2} type
	// {3} check serializable
	// {4} compare statement
	private const string ClassContent =
	@"    
    public class {0}ChangeCommand{1} : ICommand
    {{
        public string ID {{ get {{ return _id; }} }}
        public {2} Value {{ get {{ return _value; }} }}

        public {0}ChangeCommand(ElementModel model, string id)
        {{{3}
            _id = string.Format(""{{0}}/{{1}}"", model.GetType(), id);
            SceneElementConfig.CommandHistory.Regiser(this);
        }}

        public void Dispatch(string param)
        {{
            ForceExec(JsonHelper.FromJson<{2}>(param));
        }}

        public void Exec({2} value = default({2}))
        {{
            if ({4})
            {{
#if UNITY_EDITOR
                if (SceneElementConfig.CommandHistory.IsInHistory())
                {{
                    return;
                }}
                SceneElementConfig.CommandHistory.Push(_id, value);
#endif
                ForceExec(value);
            }}
        }}

        public void ForceExec({2} value = default({2}))
        {{
            var oldValue = _value;
            _value = value;
            OnValueChange(oldValue, _value);
        }}

        private {2} _value;
        private readonly string _id;
        private event Action<{2}, {2}> OnValueChange = delegate {{ }};
    }}";

	private const string CheckSerializable = @"
            if (!typeof(TParam).IsSerializable)
            {
                throw new InvalidOperationException(""A serializable type is required"");
            }";
	private const string CompareGeneric =
			//@"!System.Collections.Generic.EqualityComparer<TParam>.Default.Equals(value, _value)";
			@"!value.Equals(_value)";
	private const string CompareNormal =
			@"_value != value";
}
