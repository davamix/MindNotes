using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Models;
public class Settings {
    public string OllamaServerAddress { get; set; } = string.Empty;
    public string QdrantServerAddress { get; set; } = string.Empty;
    public string EmbeddingModelName { get; set; } = string.Empty;
    public string OutputVectorSize { get; set; } = string.Empty;
}
