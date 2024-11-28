using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

// TODO: Please make sure that you are eligible to use the Community license.
// To learn more about the QuestPDF licensing, please visit:
// https://www.questpdf.com/pricing.html
QuestPDF.Settings.License = LicenseType.Community;
     

// STEP 1: generate sample PDF file
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Margin(1, Unit.Centimetre);
            page.Content().Text("Your invoice content");
        });
    })
    .WithSettings(new DocumentSettings { PdfA = true }) // PDF/A-3b
    .GeneratePdf("invoice.pdf");
        
// STEP 2: attach ZUGFeRD required content

// important: provided XML files are only examples,
// please ensure their content corresponds to the real information and is compliant with the ZUGFeRD specification
DocumentOperation
    .LoadFile("invoice.pdf")
    .AddAttachment(new DocumentOperation.DocumentAttachment
    {
        Key = "factur-zugferd",
        FilePath = "resource-factur-x.xml",
        AttachmentName = "factur-x.xml",
        MimeType = "text/xml",
        Description = "Factur-X Invoice",
        Relationship = DocumentOperation.DocumentAttachmentRelationship.Source,
        CreationDate = DateTime.UtcNow,
        ModificationDate = DateTime.UtcNow
    })
    .ExtendMetadata(File.ReadAllText("resource-zugferd-metadata.xml"))
    .Save("zugferd-invoice.pdf");
    

// STEP 3: open output file in default reader
var process = new Process
{
    StartInfo = new ProcessStartInfo("zugferd-invoice.pdf")
    {
        UseShellExecute = true
    }
};

process.Start();   