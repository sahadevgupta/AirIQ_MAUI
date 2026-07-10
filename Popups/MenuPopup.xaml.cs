using AirIQ.Helpers;
using AirIQ.ViewModels.Common;
using Mopups.Pages;

namespace AirIQ.Popups;

public partial class MenuPopup : PopupPage
{
	readonly MenuPageViewModel? viewModel;
	public MenuPopup()
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