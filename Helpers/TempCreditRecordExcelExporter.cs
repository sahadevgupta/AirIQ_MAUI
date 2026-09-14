using AirIQ.Models;

using ClosedXML.Excel;

namespace AirIQ.Helpers;

public static class TempCreditRecordExcelExporter
{
    private static readonly string[] Headers =
    {
        "Credit ID", "Date", "Name", "Amount"
    };

    public static byte[] Export(IReadOnlyCollection<TempCreditRecord> records)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Temp Credit");

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
            worksheet.Cell(row, 1).Value = record.CreditId;

            var dateCell = worksheet.Cell(row, 2);
            dateCell.Value = record.Date;
            dateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";

            worksheet.Cell(row, 3).Value = record.Name;

            var amountCell = worksheet.Cell(row, 4);
            amountCell.Value = record.Amount;
            amountCell.Style.NumberFormat.Format = "₹ #,##0.00";

            row++;
        }

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 20;
        worksheet.Column(3).Width = 28;
        worksheet.Column(4).Width = 14;

        worksheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
