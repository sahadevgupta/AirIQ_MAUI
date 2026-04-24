using AirIQ.Controls.StepBar;
using AirIQ.ViewModels.Common;

namespace AirIQ.Controls;

public partial class StepBarComponentView : ContentView
{
	public StepBarComponentView()
	{
		InitializeComponent();

	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (BindingContext is ViewModelBase vm)
		{
			BindingContext = vm;
			collectionView.ItemTemplate = new DataTemplate(() => new StepBarViewCell(vm));
		}
	}
}