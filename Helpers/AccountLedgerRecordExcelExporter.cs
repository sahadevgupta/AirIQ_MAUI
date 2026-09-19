using AirIQ.Models;

using ClosedXML.Excel;

namespace AirIQ.Helpers;

public static class AccountLedgerRecordExcelExporter
{
    private static readonly string[] Headers =
    {
        "Ref No", "Date", "Particulars", "Destination", "Travel Date", "Amount", "Balance"
    };

    public static byte[] Export(IReadOnlyCollection<AccountLedgerRecord> records)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Account Ledger");

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
            worksheet.Cell(row, 1).Value = record.RefNo;

            if (record.Date.HasValue)
            {
                var dateCell = worksheet.Cell(row, 2);
                dateCell.Value = record.Date.Value;
                dateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";
            }

            worksheet.Cell(row, 3).Value = record.Particulars;
            worksheet.Cell(row, 4).Value = record.Destination;

            var travelDateCell = worksheet.Cell(row, 5);
            if (record.TravelDate.HasValue)
            {
                travelDateCell.Value = record.TravelDate.Value;
                travelDateCell.Style.DateFormat.Format = "dd MMM yyyy HH:mm";
            }

            if (record.Amount.HasValue)
            {
                var amountCell = worksheet.Cell(row, 6);
                amountCell.Value = record.Amount.Value;
                amountCell.Style.NumberFormat.Format = "₹ #,##0.00";
            }

            if (record.Balance.HasValue)
            {
                var balanceCell = worksheet.Cell(row, 7);
                balanceCell.Value = record.Balance.Value;
                balanceCell.Style.NumberFormat.Format = "₹ #,##0.00";
            }

            row++;
        }

        worksheet.Column(1).Width = 14;
        worksheet.Column(2).Width = 20;
        worksheet.Column(3).Width = 28;
        worksheet.Column(4).Width = 24;
        worksheet.Column(5).Width = 20;
        worksheet.Column(6).Width = 14;
        worksheet.Column(7).Width = 14;

        worksheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
