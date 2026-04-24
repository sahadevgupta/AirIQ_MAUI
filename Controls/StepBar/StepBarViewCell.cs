using System;
using AirIQ.Converter;
using AirIQ.Models;
using AirIQ.ViewModels.Common;
using Microsoft.Maui.Controls.Shapes;

namespace AirIQ.Controls.StepBar;

public class StepBarViewCell : ContentView
{
    private readonly ProgressBar progress;
    private readonly ViewModelBase viewModel;
    private readonly Label mainLabel;
    private readonly Grid mainGrid;
    private readonly Grid trackerGrid;

    public StepBarViewCell(ViewModelBase viewModel)
    {
        this.viewModel = viewModel;

        mainGrid = new Grid
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            ColumnSpacing = 0,
            Margin = 0,
            Padding = 0,
            RowDefinitions =
            {
                new RowDefinition{ Height = GridLength.Auto},
                new RowDefinition{ Height = GridLength.Auto}
            },
            ColumnDefinitions =
            {
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto)},
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto)},
            }
        };

        trackerGrid = new Grid
        {
            WidthRequest = 20,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            ColumnSpacing = 0,
            Margin = new Thickness(0, 10, 0, 0),
            Padding = 0,
            RowSpacing = 1,
            RowDefinitions =
            {
                new RowDefinition{ Height = GridLength.Auto}
            },
            ColumnDefinitions =
            {
                new ColumnDefinition {Width = new GridLength(1,GridUnitType.Auto)}
            }
        };

        Border tackerbg = new Border()
        {
            StrokeShape = new Ellipse(),
            HeightRequest = 20,
            WidthRequest = 20
        };

        tackerbg.SetBinding(Border.BackgroundColorProperty, "Status", converter: new StepColorConverter());

        trackerGrid.Children.Add(tackerbg);
        Grid.SetColumn(tackerbg, 0);
        Grid.SetRow(tackerbg, 0);

        Image img = new Image()
        {
            Aspect = Aspect.AspectFill,
            Margin = 4
        };
        img.SetBinding(Image.SourceProperty, "Status", converter: new StepColorConverter());

        mainLabel = new Label()
        {
            HorizontalTextAlignment = TextAlignment.Start,
            HorizontalOptions = LayoutOptions.Start,
            LineBreakMode = LineBreakMode.WordWrap
        };
        mainLabel.SetBinding(Label.TextProperty, "StepName");

        // mainGrid.Children.Add(mainLabel);
        // Grid.SetColumn(mainLabel, 0);
        // Grid.SetRow(mainLabel, 1);
        // Grid.SetColumnSpan(mainLabel, 2);

        progress = new ProgressBar()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Margin = new Thickness(0, 10, 0, 0),
            ProgressColor = Colors.Red
        };
        progress.SetBinding(ProgressBar.IsVisibleProperty, "IsNotLast");

        mainGrid.Children.Add(trackerGrid);
        Grid.SetColumn(mainLabel, 0);
        Grid.SetRow(mainLabel, 0);

        mainGrid.Children.Add(progress);

        Grid.SetColumn(progress, 1);
        Grid.SetRow(progress, 0);
        Content = mainGrid;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        double cellwidth = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) / 3;
        this.WidthRequest = cellwidth;
        progress.WidthRequest = cellwidth - 20;
        mainLabel.WidthRequest = cellwidth - 8 - 20;
        if (BindingContext is StepBarModel stepBarModel)
        {
            if (stepBarModel.IsNotLast)
            {
                trackerGrid.HorizontalOptions = LayoutOptions.Center;
            }
            else
            {
                trackerGrid.HorizontalOptions = LayoutOptions.End;
            }
        }
    }
}
