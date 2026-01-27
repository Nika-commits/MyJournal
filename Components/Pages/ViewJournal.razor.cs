using Microsoft.AspNetCore.Components;
using MyJournal.Components.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyJournal.Components.Pages
{
    public partial class ViewJournal
    {
        [Parameter] public Guid Id { get; set; }
        private Journal? Entry;

        protected override async Task OnInitializedAsync()
        {
            Entry = await DbService.GetJournalByIdAsync(Id);
        }

        private void GoBack() => NavManager.NavigateTo("/myJournals");


        private void GoToEdit()
        {
        }

        private async Task ExportEntry()
        {
            if (Entry == null) return;

            var htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{ font-family: 'Segoe UI', sans-serif; padding: 40px; color: #333; }}
                    
                    h1 {{ 
                        font-family: serif; 
                        font-size: 28pt; 
                        border-bottom: 2px solid #8FA885; 
                        padding-bottom: 10px; 
                        color: #2F3E2E; 
                        margin-bottom: 5px; 
                    }}
                    
                    .meta {{ 
                        color: #666; 
                        font-size: 11pt; 
                        margin-bottom: 30px; 
                        font-style: italic; 
                    }}

                    .content {{ 
                        font-size: 12pt; 
                        line-height: 1.6; 
                    }}
                    
                    img {{ 
                        max-width: 100%; 
                        height: auto; 
                        border-radius: 8px; 
                        margin: 10px 0;
                    }}

                    .tags {{ 
                        margin-top: 50px; 
                        border-top: 1px dashed #ccc; 
                        padding-top: 10px; 
                        color: #555; 
                        font-size: 10pt; 
                    }}

                    /* WINDOWS & PRINT FIXES */
                    @media print {{
                        body {{ -webkit-print-color-adjust: exact; print-color-adjust: exact; }} 
                    }}
                </style>
            </head>
            <body>
                <h1>{Entry.Title}</h1>
                
                <div class='meta'>
                    <span> {Entry.EntryDate:D}</span> &nbsp;|&nbsp; 
                    <span>Mood: {Entry.PrimaryMood}</span>
                </div>

                <div class='content'>
                    {Entry.Content} 
                </div>

                <div class='tags'>
                    <strong>Tags:</strong> {Entry.Tags?.Replace(",", ", ")}
                </div>
            </body>
            </html>";

            await PrintService.Print($"Journal_{Entry.EntryDate:yyyyMMdd}", htmlContent);
        }
    }
}
