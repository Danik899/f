using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using KBIPMobileBackend.Data;
using KBIPMobileBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.IO;

namespace KBIPMobileBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ApplicationDbContext db, ILogger<ReportsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // Метод для создания PDF отчета
        [HttpGet("generate-pdf")]
        public IActionResult GenerateReport()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    // Создание PdfWriter для записи в поток
                    var writer = new PdfWriter(ms);
                    var pdf = new PdfDocument(writer);

                    // Создание документа для работы с содержимым
                    var document = new Document(pdf);

                    // Добавляем заголовок
                    document.Add(new Paragraph("Отчет о студентах и расписании"));

                    // Добавление списка студентов
                    var students = _db.Students.ToList();
                    if (!students.Any())
                    {
                        _logger.LogWarning("No students found in the database.");
                        document.Add(new Paragraph("Нет студентов в базе данных."));
                    }
                    else
                    {
                        document.Add(new Paragraph("\nСписок студентов:"));
                        var studentTable = new Table(3);
                        studentTable.AddHeaderCell("ID");
                        studentTable.AddHeaderCell("Имя");
                        studentTable.AddHeaderCell("Группа");

                        foreach (var student in students)
                        {
                            studentTable.AddCell(student.Id.ToString());
                            studentTable.AddCell(student.FullName);
                            studentTable.AddCell(student.GroupNumber);
                        }
                        document.Add(studentTable);
                    }

                    // Добавление списка расписания
                    var schedule = _db.ScheduleEntries.ToList();
                    if (!schedule.Any())
                    {
                        _logger.LogWarning("No schedule entries found in the database.");
                        document.Add(new Paragraph("Нет расписания в базе данных."));
                    }
                    else
                    {
                        document.Add(new Paragraph("\nРасписание:"));
                        var scheduleTable = new Table(4);
                        scheduleTable.AddHeaderCell("ID");
                        scheduleTable.AddHeaderCell("Группа");
                        scheduleTable.AddHeaderCell("Район");
                        scheduleTable.AddHeaderCell("Предмет");

                        foreach (var entry in schedule)
                        {
                            scheduleTable.AddCell(entry.Id.ToString());
                            scheduleTable.AddCell(entry.GroupNumber);
                            scheduleTable.AddCell(entry.Location);  // если это поле есть в модели ScheduleEntry
                            scheduleTable.AddCell(entry.Subject);   // если это поле есть в модели ScheduleEntry
                        }
                        document.Add(scheduleTable);
                    }

                    // Завершаем создание PDF
                    document.Close();

                    // Отправляем файл пользователю
                    return File(ms.ToArray(), "application/pdf", "report.pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF report");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
