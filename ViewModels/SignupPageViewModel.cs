using System;
using System.Collections.ObjectModel;
using AirIQ.Controls;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views.ContentViews;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

public partial class SignupPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<StepBarModel> _steps = new();

    [ObservableProperty]
    private int _stepListCount;

    #endregion

    #region [ Methods & Service Calls ]

    private void InitializeData()
    {
        Steps = new ObservableCollection<StepBarModel>()
        {
            new StepBarModel()
            {
                StepName="Personal",
                Status=StepBarStatus.InProgress,
                IsNotLast=true,
                IsFirst=true,
                MainContent=new PersonalInformationView(),
                IsCurrentContent=true,
            },
            new StepBarModel()
            {
                StepName="Contact",
                Status=StepBarStatus.Pending,
                IsNotLast=true,
                IsFirst=false,
                MainContent=new ContactInformationView(),
                IsCurrentContent=false
            },
            new StepBarModel()
            {
                StepName="Business",
                Status=StepBarStatus.Pending,
                IsNotLast=false,
                IsFirst=false,
                MainContent=new BusinessInformationView(),
                IsCurrentContent=false
            }
        };
        StepListCount = Steps.Count;

        AddContentForSelectedStep();
    }

    #endregion

    #region StepBar Module

    public void AddContentForSelectedStep()
    {
        if (Steps.FirstOrDefault(x => x.IsCurrentContent) != null)
        {
            ContentView content = Steps.FirstOrDefault(x => x.IsCurrentContent).MainContent;
            bool isNotLast = Steps.FirstOrDefault(x => x.IsCurrentContent).IsNotLast;
            if (content != null)
            {
                // if (SubGrid.Children.Count >= 1)
                // {
                //     for (int i = 0; i < SubGrid.Children.Count; i++)
                //     {
                //         SubGrid.Children.RemoveAt(i);
                //     }
                // }

                // SubGrid.Children.Add(content, 0, 0);
            }
            //IsNotLastPage = isNotLast;
        }
    }

    public void NavigateToBackStep()
    {

        int index = Steps.IndexOf(Steps.LastOrDefault(x => x.IsCurrentContent));
        if (index > 0)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Pending;
            step.IsCurrentContent = false;
            Steps[index] = step;
            if ((index - 1) > 0)
            {
                StepBarModel stepnext = Steps.ElementAt(index - 1);
                if (stepnext.Status == StepBarStatus.Completed)
                {
                    stepnext.Status = StepBarStatus.InProgress;
                }
                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);

                stepnext.Status = StepBarStatus.InProgress;

                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }

            StepBarComponentView stepbar = new StepBarComponentView();
            // int indexforstepper = MainGrid.Children.IndexOf(MainGrid.Children.LastOrDefault(x => x.GetType() == typeof(StepBarComponentView)));
            // MainGrid.Children.RemoveAt(indexforstepper);
            // MainGrid.Children.Add(stepbar, 0, 0);
        }
        AddContentForSelectedStep();

    }

    public void NavigateToNextStep()
    {
        //if (ValidateFields())
        {


            // if (isCreditCard && string.IsNullOrEmpty(CardNumber))
            //     CardDesc = $"{AppResource.ResourceManager.GetString("NotificationViewExpires", AppResource.Culture)} {CardExpirationDate}";

            // ShippingAddressDesc = shippingAddress.FormatAddress;
            // BillingingAddressDesc = billingAddress?.FormatAddress;

            int index = Steps.IndexOf(Steps.LastOrDefault(x => x.IsCurrentContent));
            if (index < Steps.Count)
            {
                StepBarModel step = Steps.ElementAt(index);
                step.Status = StepBarStatus.Completed;
                step.IsCurrentContent = false;
                Steps[index] = step;
                if ((index + 1) < Steps.Count)
                {
                    StepBarModel stepnext = Steps.ElementAt(index + 1);
                    if (stepnext.Status == StepBarStatus.Pending)
                    {
                        stepnext.Status = StepBarStatus.InProgress;
                    }
                    stepnext.IsCurrentContent = true;
                    Steps[index + 1] = stepnext;
                }

                StepBarComponentView stepbar = new StepBarComponentView();
                // int indexforstepper = MainGrid.Children.IndexOf(MainGrid.Children.FirstOrDefault(x => x.GetType() == typeof(StepBarComponentView)));
                // MainGrid.Children.RemoveAt(indexforstepper);

                // MainGrid.Children.Add(stepbar, 0, 0);
            }
            AddContentForSelectedStep();
        }

    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task NextStep()
    {
        int index = Steps.IndexOf(Steps.LastOrDefault(x => x.IsCurrentContent)!);
        if (index < Steps.Count)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Completed;
            step.IsCurrentContent = false;

            if ((index + 1) < Steps.Count)
            {
                StepBarModel stepnext = Steps.ElementAt(index + 1);
                if (stepnext.Status == StepBarStatus.Pending)
                {
                    stepnext.Status = StepBarStatus.InProgress;
                }
                stepnext.IsCurrentContent = true;
            }
        }
    }

    [RelayCommand]
    private async Task BackStep()
    {
        int index = Steps.IndexOf(Steps.LastOrDefault(x => x.IsCurrentContent));
        if (index > 0)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Pending;
            step.IsCurrentContent = false;
            if ((index - 1) > 0)
            {
                StepBarModel stepnext = Steps.ElementAt(index - 1);
                if (stepnext.Status == StepBarStatus.Completed)
                {
                    stepnext.Status = StepBarStatus.InProgress;
                }
                stepnext.IsCurrentContent = true;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);
                stepnext.Status = StepBarStatus.InProgress;
                stepnext.IsCurrentContent = true;
            }
        }
    }

    #endregion

    #region [ Override Methods ]

    public override Task LoadDataWhenNavigatedTo()
    {
        InitializeData();
        return base.LoadDataWhenNavigatedTo();
    }

    #endregion

}
