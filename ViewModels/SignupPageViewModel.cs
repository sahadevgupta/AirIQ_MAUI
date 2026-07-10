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

    private static ContentView CreateStepContent(int index)
    {
        return index switch
        {
            0 => new PersonalInformationView(),
            1 => new ContactInformationView(),
            2 => new BusinessInformationView(),
            _ => new ContentView()
        };
    }

    private void EnsureStepContentLoaded(int index)
    {
        if (index < 0 || index >= Steps.Count)
        {
            return;
        }

        if (Steps[index].MainContent is null)
        {
            Steps[index].MainContent = CreateStepContent(index);
        }
    }

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
                IsCurrentContent=false
            },
            new StepBarModel()
            {
                StepName="Business",
                Status=StepBarStatus.Pending,
                IsNotLast=false,
                IsFirst=false,
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
        StepBarModel? currentStep = Steps.FirstOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index >= 0)
        {
            EnsureStepContentLoaded(index);

            ContentView? content = Steps[index].MainContent;
            bool isNotLast = Steps[index].IsNotLast;
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
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
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
                EnsureStepContentLoaded(index - 1);
                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);

                stepnext.Status = StepBarStatus.InProgress;

                EnsureStepContentLoaded(0);
                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }
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

            StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
            if (currentStep is null)
            {
                return;
            }

            int index = Steps.IndexOf(currentStep);
            if (index >= 0 && index < Steps.Count)
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
                    EnsureStepContentLoaded(index + 1);
                    stepnext.IsCurrentContent = true;
                    Steps[index + 1] = stepnext;
                }
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
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index >= 0 && index < Steps.Count)
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
                EnsureStepContentLoaded(index + 1);
                stepnext.IsCurrentContent = true;
            }
        }
    }

    [RelayCommand]
    private async Task BackStep()
    {
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
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
                EnsureStepContentLoaded(index - 1);
                stepnext.IsCurrentContent = true;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);
                stepnext.Status = StepBarStatus.InProgress;
                EnsureStepContentLoaded(0);
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
