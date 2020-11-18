using System.Drawing;
using Microsoft.ML.Transforms.Image;

namespace AISysML.Model
{  
    public class ModelInput
    {
        public string Label { get; set; }
        
        public uint LabelAsKey { get; set; }
         
        public byte[] Image { get; set; } 
    }
}
