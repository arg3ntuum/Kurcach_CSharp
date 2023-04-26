using System.Runtime.Serialization;

namespace Kursach
{
    public class Const {
        public const string FilesFilter =
            "Image files(*.bmp;*.jpg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.png;*.gif;*.tiff";
        public const string ImageFileName = "image.png";
        public const string ProgramName = "Kursach";
        public partial class Messages { 
            public const string Attention = "Внимание!";
            public const string ImageInThisFile = "Вы хотите сохранить картинку в том же файле или вы предпочтете не сохранять вовсе?";
            public static string IsNull(string str) =>
                $"{str} is null";
        }
    }
}




