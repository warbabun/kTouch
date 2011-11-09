using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;

namespace KTouch.Units {

    /// <summary>
    /// Thumbnail output format.
    /// </summary>
    public enum OutputFormat { Jpg, Png, Gif }

    /// <summary>
    /// Thumbnail output quality.
    /// </summary>
    public enum OutputQuality { Low, Normal, Good, Super }

    /// <summary>
    /// Size with width and height as integers.
    /// </summary>
    internal struct IntSize {
        public int Width;
        public int Height;
    }

    /// <summary>
    /// Provides methods for generating thumbnails for various file formats.
    /// </summary>
    internal class ThumbnailCreator {

        private double _imageQualityRatio;
        private BitmapEncoder _bitmapEncoder;
        private string _outputFileExtension;

        /// <summary>
        /// Creates a thumbnail for the file.
        /// </summary>
        /// <returns>Thumbnail name with extension.</returns>
        public static string CreateThumbnail(string file) {
            ThumbnailCreator creator = new ThumbnailCreator();
            return creator.ProcessFile(file);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ThumbnailCreator() {
            this.Init();
        }

        /// <summary>
        /// Extended constructor.
        /// </summary>
        /// <param name="outputFormat">Image format for thumbnail.</param>
        /// <param name="outputQuality">Image quality for thumbnail.</param>
        public ThumbnailCreator(OutputFormat outputFormat, OutputQuality outputQuality) {
            this.Init(outputFormat, outputQuality);
        }

        /// <summary>
        /// Initializes private fields.
        /// </summary>
        /// <param name="outputFormat">Image format for thumbnail.</param>
        /// <param name="outputQuality">Image quality for thumbnail.</param>
        private void Init(OutputFormat outputFormat = OutputFormat.Png, OutputQuality outputQuality = OutputQuality.Normal) {
            switch (outputFormat) {
                case OutputFormat.Jpg:
                    _outputFileExtension = SupportedExtensions.JPG;
                    _bitmapEncoder = new JpegBitmapEncoder();
                    break;
                case OutputFormat.Png:
                    _outputFileExtension = SupportedExtensions.PNG;
                    _bitmapEncoder = new PngBitmapEncoder();
                    break;
                case OutputFormat.Gif:
                    _outputFileExtension = SupportedExtensions.GIF;
                    _bitmapEncoder = new GifBitmapEncoder();
                    break;
                default:
                    _outputFileExtension = SupportedExtensions.PNG;
                    _bitmapEncoder = new PngBitmapEncoder();
                    break;
            }
            _imageQualityRatio = 1.0;
            switch (outputQuality) {
                case OutputQuality.Low:
                    _imageQualityRatio /= 2.0;
                    break;
                case OutputQuality.Good:
                    _imageQualityRatio *= 2.0;
                    break;

                case OutputQuality.Super:
                    _imageQualityRatio *= 3.0;
                    break;
                default:
                    _imageQualityRatio *= 1.0;
                    break;
            }
        }

        /// <summary>
        /// Process the file and create a thumbnail.
        /// </summary>
        /// <param name="sourceFile">Source file.</param>
        /// <returns>Thumbnail file full path with extension.</returns>
        private string ProcessFile(string sourceFile) {
            /* TODO: This must be asynchroniously */
            try {
                Visual visual;
                IntSize visualSize;
                double scale = 0.5;
                switch (Path.GetExtension(sourceFile)) {
                    case SupportedExtensions.XPS:
                        visual = this.CreateXpsVisual(sourceFile, out visualSize);
                        break;
                    case SupportedExtensions.AVI:
                    case SupportedExtensions.MP4:
                    case SupportedExtensions.MPG:
                    case SupportedExtensions.WMV:
                        visual = this.CreateVideoVisual(sourceFile, TimeSpan.FromSeconds(10), out visualSize);
                        break;
                    default:
                        return string.Empty;
                }
                int width = (int)(visualSize.Width * _imageQualityRatio) > 0 ? (int)(visualSize.Width * _imageQualityRatio) : 800;
                int height = (int)(visualSize.Height * _imageQualityRatio) > 0 ? (int)(visualSize.Height * _imageQualityRatio) : 600;

                RenderTargetBitmap targetBitmap = new RenderTargetBitmap(width, height, 96.0 * _imageQualityRatio, 96.0 * _imageQualityRatio, PixelFormats.Pbgra32);
                targetBitmap.Render(visual);

                var frame = BitmapFrame.Create(targetBitmap).GetCurrentValueAsFrozen();
                var thumbnailFrame = BitmapFrame.Create(new TransformedBitmap(frame as BitmapFrame, new ScaleTransform(scale, scale))).GetCurrentValueAsFrozen();
                _bitmapEncoder.Frames.Add(thumbnailFrame as BitmapFrame);

                string thumbnailName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(sourceFile), _outputFileExtension);
                string thumbnailFullPath = Path.Combine(Path.GetDirectoryName(sourceFile), thumbnailName);

                using (FileStream fileStream = new FileStream(thumbnailFullPath, FileMode.Create)) {
                    _bitmapEncoder.Save(fileStream);
                }
                return thumbnailFullPath;
            } catch (OutOfMemoryException ex) {
                Console.WriteLine(ex.Message);
                string thumbnailName = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(sourceFile), _outputFileExtension);
                return Path.Combine(Path.GetDirectoryName(sourceFile), thumbnailName);
            }
        }

        /// <summary>
        /// Creates a visual for the xps document.
        /// </summary>
        /// <param name="sourceFile">Source file.</param>
        /// <param name="visualSize">Reference to export dimensions of the visual.</param>
        /// <returns>Visual of the file.</returns>
        private Visual CreateXpsVisual(string sourceFile, out IntSize visualSize) {
            try {
                XpsDocument xpsDocument = new XpsDocument(sourceFile, FileAccess.Read);
                FixedDocumentSequence documentPageSequence = xpsDocument.GetFixedDocumentSequence();
                DocumentPage documentPage = documentPageSequence.DocumentPaginator.GetPage(0);

                visualSize.Width = (int)documentPage.Size.Width;
                visualSize.Height = (int)documentPage.Size.Height;

                xpsDocument.Close();
                return documentPage.Visual;
            } catch {
                throw new Exception();
            }
        }

        /// <summary>
        /// Creates a visual from screencaptures.
        /// </summary>
        /// <param name="sourceFile">Source file.</param>
        /// <param name="timeSpan">Time span where the capture has to be done.</param>
        /// <param name="visualSize">Reference to export dimensions of the visual.</param>
        /// <returns>Visual of the file.</returns>
        private Visual CreateVideoVisual(string sourceFile, TimeSpan timeSpan, out IntSize visualSize) {
            try {
                Uri sourceUri = new Uri(sourceFile);
                var mutexLock = new Mutex(false, sourceUri.GetHashCode().ToString());
                mutexLock.WaitOne();

                MediaPlayer mediaPayer = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
                mediaPayer.Open(sourceUri);

                mediaPayer.Pause();
                mediaPayer.Position = timeSpan;
                Thread.Sleep(1000);

                visualSize.Width = mediaPayer.NaturalVideoWidth;
                visualSize.Height = mediaPayer.NaturalVideoHeight;

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext drawingContext = drawingVisual.RenderOpen()) {
                    drawingContext.DrawVideo(mediaPayer, new Rect(0, 0, visualSize.Width, visualSize.Height));
                }
                mediaPayer.Close();
                mutexLock.ReleaseMutex();
                return drawingVisual;
            } catch {
                throw new Exception();
            }
        }
    }
}
