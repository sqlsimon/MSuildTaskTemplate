using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

/*
 * SOURCE :
 * https://deejaygraham.github.io/2013/04/14/custom-msbuild-task-template/
 *
 * 
 */

public class MyTask : Task
{
    [Required]
    public ITaskItem[] Files { get; set; }

    [Required]
    public ITaskItem OutputFolder { get; set; }

    public override bool Execute()
    {
        var files = new List<string>();

        if (this.Files.Length > 0)
        {
            for (int i = 0; i < this.Files.Length; ++i)
            {
                ITaskItem item = this.Files[i];
                string path = item.GetMetadata("FullPath");

                if (File.Exists(path))
                {
                    files.Add(path);
                }
            }
        }

        if (!files.Any())
        {
            Log.LogWarning("No files found to transform");
        }

        string outputFolder = string.Empty;

        if (this.OutputFolder != null)
        {
            outputFolder = this.OutputFolder.GetMetadata("FullPath");
        }

        if (!string.IsNullOrEmpty(outputFolder))
        {
            if (!Directory.Exists(outputFolder))
            {
                Log.LogMessage("Creating folder: \"{0}\"", outputFolder);
                Directory.CreateDirectory(outputFolder);
            }
        }

        // other useful stuff...

        return true;
    }
}