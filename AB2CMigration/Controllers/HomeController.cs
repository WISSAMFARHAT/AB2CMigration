using ConnectionB2C;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using OfficeOpenXml;
using System.Data;


namespace AB2CMigration.Controllers;

[Route("")]
public class HomeController : BaseController
{

    [Route("")]
    public async Task<IActionResult> Index(string? message,string color="red")
    {
        CloudPage();


        ViewBag.Message = message;
        ViewBag.color = color;

        return View();
    }
    

    [HttpPost]
    [Route("MigrationExport")]
    public async Task<IActionResult> MigrationExport(IFormFile file)
    {
        try
        {
            if (file is null)
                return RedirectToAction("Index", new { message = "Please put a file path" });

            List<UserModel> users = new();

            using (MemoryStream stream = new())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage pck = new(stream);

                ExcelWorksheet worksheet = pck.Workbook.Worksheets[0];

                users = ConvertToUsersModel(worksheet);
            }


            foreach (UserModel user in users)
                await GraphProvider.Cosmos.CreateUser(user);

            return RedirectToAction("Index", new { message = "Done",color="green" });
        }
        catch (Exception ex)
        {

            return RedirectToAction("Index", new { message = ex.Message });
        }
    }

    [HttpPost]
    [Route("MigrationGraph")]
    public async Task<IActionResult> MigrationGraph()
    {
        try
        {

            List<UserModel> users = await GraphProvider.Graph.GetAllUsers();


            foreach (UserModel user in users)
                await GraphProvider.Cosmos.CreateUser(user);

            return RedirectToAction("Index", new { message = "Done", color = "green" });

        }
        catch (Exception ex)
        {
            return RedirectToAction("Index", new { message = ex.Message });
        }


    }

    private static List<UserModel> ConvertToUsersModel(ExcelWorksheet worksheet)
    {
        List<UserModel> users = new();

        int indexDisplayName = 0;
        int indexFirstName = 0;
        int indexLastName = 0;
        int indexID = 0;
        int indexEmail = 0;

        for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
        {
            string test = worksheet.Cells[1, i].Value?.ToString().ToLower();
            switch (worksheet.Cells[1, i].Value?.ToString().ToLower())
            {
                case "displayname":
                    indexDisplayName = i;
                    break;
                case "surname":
                    indexFirstName = i;
                    break;
                case "givenname":
                    indexLastName = i;
                    break;
                case "objectid":
                    indexID = i;
                    break;
                case "alternateemailaddress":
                    indexEmail = i;
                    break;

            }
        }

        for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
            users.Add(new()
            {
                DisplayName = worksheet.Cells[i, indexDisplayName].Value?.ToString(),
                FirstName = worksheet.Cells[i, indexFirstName].Value?.ToString(),
                LastName = worksheet.Cells[i, indexLastName].Value?.ToString(),
                ID = worksheet.Cells[i, indexID].Value?.ToString(),
                Email = worksheet.Cells[i, indexEmail].Value?.ToString(),
            });

        return users;
    }
}


