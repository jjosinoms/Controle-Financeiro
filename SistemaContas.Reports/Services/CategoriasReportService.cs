using OfficeOpenXml;
using SistemaContas.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaContas.Reports.Services
{
    public class CategoriasReportService
    {
        public byte[] GerarRelatorio(List<Categoria> categorias)
        {

            //define o tipo de licença para criação do arquivo excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //criando o conteudo do arquivo excel
            using (var excelPackage = new ExcelPackage())
            {
                //nome da planilha
                var sheet = excelPackage.Workbook.Worksheets.Add("Categorias");

                //escrevendo nas células:
                sheet.Cells["A1"].Value = "Relatório de categorias";

                sheet.Cells["A3"].Value = "Id";
                sheet.Cells["B3"].Value = "Nome da Categoria";

                //imprimindo as categorias
                var linha = 4;

                foreach (var item in categorias)
                {
                    sheet.Cells[$"A{linha}"].Value = item.Id.ToString();
                    sheet.Cells[$"B{linha}"].Value = item.Nome;

                    linha++;
                }

                //formatando as celulas da planilha
                sheet.Cells["A:B"].AutoFitColumns();

                //retornando o arquivo excel em memória
                return excelPackage.GetAsByteArray();

            }
        }
    }
}
