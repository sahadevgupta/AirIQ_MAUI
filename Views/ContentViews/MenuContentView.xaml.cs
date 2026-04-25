using AirIQ.Helpers;
using AirIQ.ViewModels.Common;

namespace AirIQ.Views.ContentViews;

public partial class MenuContentView : ContentView
{
	private readonly MenuPageViewModel viewModel;
	public MenuContentView()
	{
		InitializeComponent();
		viewModel = ServiceHelper.GetService<MenuPageViewModel>();
		BindingContext = viewModel;
	}

	protected override void OnParentSet()
	{
		base.OnParentSet();
		if (Parent != null && viewModel != null)
		{
			viewModel.LoadDataWhenNavigatedTo();
		}
	}
}