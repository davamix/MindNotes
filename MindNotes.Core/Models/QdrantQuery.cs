using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNotes.Core.Models;
public class QdrantQuery(List<float[]> vectorQuery, string textQuery) {
    public float[] VectorQuery => vectorQuery.SelectMany(x=>x).ToArray();
    public string TextQuery => textQuery;
}
