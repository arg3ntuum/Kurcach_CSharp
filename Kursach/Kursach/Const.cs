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
            public const string CreateFormToViewImage = "Вы хотите создать форму для отображения картинки?";
            public const string ImageInThisFile = "Вы хотите сохранить картинку в том же файле или вы предпочтете не сохранять вовсе?";
            public const string DownloadImageToBuffer = "Вы хотите подгрузить измененную версию этой картинки в ImageBuffer?";
            public const string FileNotWasChanged = "Файл не был изменен.";
            public const string ActiveFormIsNull = "Активной формы не существует!";
            public const string ImageBuffesIsNull = "Буфер картинки пуст! Возможно её не существует на форме!";
            public const string IsNullOrWhiteSpace = "Не задан путь для сохранения!";
            public static string ObjectIsNull(object someObject) { 
                return someObject.ToString() + "пуст!";
            }
        }
    }
}




