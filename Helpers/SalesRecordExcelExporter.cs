using AirIQ.Models;

using ClosedXML.Excel;

namespace AirIQ.Helpers;

public static class SalesRecordExcelExporter
{
    private static readonly string[] Headers =
    {
        "Sale ID", "Prefix", "PNR", "Passenger(s)", "Entry Date", "Travel Date & Time",
        "Destination", "Airline", "Qty", "Price", "Amount"
    };

    public static byte[] Export(IReadOnlyCollection<SalesRecord> salesRecords)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sales Records");

        for (var i = 0; i < Headers.Length; i++)
            worksheet.Cell(1, i + 1).Value = Headers[i];

        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Font.FontColor = XLColor.White;
        headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F3864");
        headerRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        var row = 2;
        foreach (var record in salesRecords)
        {
            worksheet.Cell(row, 1).Value = record.SaleID;
            worksheet.Cell(row, 2).Value = record.Prefix;
            worksheet.Cell(row, 3).Value = record.PNR;

            var passengerCell = worksheet.Cell(row, 4);
            passengerCell.Value = record.PassengersName;
            passengerCell.Style.Alignment.WrapText = true;

            var entryDateCell = worksheet.Cell(row, 5);
            entryDateCell.Value = record.EntryDate;
            entryDateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";

            var travelDateCell = worksheet.Cell(row, 6);
            travelDateCell.Value = record.TravelDateTime;
            travelDateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";

            worksheet.Cell(row, 7).Value = record.FDestName;
            worksheet.Cell(row, 8).Value = record.AirlineName;
            worksheet.Cell(row, 9).Value = record.PAX_Qty;

            var priceCell = worksheet.Cell(row, 10);
            priceCell.Value = record.FinalRate;
            priceCell.Style.NumberFormat.Format = "₹ #,##0.00";

            var amountCell = worksheet.Cell(row, 11);
            amountCell.Value = record.Amount;
            amountCell.Style.NumberFormat.Format = "₹ #,##0.00";

            row++;
        }

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 14;
        worksheet.Column(3).Width = 12;
        worksheet.Column(4).Width = 28;
        worksheet.Column(5).Width = 20;
        worksheet.Column(6).Width = 20;
        worksheet.Column(7).Width = 24;
        worksheet.Column(8).Width = 16;
        worksheet.Column(9).Width = 8;
        worksheet.Column(10).Width = 14;
        worksheet.Column(11).Width = 14;

        worksheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
