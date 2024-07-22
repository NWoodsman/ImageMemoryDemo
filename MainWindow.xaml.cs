using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ScaleImage
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var canvas = this.FindName("TheCanvas") as Canvas;

			VHost vhost = new VHost();

			canvas.Children.Add(vhost);

			var dc = vhost.RenderOpen();

			string path = @"..\..\..\Arc_de_Triomphe_ABZ_Wien_1838_Plan_180.jpg";

			BitmapImage image = new BitmapImage();



			using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;
				image.EndInit();
				image.DecodePixelWidth = Convert.ToInt32(image.Width);
				image.DecodePixelHeight = Convert.ToInt32(image.Height);
			}

			ImageSource isrc = image;

			isrc.Freeze();

			var ratio = isrc.Width / isrc.Height;

			Rect imagerect = new Rect(new Point(-3000, -2000), new Size(isrc.Width, isrc.Height));

			ImageDrawing ig = new ImageDrawing(isrc, imagerect);

			ig.ImageSource = isrc;

			ig.Freeze();

			dc.DrawDrawing(ig);

			/*context.dc.Pop();
			context.dc.Pop();*/

			dc.Close();
		}
	}

	class VHost:UIElement
	{
		internal DrawingVisual MyVisual { get; init; } = new DrawingVisual();

		internal VisualCollection Visuals { get; init; }

		protected override Visual GetVisualChild(int index) => Visuals[index];

		protected override int VisualChildrenCount => Visuals.Count;
		public DrawingContext RenderOpen() => MyVisual.RenderOpen();
	
		public VHost()
		{
			Visuals = new VisualCollection(this)
			{
				MyVisual
			};
		}

	}
}