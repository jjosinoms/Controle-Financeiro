using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using OfficeOpenXml;
using SistemaContas.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaContas.Reports.Services
{
    public class ContasReportService
    {
        public byte[] GerarRelatorioExcel(List<Conta> contas)
        {

            //define o tipo de licença para criação do arquivo excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //criando o conteudo do arquivo excel
            using (var excelPackage = new ExcelPackage())
            {
                //nome da planilha
                var sheet = excelPackage.Workbook.Worksheets.Add("Contas");

                //escrevendo nas células:
                sheet.Cells["A1"].Value = "Relatório de contas";

                sheet.Cells["A3"].Value = "Id";
                sheet.Cells["B3"].Value = "Nome";
                sheet.Cells["C3"].Value = "Data";
                sheet.Cells["D3"].Value = "Valor";
                sheet.Cells["E3"].Value = "Tipo";
                sheet.Cells["F3"].Value = "Categoria";
                sheet.Cells["G3"].Value = "Observações";


                //imprimindo as categorias
                var linha = 4;

                foreach (var item in contas)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Id.ToString();
                    sheet.Cells[$"B{linha}"].Value = item.Nome;
                    sheet.Cells[$"C{linha}"].Value = item.Data.ToString("dd-MM-yyyy");
                    sheet.Cells[$"D{linha}"].Value = "R$ " + item.Valor;
                    sheet.Cells[$"E{linha}"].Value = item.Tipo;
                    sheet.Cells[$"F{linha}"].Value = item.Categoria?.Nome;
                    sheet.Cells[$"G{linha}"].Value = item.Observacoes;

                    linha++;
                }

                //formatando as celulas da planilha
                sheet.Cells["A:G"].AutoFitColumns();

                //retornando o arquivo excel em memória
                return excelPackage.GetAsByteArray();

            }
        }

        public byte[] GerarRelatorioPdf(List<Conta> contas)
        {
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatório de Contas\n"));
                document.Add(new Paragraph($"Data: {DateTime.Now.ToString("dd/MM/yyyy")}\n\n"));

                foreach (var item in contas)
                {
                    document.Add(new Paragraph($"ID: {item.Id}"));
                    document.Add(new Paragraph($"Nome da Conta: {item.Nome}"));
                    document.Add(new Paragraph($"Data da Conta: {item.Data.ToString("dd/MM/yyyy")}"));
                    document.Add(new Paragraph($"Valor: {item.Valor}"));
                    document.Add(new Paragraph($"Tipo: {item.Tipo.ToString()}"));
                    document.Add(new Paragraph($"Categoria:"));
                    document.Add(new Paragraph($"Observações: {item.Observacoes}"));
                    document.Add(new Paragraph("\n\n"));
                }
            }

            return memoryStream.ToArray();
        }
    }
}

