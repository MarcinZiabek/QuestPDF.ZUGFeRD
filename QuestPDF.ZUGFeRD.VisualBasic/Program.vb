Imports System.IO
Imports QuestPDF.Fluent
Imports QuestPDF.Infrastructure

Module Program
    Sub Main(args As String())
        ' TODO: Please make sure that you are eligible to use the Community license.
        ' To learn more about the QuestPDF licensing, please visit:
        ' https://www.questpdf.com/pricing.html
        QuestPDF.Settings.License = LicenseType.Community

        
        ' STEP 1: generate sample PDF file
        Document.Create(
            Sub(document)
                document.Page(
                    Sub(page)
                        page.Margin(1, Unit.Centimetre)
                        page.Content().Text("Your invoice content")
                    End Sub
                    )
            End Sub
            ) _
            .WithSettings(New DocumentSettings With {.PdfA = True}) _
            .GeneratePdf("invoice.pdf")

        
        ' STEP 2: attach ZUGFeRD required content

        ' important: provided XML files are only examples,
        ' please ensure their content corresponds to the real information and is compliant with the ZUGFeRD specification
        DocumentOperation.LoadFile("invoice.pdf") _
            .AddAttachment(New DocumentOperation.DocumentAttachment With {
                              .Key = "factur-zugferd",
                              .FilePath = "resource-factur-x.xml", 
                              .AttachmentName = "factur-x.xml",
                              .MimeType = "text/xml",
                              .Description = "Factur-X Invoice",
                              .Relationship = DocumentOperation.DocumentAttachmentRelationship.Source,
                              .CreationDate = DateTime.UtcNow,
                              .ModificationDate = DateTime.UtcNow
                              }) _
            .ExtendMetadata(File.ReadAllText("resource-zugferd-metadata.xml")) _
            .Save("zugferd-invoice-visualbasic.pdf")
        
        
        ' STEP 3: open output file in default reader
        Dim process As New Process With {
                .StartInfo = New ProcessStartInfo("zugferd-invoice-visualbasic.pdf") With {
                .UseShellExecute = True } }

        process.Start()
    End Sub
End Module
