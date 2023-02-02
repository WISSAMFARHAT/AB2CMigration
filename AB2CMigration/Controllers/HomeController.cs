using ConnectionB2C;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using OfficeOpenXml;
using System.Data;


namespace AB2CMigration.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("migration")]
        public async Task<IActionResult> Migration(IFormFile file)
        {
            
            try
            {

               // List<UserModel> model = await GraphProvider.Graph.GetAllUsers();

                
                if (file is null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                List<UserModel> users = new();

                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage pck = new(stream);

                    var worksheet = pck.Workbook.Worksheets[0];

                    users = ConvertToUsersModel(worksheet);

                }

                foreach(UserModel user in users)
                {
                    await GraphProvider.Cosmos.CreateUser(user);
                }

            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        private List<UserModel> ConvertToUsersModel(ExcelWorksheet worksheet)
        {
            List<UserModel> users = new();

            for (int i = 2; i < worksheet.Dimension.End.Row-1; i++)
            {
                users.Add(new()
                {
                    DisplayName= worksheet.Cells[i, 1].Value?.ToString(),
                    FirstName = worksheet.Cells[i, 2].Value?.ToString(),
                    LastName = worksheet.Cells[i, 3].Value?.ToString(),
                    ID = worksheet.Cells[i, 4].Value?.ToString(),
                    Email = worksheet.Cells[i, 5].Value?.ToString(),
                });

            }

            return users;


        }
}

}
