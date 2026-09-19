using AirIQ.Models;

using ClosedXML.Excel;

namespace AirIQ.Helpers;

public static class RefundRecordExcelExporter
{
    private static readonly string[] Headers =
    {
        "Return ID", "Prefix", "PNR", "Entry Date", "Travel Date & Time", "Destination",
        "Qty", "Cancellation Charge", "Refund Amount"
    };

    public static byte[] Export(IReadOnlyCollection<RefundRecord> records)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Refunds");

        for (var i = 0; i < Headers.Length; i++)
            worksheet.Cell(1, i + 1).Value = Headers[i];

        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Font.FontColor = XLColor.White;
        headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F3864");
        headerRow.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        var row = 2;
        foreach (var record in records)
        {
            worksheet.Cell(row, 1).Value = record.ReturnId;
            worksheet.Cell(row, 2).Value = record.Prefix;
            worksheet.Cell(row, 3).Value = record.PNR;

            if (record.EntryDate.HasValue)
            {
                var entryDateCell = worksheet.Cell(row, 4);
                entryDateCell.Value = record.EntryDate.Value;
                entryDateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";
            }

            var travelDateCell = worksheet.Cell(row, 5);
            travelDateCell.Value = record.TravelDateTime;
            travelDateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";

            worksheet.Cell(row, 6).Value = record.FDestName;
            worksheet.Cell(row, 7).Value = record.Qty;

            if (record.CancelChrg.HasValue)
            {
                var cancelChargeCell = worksheet.Cell(row, 8);
                cancelChargeCell.Value = record.CancelChrg.Value;
                cancelChargeCell.Style.NumberFormat.Format = "₹ #,##0.00";
            }

            if (record.RefundAmount.HasValue)
            {
                var refundAmountCell = worksheet.Cell(row, 9);
                refundAmountCell.Value = record.RefundAmount.Value;
                refundAmountCell.Style.NumberFormat.Format = "₹ #,##0.00";
            }

            row++;
        }

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 14;
        worksheet.Column(3).Width = 12;
        worksheet.Column(4).Width = 20;
        worksheet.Column(5).Width = 20;
        worksheet.Column(6).Width = 24;
        worksheet.Column(7).Width = 8;
        worksheet.Column(8).Width = 16;
        worksheet.Column(9).Width = 16;

        worksheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
