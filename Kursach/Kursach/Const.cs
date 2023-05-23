using System.Runtime.Serialization;

namespace Kursach
{
    public class Const {
        public const string FilesFilter =
            "Image files(*.bmp;*.jpg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.png;*.gif;*.tiff";
        public const string ImageFileName = "image.png";
        public const string ProgramName = "Kursach";
        public partial class Messages { 
            public const string Attention = "Увага!";
            public const string CreateFormToViewImage = "Ви бажаєте створити форму для відображення картинки?";
            public const string ImageInThisFile = "Ви хочете зберегти картинку в тому ж файлі або ви не хочете зберігати зовсім?";
            public const string DownloadImageToBuffer = "Ви хочете підвантажити змінену версію цієї картинки в ImageBuffer?";
            public const string FileNotWasChanged = "Файл не змінено.";
            public const string ActiveFormIsNull = "Активної форми немає!";
            public const string ImageBufferIsNull = "Буфер картинки порожній! Можливо, її не існує на формі!";
            public const string ImageBufferAddIsNull = "Буфер картинки на головній формі, що повинна об`єднуватися з цією формою пустий!";
            public const string ImagesIsEqual = "Не можна об'єднувати однакові зображення!";
            public const string IsNullOrWhiteSpace = "Не задано шлях для збереження!";
        }
    }
}