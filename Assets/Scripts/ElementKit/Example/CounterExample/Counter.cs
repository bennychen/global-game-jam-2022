using UnityEngine;

public class CounterView : Codeplay.ElementBehavior<Counter>
{
	public void OnGUI()
	{
		GUILayout.Label("Current count = " + _counterModel.Count);

		if (GUILayout.Button("Plus One"))
		{
			_counterModel.PlusOneCommand.Exec();
		}
		if (GUILayout.Button("Minus One"))
		{
			_counterModel.MinusOneCommand.Exec();
		}
		if (GUILayout.Button("Plus 10"))
		{
			_counterModel.IncreaseCommand.Exec(10);
		}
		if (GUILayout.Button("Minus 10"))
		{
			_counterModel.DescreaseCommand.Exec(10);
		}
	}

	protected override void OnInit()
	{
		_counterModel = _context.GetModel<CounterModel>();
	}

	private CounterModel _counterModel;
}

public class CounterModel : Codeplay.ElementModel
{
	public int Count { get; private set; }

	public Codeplay.Command PlusOneCommand { get; private set; }
	public Codeplay.Command MinusOneCommand { get; private set; }
	public Codeplay.Command<int> IncreaseCommand { get; private set; }
	public Codeplay.Command<int> DescreaseCommand { get; private set; }

	public CounterModel()
	{
		PlusOneCommand = new Codeplay.Command(this, PlusOne);
		MinusOneCommand = new Codeplay.Command(this, MinusOne);
		IncreaseCommand = new Codeplay.Command<int>(this, Increase);
		DescreaseCommand = new Codeplay.Command<int>(this, Decrease);
	}

	private void PlusOne()
	{
		Count++;
	}

	private void MinusOne()
	{
		Count--;
	}

	private void Increase(int value)
	{
		Count += value;
	}

	private void Decrease(int value)
	{
		Count -= value;
	}
}

public class Counter : Codeplay.SceneElement<Counter>
{
	protected override void OnInit(object data)
	{
		Models.Add(new CounterModel());

		Views.Add(_viewsNode.gameObject.AddComponent<CounterView>());
	}
}
